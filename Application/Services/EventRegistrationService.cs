using Domain.Entities;
using Infrastructure.UoW;


namespace Application.Services
{
	public class EventRegistrationService : IEventRegistrationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public EventRegistrationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task AddRegistrationAsync(int participantId, int eventId)
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

		public async Task RemoveRegistrationAsync(int participantId, int eventId)
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
