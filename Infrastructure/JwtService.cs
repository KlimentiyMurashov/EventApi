using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
	public class JwtService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly IConfiguration _configuration;


		public JwtService(IConfiguration configuration)
		{
			_jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
			_configuration = configuration;
		}

		public string GenerateAccessToken(User user)
		{
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public string GenerateRefreshToken()
		{
			return Guid.NewGuid().ToString();
		}

		public ClaimsPrincipal ValidateToken(string token)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

			try
			{
				Console.WriteLine($"Token to validate: {token}");

				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ClockSkew = TimeSpan.Zero 
				};

				Console.WriteLine($"Expected Issuer: {jwtSettings.Issuer}");
				Console.WriteLine($"Expected Audience: {jwtSettings.Audience}");
				Console.WriteLine($"Expected Secret Key: {jwtSettings.SecretKey}");

				var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

				Console.WriteLine("Token successfully validated.");
				return principal;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Token validation failed: {ex.Message}");
				return null; 
			}
		}

	}
}
