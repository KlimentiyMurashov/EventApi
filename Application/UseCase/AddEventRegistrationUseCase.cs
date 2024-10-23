using Domain.Entities;
using Application.Interfaces;

namespace Application.UseCase
{
	public class AddEventRegistrationUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddEventRegistrationUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(int participantId, int eventId)
		{
			if (participantId <= 0 || eventId <= 0)
			{
				throw new ArgumentNullException("Participant ID and Event ID must be greater than zero.");
			}

			var participant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(participantId);
			var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);

			if (participant == null || eventEntity == null)
			{
				throw new InvalidOperationException("Participant or event not found.");
			}

			var registrationEntity = new EventRegistration
			{
				ParticipantId = participantId,
				EventId = eventId,
				RegistrationDate = DateTime.UtcNow
			};

			await _unitOfWork.EventRegistrationRepository.AddRegistrationAsync(registrationEntity);
			await _unitOfWork.CommitAsync();
		}
	}
}
