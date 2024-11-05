using Application.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;

namespace Application.UseCase
{
	public class LoginUseCase
	{
		private readonly UserManager<User> _userManager;
		private readonly IAuthService _authService;

		public LoginUseCase(UserManager<User> userManager, IAuthService authService)
		{
			_userManager = userManager;
			_authService = authService;
		}

		public async Task<AuthResponseDto> ExecuteAsync(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null) throw new UnauthorizedAccessException("Invalid email or password.");

			var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
			if (!result) throw new UnauthorizedAccessException("Invalid email or password.");

			var accessToken = await _authService.GenerateAccessTokenAsync(user);
			var refreshToken = _authService.GenerateRefreshToken();

			user.RefreshToken = refreshToken;
			await _userManager.UpdateAsync(user);

			return new AuthResponseDto { AccessToken = accessToken, RefreshToken = refreshToken };
		}
	}
}
