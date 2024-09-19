using BusinessLogicLayer.DTOs;


namespace BusinessLogicLayer.Services
{
	public interface IParticipantService
	{
		Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync();
		Task<ParticipantDto> GetParticipantByIdAsync(int id);
		Task<int> AddParticipantAsync(ParticipantDto participantDto);
		Task<int> UpdateParticipantAsync(ParticipantDto participantDto);
		Task DeleteParticipantAsync(int id);
		Task RegisterParticipantForEventAsync(int participantId, int eventId);
		Task CancelParticipationAsync(int participantId, int eventId);
	}
}
