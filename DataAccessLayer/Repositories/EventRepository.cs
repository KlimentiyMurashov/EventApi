using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly ApplicationDbContext _context;

		public EventRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Event>> GetAllEventsAsync()
		{
			return await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)  
				.ToListAsync();
		}

		public async Task<Event> GetEventByIdAsync(int id)
		{
			return await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)  
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<Event?> GetEventByNameAsync(string eventName)
		{
			return await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)  
				.FirstOrDefaultAsync(e => e.Title == eventName);
		}

		public async Task AddEventAsync(Event newEvent)
		{
			await _context.Events.AddAsync(newEvent);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateEventAsync(Event updatedEvent)
		{
			_context.Events.Update(updatedEvent);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteEventByIdAsync(int eventId)
		{
			var eventToDelete = await GetEventByIdAsync(eventId);
			if (eventToDelete != null)
			{
				_context.Events.Remove(eventToDelete);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Event>> GetEventsByCriteriesAsync(DateTime? date = null, string? location = null, string? category = null)
		{
			var query = _context.Events.AsQueryable();

			if (date.HasValue)
				query = query.Where(e => e.DateTime.Date == date.Value.Date);

			if (!string.IsNullOrEmpty(location))
				query = query.Where(e => e.Location == location);

			if (!string.IsNullOrEmpty(category))
				query = query.Where(e => e.Category == category);

			return await query
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant) 
				.ToListAsync();
		}

		public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
		{
			var eventEntity = await GetEventByIdAsync(eventId);
			return eventEntity?.EventRegistrations.Select(er => er.Participant) ?? Enumerable.Empty<Participant>();
		}

	}

}
