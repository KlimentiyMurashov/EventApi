using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;

namespace BusinessLogicLayer.Services
{
	public class ParticipantService : IParticipantService
	{
		private readonly IParticipantRepository _participantRepository;
		private readonly IMapper _mapper;

		public ParticipantService(IParticipantRepository participantRepository, IMapper mapper)
		{
			_participantRepository = participantRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync()
		{
			var participants = await _participantRepository.GetAllParticipantsAsync();
			return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
		}

		public async Task<ParticipantDto> GetParticipantByIdAsync(int id)
		{
			var participant = await _participantRepository.GetParticipantByIdAsync(id);
			return _mapper.Map<ParticipantDto>(participant);
		}

		public async Task<int> AddParticipantAsync(ParticipantDto participantDto)
		{
			var participantEntity = _mapper.Map<Participant>(participantDto);
			await _participantRepository.AddParticipantAsync(participantEntity);
			return participantEntity.Id;
		}

		public async Task<int> UpdateParticipantAsync(ParticipantDto participantDto)
		{
			var participant = _mapper.Map<Participant>(participantDto);
			await _participantRepository.UpdateParticipantAsync(participant);
			return participant.Id;
		}

		public async Task DeleteParticipantAsync(int id)
		{
			await _participantRepository.DeleteParticipantByIdAsync(id);
		}

		public async Task RegisterParticipantForEventAsync(int participantId, int eventId)
		{
			await _participantRepository.RegisterParticipantForEventAsync(participantId, eventId);
		}

		public async Task CancelParticipationAsync(int participantId, int eventId)
		{
			await _participantRepository.CancelParticipationAsync(participantId, eventId);
		}
	}
}

