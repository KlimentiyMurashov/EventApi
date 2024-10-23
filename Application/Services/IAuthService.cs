using Application.DTOs;


namespace Application.Services
{
	public interface IAuthService
	{
		Task<string> GenerateAccessTokenAsync(User user);
		string GenerateRefreshToken();
		Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
	}


}
