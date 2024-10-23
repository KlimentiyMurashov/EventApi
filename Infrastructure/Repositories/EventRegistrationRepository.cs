using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class EventRegistrationRepository : IEventRegistrationRepository
	{
		private readonly ApplicationDbContext _context;

		public EventRegistrationRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<EventRegistration?> GetRegistrationAsync(int participantId, int eventId)
		{
			return await _context.EventRegistrations
				.FirstOrDefaultAsync(er => er.ParticipantId == participantId && er.EventId == eventId);
		}

		public async Task AddRegistrationAsync(EventRegistration registration)
		{
			var existingRegistration = await _context.EventRegistrations
					.FirstOrDefaultAsync(r => r.ParticipantId == registration.ParticipantId && r.EventId == registration.EventId);
			await _context.EventRegistrations.AddAsync(registration);
		}

		public async Task RemoveRegistrationAsync(EventRegistration registration)
		{
			_context.EventRegistrations.Remove(registration);
		}
	}

}
