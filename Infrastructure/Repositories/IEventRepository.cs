using Domain.Entities;


namespace Infrastructure.Repositories
{
	public interface IEventRepository
	{
		Task<IEnumerable<Event>> GetAllEventsAsync();
		Task<Event> GetEventByIdAsync(int id);
		Task<Event?> GetEventByNameAsync(string eventName);
		Task AddEventAsync(Event newEvent);
		Task UpdateEventAsync(Event updatedEvent);
		Task DeleteEventByIdAsync(int eventId);

		Task<IEnumerable<Event>> GetEventsByCriteriesAsync(
			DateTime? date = null,
			string? location = null,
			string? category = null
		);


		Task<IEnumerable<Participant>> GetParticipantsByEventIdAsync(int eventId);
	}

}
