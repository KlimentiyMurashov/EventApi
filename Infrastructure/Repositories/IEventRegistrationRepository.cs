using Domain.Entities;

namespace Infrastructure.Repositories
{
	public interface IEventRegistrationRepository
	{
		Task<EventRegistration?> GetRegistrationAsync(int participantId, int eventId);
		Task AddRegistrationAsync(EventRegistration registration);
		Task RemoveRegistrationAsync(EventRegistration registration);
	}

}
