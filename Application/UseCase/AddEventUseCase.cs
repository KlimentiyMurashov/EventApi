using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Application.Interfaces;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases
{
	public class AddEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IValidator<EventDto> _validator;

		public AddEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<EventDto> validator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task<int> ExecuteAsync(EventDto eventDto)
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

			var eventEntity = _mapper.Map<Event>(eventDto);
			await _unitOfWork.EventRepository.AddEventAsync(eventEntity);
			await _unitOfWork.CommitAsync();
			return eventEntity.Id;
		}
	}
}
