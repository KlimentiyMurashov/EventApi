using Domain.Entities;
using Application.Interfaces;
using Application.Requests;

namespace Application.UseCase
{
	public class AddEventRegistrationUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddEventRegistrationUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(AddEventRegistrationRequest request)
		{
			if (request.ParticipantId <= 0 || request.EventId <= 0)
			{
				throw new ArgumentNullException("Participant ID and Event ID must be greater than zero.");
			}

			var participant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(request.ParticipantId);
			var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId);

			if (participant == null || eventEntity == null)
			{
				throw new InvalidOperationException("Participant or event not found.");
			}

			var registrationEntity = new EventRegistration
			{
				ParticipantId = request.ParticipantId,
				EventId = request.EventId,
				RegistrationDate = DateTime.UtcNow
			};

			await _unitOfWork.EventRegistrationRepository.AddRegistrationAsync(registrationEntity);
			await _unitOfWork.CommitAsync();
		}
	}
}
