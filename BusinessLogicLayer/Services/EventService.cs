using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;
		private readonly IMapper _mapper;

		public EventService(IEventRepository eventRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
		{
			var events = await _eventRepository.GetAllEventsAsync();
			return _mapper.Map<IEnumerable<EventDto>>(events);
		}

		public async Task<EventDto> GetEventByIdAsync(int id)
		{
			var eventItem = await _eventRepository.GetEventByIdAsync(id);
			return _mapper.Map<EventDto>(eventItem);
		}

		public async Task<int> AddEventAsync(EventDto eventDto)
		{
			var eventEntity = _mapper.Map<Event>(eventDto);
			await _eventRepository.AddEventAsync(eventEntity);
			return eventEntity.Id; 
		}

		public async Task UpdateEventAsync(EventDto eventDto)
		{
			var eventItem = _mapper.Map<Event>(eventDto);
			await _eventRepository.UpdateEventAsync(eventItem);
		}

		public async Task DeleteEventAsync(int id)
		{
			await _eventRepository.DeleteEventByIdAsync(id);
		}
	}

}
