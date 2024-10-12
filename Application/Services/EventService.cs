using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.UoW;

namespace Application.Services
{
	public class EventService : IEventService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EventService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
		{
			var events = await _unitOfWork.EventRepository.GetAllEventsAsync();
			return _mapper.Map<IEnumerable<EventDto>>(events);
		}

		public async Task<EventDto> GetEventByIdAsync(int id)
		{
			var eventItem = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
			return _mapper.Map<EventDto>(eventItem);
		}

		public async Task<EventDto> GetEventByNameAsync(string eventName)
		{
			var eventItem = await _unitOfWork.EventRepository.GetEventByNameAsync(eventName);
			return _mapper.Map<EventDto>(eventItem);
		}

		public async Task<int> AddEventAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var eventEntity = _mapper.Map<Event>(eventDto);
			await _unitOfWork.EventRepository.AddEventAsync(eventEntity);
			await _unitOfWork.CommitAsync();
			return eventEntity.Id;
		}

		public async Task UpdateEventAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var eventItem = _mapper.Map<Event>(eventDto);
			await _unitOfWork.EventRepository.UpdateEventAsync(eventItem);
			await _unitOfWork.CommitAsync();
		}

		public async Task DeleteEventAsync(int id)
		{
			await _unitOfWork.EventRepository.DeleteEventByIdAsync(id);
			await _unitOfWork.CommitAsync();
		}

		public async Task<IEnumerable<EventDto>> GetEventsByCriteriesAsync(DateTime? date = null, string? location = null, string? category = null)
		{
			var events = await _unitOfWork.EventRepository.GetEventsByCriteriesAsync(date, location, category);
			return _mapper.Map<IEnumerable<EventDto>>(events);
		}

		public async Task<IEnumerable<ParticipantDto>> GetParticipantsByEventIdAsync(int eventId)
		{
			var participants = await _unitOfWork.EventRepository.GetParticipantsByEventIdAsync(eventId);
			return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
		}

		public async Task AddImageUrlToEventAsync(int eventId, string imageUrl)
		{
			var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);

			if (eventEntity == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			eventEntity.ImageUrl = imageUrl;
			await _unitOfWork.CommitAsync();
		}

		public async Task<bool> IsTitleUniqueAsync(string title)
		{
			return await _unitOfWork.EventRepository.IsTitleUniqueAsync(title);
		}
	}
}
