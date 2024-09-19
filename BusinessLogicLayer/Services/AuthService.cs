using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public AuthService(UserManager<User> userManager, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
		{
			_userManager = userManager;
			_configuration = configuration;
			_refreshTokenRepository = refreshTokenRepository;
		}

		public async Task<TokenResponse> GenerateTokensAsync(string userId, string email)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var accessToken = new JwtSecurityToken(
				issuer: jwtSettings.Issuer,
				audience: jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpiryMinutes),
				signingCredentials: creds
			);

			var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);

			var refreshToken = new RefreshToken
			{
				Token = Guid.NewGuid().ToString(),
				UserId = userId,
				ExpiryDate = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpiryDays)
			};

			await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

			return new TokenResponse
			{
				AccessToken = accessTokenString,
				RefreshToken = refreshToken.Token
			};
		}

		public async Task<TokenResponse> RefreshTokensAsync(string refreshToken)
		{
			var existingToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);

			if (existingToken == null || existingToken.ExpiryDate < DateTime.UtcNow || existingToken.IsRevoked || existingToken.IsUsed)
			{
				throw new UnauthorizedAccessException("Invalid refresh token.");
			}

			var user = await _userManager.FindByIdAsync(existingToken.UserId);

			existingToken.IsUsed = true;
			await _refreshTokenRepository.UpdateRefreshTokenAsync(existingToken);

			return await GenerateTokensAsync(user.Id, user.Email);
		}
	}

}
