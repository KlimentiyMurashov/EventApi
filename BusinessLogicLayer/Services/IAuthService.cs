using Application.DTOs;


namespace Application.Services
{
	public interface IAuthService
	{
		Task<TokenResponse> GenerateTokensAsync(string userId, string email);
		Task<TokenResponse> RefreshTokensAsync(string refreshToken);
	}

}
