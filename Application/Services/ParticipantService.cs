using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Infrastructure.UoW;

namespace Application.Services
{
	public class ParticipantService : IParticipantService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ParticipantService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync()
		{
			var participants = await _unitOfWork.ParticipantRepository.GetAllParticipantsAsync();
			return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
		}

		public async Task<ParticipantDto> GetParticipantByIdAsync(int id)
		{
			var participant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);
			return _mapper.Map<ParticipantDto>(participant);
		}

		public async Task<int> AddParticipantAsync(ParticipantDto participantDto)
		{
			if (participantDto == null)
				throw new ArgumentNullException(nameof(participantDto));

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.AddParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync(); 
			return participantEntity.Id;
		}

		public async Task UpdateParticipantAsync(ParticipantDto participantDto)
		{
			if (participantDto == null)
				throw new ArgumentNullException(nameof(participantDto));

			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _unitOfWork.ParticipantRepository.UpdateParticipantAsync(participantEntity);
			await _unitOfWork.CommitAsync(); 
		}

		public async Task DeleteParticipantAsync(int id)
		{
			await _unitOfWork.ParticipantRepository.DeleteParticipantByIdAsync(id);
			await _unitOfWork.CommitAsync(); 
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return !await _unitOfWork.ParticipantRepository.EmailExistsAsync(email);
		}
	}
}
