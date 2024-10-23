using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly UserManager<User> _userManager;
	private readonly IAuthService _authService;

	public AuthController(UserManager<User> userManager, IAuthService authService)
	{
		_userManager = userManager;
		_authService = authService;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
	{
		var user = await _userManager.FindByEmailAsync(loginRequest.Email);
		if (user == null) return Unauthorized("Invalid email or password.");

		var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
		if (!result) return Unauthorized("Invalid email or password.");

		var accessToken = await _authService.GenerateAccessTokenAsync(user);
		var refreshToken = _authService.GenerateRefreshToken();

		user.RefreshToken = refreshToken;
		await _userManager.UpdateAsync(user);

		return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshDto tokenRefreshRequest)
	{
		try
		{
			var result = await _authService.RefreshTokenAsync(tokenRefreshRequest.AccessToken, tokenRefreshRequest.RefreshToken);
			return Ok(result);
		}
		catch (SecurityTokenException ex)
		{
			return Unauthorized(ex.Message);
		}
	}
}
