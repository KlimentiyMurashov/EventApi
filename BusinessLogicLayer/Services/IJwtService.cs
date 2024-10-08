using System.Security.Claims;

namespace Application.Services
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user);
		string GenerateRefreshToken();
		ClaimsPrincipal ValidateToken(string token);
	}
}
