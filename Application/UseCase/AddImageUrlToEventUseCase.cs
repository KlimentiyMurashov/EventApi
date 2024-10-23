using Application.Interfaces;

namespace Application.UseCases
{
	public class AddImageUrlToEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddImageUrlToEventUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(int eventId, string imageUrl)
		{
			if (eventId <= 0)
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(eventId);

			if (eventEntity == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			eventEntity.ImageUrl = imageUrl;
			await _unitOfWork.CommitAsync();
		}
	}
}
