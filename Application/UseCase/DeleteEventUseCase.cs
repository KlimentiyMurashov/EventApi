using Application.Interfaces;
using Application.Requests;

namespace Application.UseCases
{
	public class DeleteEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteEventUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(DeleteEventRequest request)
		{
			if(request.EventId <= 0) 
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var eventItem = await _unitOfWork.EventRepository.GetEventByIdAsync(request.EventId);
			if (eventItem == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			await _unitOfWork.EventRepository.DeleteEventByIdAsync(request.EventId);
			await _unitOfWork.CommitAsync();
		}
	}
}
