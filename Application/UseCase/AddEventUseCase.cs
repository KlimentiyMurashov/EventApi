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
		private readonly IsTitleUniqueUseCase _isTitleUniqueUseCase;

		public AddEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IsTitleUniqueUseCase isTitleUniqueUseCase)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_isTitleUniqueUseCase = isTitleUniqueUseCase;
		}

		public async Task<int> ExecuteAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var titleIsUnique = await _isTitleUniqueUseCase.ExecuteAsync(eventDto.Title);
			if (!titleIsUnique)
			{
				throw new InvalidOperationException("Title is already in use."); 
			}

			var eventEntity = _mapper.Map<Event>(eventDto);
			await _unitOfWork.EventRepository.AddEventAsync(eventEntity);
			await _unitOfWork.CommitAsync();
			return eventEntity.Id;
		}
	}
}
