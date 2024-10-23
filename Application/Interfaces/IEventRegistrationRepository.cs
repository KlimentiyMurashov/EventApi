using Domain.Entities;

namespace Application.Interfaces
{
	public interface IEventRegistrationRepository
	{
		Task<EventRegistration?> GetRegistrationAsync(int participantId, int eventId);
		Task AddRegistrationAsync(EventRegistration registration);
		Task RemoveRegistrationAsync(EventRegistration registration);
	}

}
