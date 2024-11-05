using Application.Interfaces;
using Application.Requests;



namespace Application.UseCase
{
	public class RemoveEventRegistrationUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public RemoveEventRegistrationUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(RemoveEventRegistrationRequest request)
		{
			if (request.ParticipantId <= 0 || request.EventId <= 0)
			{
				throw new ArgumentNullException("Participant ID and Event ID must be greater than zero.");
			}

			var registration = await _unitOfWork.EventRegistrationRepository.GetRegistrationAsync(request.ParticipantId, request.EventId);
			if (registration == null)
			{
				throw new InvalidOperationException("Registration not found.");
			}

			await _unitOfWork.EventRegistrationRepository.RemoveRegistrationAsync(registration);
			await _unitOfWork.CommitAsync();
		}
	}
}
