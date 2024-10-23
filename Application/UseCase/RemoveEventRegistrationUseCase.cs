using Application.Interfaces;



namespace Application.UseCase
{
	public class RemoveEventRegistrationUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public RemoveEventRegistrationUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(int participantId, int eventId)
		{
			if (participantId <= 0 || eventId <= 0)
			{
				throw new ArgumentNullException("Participant ID and Event ID must be greater than zero.");
			}

			var registration = await _unitOfWork.EventRegistrationRepository.GetRegistrationAsync(participantId, eventId);
			if (registration == null)
			{
				throw new InvalidOperationException("Registration not found.");
			}

			await _unitOfWork.EventRegistrationRepository.RemoveRegistrationAsync(registration);
			await _unitOfWork.CommitAsync();
		}
	}
}
