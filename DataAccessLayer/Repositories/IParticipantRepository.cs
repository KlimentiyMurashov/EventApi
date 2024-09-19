using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public interface IParticipantRepository
	{
		Task<IEnumerable<Participant>> GetAllParticipantsAsync();
		Task<Participant> GetParticipantByIdAsync(int id);
		Task AddParticipantAsync(Participant participant);
		Task UpdateParticipantAsync(Participant updatedParticipant);
		Task DeleteParticipantByIdAsync(int id);

		Task RegisterParticipantForEventAsync(int participantId, int eventId);
		Task CancelParticipationAsync(int participantId, int eventId);

		Task<IEnumerable<Event>> GetEventsByParticipantIdAsync(int participantId);
	}

}
