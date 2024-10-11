using Domain.Entities;

namespace Infrastructure.Repositories
{
	public interface IRefreshTokenRepository
	{
		Task SaveRefreshTokenAsync(RefreshToken token);
		Task<RefreshToken> GetRefreshTokenAsync(string token);
		Task UpdateRefreshTokenAsync(RefreshToken token);
	}

}
