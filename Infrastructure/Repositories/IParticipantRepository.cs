using Domain.Entities;

namespace Infrastructure.Repositories
{
	public interface IParticipantRepository
	{
		Task<IEnumerable<Participant>> GetAllParticipantsAsync();
		Task<Participant> GetParticipantByIdAsync(int id);
		Task AddParticipantAsync(Participant participant);
		Task UpdateParticipantAsync(Participant updatedParticipant);
		Task DeleteParticipantByIdAsync(int id);
	}

}
