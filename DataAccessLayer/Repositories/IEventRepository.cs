using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
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
