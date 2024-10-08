namespace Application.Services
{
	public interface IEventRegistrationService
	{
		Task AddRegistrationAsync(int participantId, int eventId);
		Task RemoveRegistrationAsync(int participantId, int eventId);
	}
}
