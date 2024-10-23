using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Application.Interfaces;

namespace Application.UseCase
{
	public class AddParticipantUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public AddParticipantUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<int> ExecuteAsync(ParticipantDto participantDto)
		{
			if (participantDto == null)
				throw new ArgumentNullException(nameof(participantDto));

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.AddParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync();
			return participantEntity.Id;
		}
	}

}
