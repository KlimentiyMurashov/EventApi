using Infrastructure.Services;
using Application.DTOs;
using Application.Interfaces;
using Application.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Application.UseCase;
using Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;
	private readonly LoginUseCase _loginUseCase;

	public AuthController(IAuthService authService, LoginUseCase loginUseCase)
	{
		_authService = authService;
		_loginUseCase = loginUseCase;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		var authResponse = await _loginUseCase.ExecuteAsync(loginDto);
		return Ok(authResponse);
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshDto tokenRefreshRequest)
	{
		var result = await _authService.RefreshTokenAsync(tokenRefreshRequest.AccessToken, tokenRefreshRequest.RefreshToken);
		return Ok(result);
	}
}
