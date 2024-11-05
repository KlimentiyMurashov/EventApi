using Application.Interfaces;
using Application.Requests;

namespace Application.UseCases
{
	public class AddImageUrlToEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public AddImageUrlToEventUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(AddImageUrlToEventRequest request)
		{
			if (request.EventId <= 0)
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var eventEntity = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId);

			if (eventEntity == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			eventEntity.ImageUrl = request.ImageUrl;
			await _unitOfWork.CommitAsync();
		}
	}
}
