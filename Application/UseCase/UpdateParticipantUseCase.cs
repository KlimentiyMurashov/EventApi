using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;
using FluentValidation;

namespace Application.UseCase
{
	public class UpdateParticipantUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IValidator<ParticipantDto> _validator;

		public UpdateParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ParticipantDto> validator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task ExecuteAsync(ParticipantDto participantDto)
		{
			if (participantDto == null)
				throw new ArgumentNullException(nameof(participantDto));

			var existingParticipant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(participantDto.Id);

			if (existingParticipant == null)
			{
				throw new InvalidOperationException("Participant not found.");
			}

			var validationResult = await _validator.ValidateAsync(participantDto);
			if (!validationResult.IsValid)
				throw new InvalidOperationException("Validation failed: " + validationResult.Errors);

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.UpdateParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync();
		}
	}

}
