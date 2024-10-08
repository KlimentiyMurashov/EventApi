using Application.DTOs;

namespace Application.Services
{
	public interface IEventService
	{
		Task<IEnumerable<EventDto>> GetAllEventsAsync();
		Task<EventDto> GetEventByIdAsync(int id);
		Task<int> AddEventAsync(EventDto eventDto);
		Task UpdateEventAsync(EventDto eventDto);
		Task DeleteEventAsync(int id);
		Task<IEnumerable<EventDto>> GetEventsByCriteriesAsync(DateTime? date = null, string location = null, string category = null);
		Task<IEnumerable<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId);
		Task AddImageUrlToEventAsync(int eventId, string imageUrl);
	}

}
