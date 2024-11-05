using AutoMapper;
using Application.DTOs;
using Application.Interfaces;


namespace Application.UseCases
{
	public class UpdateEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IsTitleUniqueUseCase isTitleUniqueUseCase;

		public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IsTitleUniqueUseCase isTitleUniqueUseCase)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			this.isTitleUniqueUseCase = isTitleUniqueUseCase;
		}

		public async Task ExecuteAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var titleIsUnique = await isTitleUniqueUseCase.ExecuteAsync(eventDto.Title);
			if (!titleIsUnique)
			{
				throw new InvalidOperationException("Title is already in use.");
			}

			var existingEvent = await _unitOfWork.EventRepository.GetEventByIdAsync(eventDto.Id);

			if (existingEvent == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			var eventItem = _mapper.Map(eventDto, existingEvent);
			await _unitOfWork.EventRepository.UpdateEventAsync(eventItem);
			await _unitOfWork.CommitAsync();
		}
	}
}
