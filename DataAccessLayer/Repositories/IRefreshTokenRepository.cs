using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public interface IRefreshTokenRepository
	{
		Task SaveRefreshTokenAsync(RefreshToken token);
		Task<RefreshToken> GetRefreshTokenAsync(string token);
		Task UpdateRefreshTokenAsync(RefreshToken token);
	}

}
