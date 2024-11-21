using AutoMapper;
using Application.DTOs;
using Application.Interfaces;
using FluentValidation;


namespace Application.UseCases
{
	public class UpdateEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IValidator<EventDto> _validator;

		public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventDto> validator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task ExecuteAsync(EventDto eventDto)
		{
			if (eventDto == null)
				throw new ArgumentNullException(nameof(eventDto));

			var validationResult = await _validator.ValidateAsync(eventDto);
			if (!validationResult.IsValid)
				throw new InvalidOperationException("Validation failed: " + validationResult.Errors);

			var titleIsUnique = await _unitOfWork.EventRepository.IsTitleUniqueAsync(eventDto.Title);
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
