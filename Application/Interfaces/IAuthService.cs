using Application.DTOs;


namespace Application.Interfaces
{
	public interface IAuthService
	{
		Task<string> GenerateAccessTokenAsync(User user);
		string GenerateRefreshToken();
		Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
	}


}
