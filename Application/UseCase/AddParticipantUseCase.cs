using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;
using FluentValidation;

namespace Application.UseCase
{
	public class AddParticipantUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IValidator<ParticipantDto> _validator;

		public AddParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ParticipantDto> validator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_validator = validator;
		}

		public async Task<int> ExecuteAsync(ParticipantDto participantDto)
		{
			if (participantDto == null)
				throw new ArgumentNullException(nameof(participantDto));

			var validationResult = await _validator.ValidateAsync(participantDto);
			if (!validationResult.IsValid)
				throw new InvalidOperationException("Validation failed: " + validationResult.Errors);

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.AddParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync();
			return participantEntity.Id;
		}
	}

}
