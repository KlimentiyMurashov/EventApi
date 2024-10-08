using Application.Services;
using Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly UserManager<User> _userManager;
	private readonly JwtService _jwtService; 

	public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, JwtService jwtService)
	{
		_userManager = userManager;
		_jwtService = jwtService;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
	{
		var user = await _userManager.FindByEmailAsync(loginRequest.Email);
		if (user == null) return Unauthorized("Invalid email or password.");

		var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
		if (!result) return Unauthorized("Invalid email or password.");

		var accessToken = _jwtService.GenerateAccessToken(user);
		var refreshToken = _jwtService.GenerateRefreshToken();

		return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
	}

}