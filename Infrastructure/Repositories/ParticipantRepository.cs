using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class ParticipantRepository : IParticipantRepository
	{
		private readonly ApplicationDbContext _context;

		public ParticipantRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Participant>> GetAllParticipantsAsync()
		{
			var participants = await _context.Participants
				.Include(p => p.EventRegistrations)
				.ThenInclude(er => er.Event)
				.ToListAsync();

			if (participants == null || !participants.Any())
				throw new InvalidOperationException("No participants found.");

			return participants;
		}

		public async Task<Participant> GetParticipantByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentException("Invalid participant ID.", nameof(id));

			var participant = await _context.Participants
				.Include(p => p.EventRegistrations)
				.ThenInclude(er => er.Event)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (participant == null)
				throw new InvalidOperationException($"Participant with ID {id} not found.");

			return participant;
		}

		public async Task AddParticipantAsync(Participant participant)
		{
			if (participant == null)
				throw new ArgumentNullException(nameof(participant));

			await _context.Participants.AddAsync(participant);
		}

		public async Task UpdateParticipantAsync(Participant updatedParticipant)
		{
			if (updatedParticipant == null)
				throw new ArgumentNullException(nameof(updatedParticipant));

			var existingParticipant = await GetParticipantByIdAsync(updatedParticipant.Id);
			if (existingParticipant == null)
				throw new InvalidOperationException($"Participant with ID {updatedParticipant.Id} not found.");

			_context.Participants.Update(updatedParticipant);
		}

		public async Task DeleteParticipantByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentException("Invalid participant ID.", nameof(id));

			var participantToDelete = await GetParticipantByIdAsync(id);
			if (participantToDelete == null)
				throw new InvalidOperationException($"Participant with ID {id} not found.");

			_context.Participants.Remove(participantToDelete);
		}

		public async Task<bool> EmailExistsAsync(string email)
		{
			return await _context.Participants.AnyAsync(p => p.Email == email);
		}

	}
}
