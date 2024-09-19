using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
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
			return await _context.Participants
				.Include(p => p.EventRegistrations)  
				.ThenInclude(er => er.Event)        
				.ToListAsync();
		}

		public async Task<Participant> GetParticipantByIdAsync(int id)
		{
			return await _context.Participants
				.Include(p => p.EventRegistrations)
				.ThenInclude(er => er.Event)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddParticipantAsync(Participant participant)
		{
			await _context.Participants.AddAsync(participant);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateParticipantAsync(Participant updatedParticipant)
		{
			_context.Participants.Update(updatedParticipant);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteParticipantByIdAsync(int id)
		{
			var participantToDelete = await GetParticipantByIdAsync(id);
			if (participantToDelete != null)
			{
				_context.Participants.Remove(participantToDelete);
				await _context.SaveChangesAsync();
			}
		}

		public async Task RegisterParticipantForEventAsync(int participantId, int eventId)
		{
			var participant = await GetParticipantByIdAsync(participantId);
			var eventEntity = await _context.Events.FindAsync(eventId);

			if (participant != null && eventEntity != null)
			{
				var existingRegistration = await _context.EventRegistrations
					.FirstOrDefaultAsync(er => er.ParticipantId == participantId && er.EventId == eventId);

				if (existingRegistration == null)
				{
					var eventRegistration = new EventRegistration
					{
						ParticipantId = participantId,
						EventId = eventId,
						RegistrationDate = DateTime.Now
					};

					_context.EventRegistrations.Add(eventRegistration);
					await _context.SaveChangesAsync();
				}
			}
		}

		public async Task CancelParticipationAsync(int participantId, int eventId)
		{
			var registration = await _context.EventRegistrations
				.FirstOrDefaultAsync(er => er.ParticipantId == participantId && er.EventId == eventId);

			if (registration != null)
			{
				_context.EventRegistrations.Remove(registration);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Event>> GetEventsByParticipantIdAsync(int participantId)
		{
			var participant = await GetParticipantByIdAsync(participantId);
			return participant?.EventRegistrations.Select(er => er.Event) ?? Enumerable.Empty<Event>();
		}

	}

}
