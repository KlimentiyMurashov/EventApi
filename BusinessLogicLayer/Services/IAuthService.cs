using BusinessLogicLayer.DTOs;


namespace BusinessLogicLayer.Services
{
	public interface IAuthService
	{
		Task<TokenResponse> GenerateTokensAsync(string userId, string email);
		Task<TokenResponse> RefreshTokensAsync(string refreshToken);
	}

}
