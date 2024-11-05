using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly UserManager<User> _userManager;

		public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<User> userManager)
		{
			_jwtSettings = jwtSettings.Value;
			_userManager = userManager;
		}

		public async Task<string> GenerateAccessTokenAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException(nameof(user));
			
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("e5d4c67d9822a63c9259d0196c940d2b3890a56f7a1b5dbb2e8c3e7d6f1a30c0"));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
				}),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
				IssuedAt = DateTime.UtcNow, 
				NotBefore = DateTime.UtcNow.AddSeconds(-1),
				Issuer = _jwtSettings.Issuer,
				Audience = _jwtSettings.Audience,
				SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		public async Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken)
		{
			var principal = GetPrincipalFromExpiredToken(token);
			var userId = principal.Identity.Name;

			var user = await _userManager.FindByIdAsync(userId);

			if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
				throw new SecurityTokenException("Invalid refresh token");

			var newAccessToken = await GenerateAccessTokenAsync(user);
			var newRefreshToken = GenerateRefreshToken();

			user.RefreshToken = newRefreshToken;
			await _userManager.UpdateAsync(user);

			return new AuthResponseDto
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken
			};
		}

		private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _jwtSettings.Issuer,
				ValidAudience = _jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateLifetime = false
			};

			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;

			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}
	}

}
