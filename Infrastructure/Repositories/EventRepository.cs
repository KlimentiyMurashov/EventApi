using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly ApplicationDbContext _context;

		public EventRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Event>> GetAllEventsAsync()
		{
			var events = await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)
				.ToListAsync();

			if (events == null || !events.Any())
				throw new InvalidOperationException("No events found.");

			return events;
		}

		public async Task<Event> GetEventByIdAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentException("Invalid event ID.", nameof(id));

			var eventItem = await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (eventItem == null)
				throw new InvalidOperationException($"Event with ID {id} not found.");

			return eventItem;
		}

		public async Task<Event?> GetEventByNameAsync(string eventName)
		{
			if (string.IsNullOrEmpty(eventName))
				throw new ArgumentException("Event name cannot be null or empty.", nameof(eventName));

			var eventItem = await _context.Events
				.Include(e => e.EventRegistrations)
				.ThenInclude(er => er.Participant)
				.FirstOrDefaultAsync(e => e.Title == eventName);

			if (eventItem == null)
				throw new InvalidOperationException($"Event with name {eventName} not found.");

			return eventItem;
		}

		public async Task AddEventAsync(Event newEvent)
		{
			if (newEvent == null)
				throw new ArgumentNullException(nameof(newEvent));

			await _context.Events.AddAsync(newEvent);
		}

		public async Task UpdateEventAsync(Event updatedEvent)
		{
			if (updatedEvent == null)
				throw new ArgumentNullException(nameof(updatedEvent));

			var existingEvent = await GetEventByIdAsync(updatedEvent.Id);
			if (existingEvent == null)
				throw new InvalidOperationException($"Event with ID {updatedEvent.Id} not found.");

			_context.Events.Update(updatedEvent);
		}

		public async Task DeleteEventByIdAsync(int eventId)
		{
			if (eventId <= 0)
				throw new ArgumentException("Invalid event ID.", nameof(eventId));

			var eventToDelete = await GetEventByIdAsync(eventId);
			if (eventToDelete == null)
				throw new InvalidOperationException($"Event with ID {eventId} not found.");

			_context.Events.Remove(eventToDelete);
		}

		public async Task<IEnumerable<Event>> GetEventsByCriteriesAsync(DateTime? date = null, string? location = null, string? category = null)
		{
			var query = _context.Events.AsQueryable();

			if (date.HasValue)
				query = query.Where(e => e.DateTime.Date == date.Value.Date);

			if (!string.IsNullOrEmpty(location))
				query = query.Where(e => e.Location.Contains(location));

			if (!string.IsNullOrEmpty(category))
				query = query.Where(e => e.Category.Contains(category));

			var events = await query.ToListAsync();

			if (events == null || !events.Any())
				return new List<Event>();

			return events;
		}

		public async Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId)
		{
			var eventEntity = await GetEventByIdAsync(eventId);
			return eventEntity.EventRegistrations.Select(er => er.Participant);
		}

		public async Task<bool> IsTitleUniqueAsync(string title)
		{
			return !await _context.Events.AnyAsync(e => e.Title == title);
		}
	}
}
