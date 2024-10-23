using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Application.Interfaces;

namespace Application.UseCases
{
	public class AddEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AddEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<int> ExecuteAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var eventEntity = _mapper.Map<Event>(eventDto);
			await _unitOfWork.EventRepository.AddEventAsync(eventEntity);
			await _unitOfWork.CommitAsync();
			return eventEntity.Id;
		}
	}
}
