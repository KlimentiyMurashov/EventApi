namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository EventRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
        IEventRegistrationRepository EventRegistrationRepository { get; }
        Task<int> CommitAsync();
    }
}
