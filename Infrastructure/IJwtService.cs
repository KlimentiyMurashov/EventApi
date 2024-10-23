using System.Security.Claims;

namespace Infrastructure
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user);
		string GenerateRefreshToken();
		ClaimsPrincipal ValidateToken(string token);
	}
}
