using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private readonly ApplicationDbContext _context;

		public RefreshTokenRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task SaveRefreshTokenAsync(RefreshToken token)
		{
			_context.RefreshTokens.Add(token);
			await _context.SaveChangesAsync();
		}

		public async Task<RefreshToken> GetRefreshTokenAsync(string token)
		{
			return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
		}

		public async Task UpdateRefreshTokenAsync(RefreshToken token)
		{
			_context.RefreshTokens.Update(token);
			await _context.SaveChangesAsync();
		}
	}

}
