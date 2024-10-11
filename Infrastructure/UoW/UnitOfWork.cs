using Infrastructure.Repositories;

namespace Infrastructure.UoW
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private EventRepository _eventRepository;
		private ParticipantRepository _participantRepository;
		private EventRegistrationRepository _eventRegistrationRepository;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_context);
		public IParticipantRepository ParticipantRepository => _participantRepository ??= new ParticipantRepository(_context);
		public IEventRegistrationRepository EventRegistrationRepository => _eventRegistrationRepository ??= new EventRegistrationRepository(_context);

		public async Task<int> CommitAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
