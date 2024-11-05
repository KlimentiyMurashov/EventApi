using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ParticipantRepository : IParticipantRepository
	{
		private readonly ApplicationDbContext _context;

		public ParticipantRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Participant>> GetAllParticipantsAsync()
		{
			var participants = await _context.Participants
				.Include(p => p.EventRegistrations)
				.ThenInclude(er => er.Event)
				.ToListAsync();
			return participants;
		}

		public async Task<Participant> GetParticipantByIdAsync(int id)
		{
			var participant = await _context.Participants
				.Include(p => p.EventRegistrations)
				.ThenInclude(er => er.Event)
				.FirstOrDefaultAsync(p => p.Id == id);
			return participant;
		}

		public async Task AddParticipantAsync(Participant participant)
		{
			await _context.Participants.AddAsync(participant);
		}

		public async Task UpdateParticipantAsync(Participant updatedParticipant)
		{
			var existingParticipant = await GetParticipantByIdAsync(updatedParticipant.Id);
			_context.Participants.Update(updatedParticipant);
		}

		public async Task DeleteParticipantByIdAsync(int id)
		{
			var participantToDelete = await GetParticipantByIdAsync(id);
			_context.Participants.Remove(participantToDelete);
		}

		public async Task<bool> EmailExistsAsync(string email)
		{
			return await _context.Participants.AnyAsync(p => p.Email == email);
		}

	}
}
