using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;

namespace Application.UseCase
{
	public class UpdateParticipantUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UpdateParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
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

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.UpdateParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync();
		}
	}

}
