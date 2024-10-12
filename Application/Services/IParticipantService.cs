using Application.DTOs;


namespace Application.Services
{
	public interface IParticipantService
	{
		Task<IEnumerable<ParticipantDto>> GetAllParticipantsAsync();
		Task<ParticipantDto> GetParticipantByIdAsync(int id);
		Task<int> AddParticipantAsync(ParticipantDto participantDto);
		Task UpdateParticipantAsync(ParticipantDto participantDto);
		Task DeleteParticipantAsync(int id);
		Task<bool> IsEmailUniqueAsync(string email);
	}
}
