using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services
{
	public interface IEventService
	{
		Task<IEnumerable<EventDto>> GetAllEventsAsync();
		Task<EventDto> GetEventByIdAsync(int id);
		Task<int> AddEventAsync(EventDto eventDto);
		Task UpdateEventAsync(EventDto eventDto);
		Task DeleteEventAsync(int id);
	}

}
