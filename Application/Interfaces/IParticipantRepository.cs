using Domain.Entities;

namespace Application.Interfaces
{
	public interface IParticipantRepository
	{
		Task<IEnumerable<Participant>> GetAllParticipantsAsync();
		Task<Participant> GetParticipantByIdAsync(int id);
		Task AddParticipantAsync(Participant participant);
		Task UpdateParticipantAsync(Participant updatedParticipant);
		Task DeleteParticipantByIdAsync(int id);
		Task<bool> EmailExistsAsync(string email);
	}

}
