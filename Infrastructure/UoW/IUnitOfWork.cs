using Infrastructure.Repositories;


namespace Infrastructure.UoW
{
	public interface IUnitOfWork : IDisposable
	{
		IEventRepository EventRepository { get; }
		IParticipantRepository ParticipantRepository { get; }
		IEventRegistrationRepository EventRegistrationRepository { get; } 
		Task<int> CommitAsync();
	}
}
