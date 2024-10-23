using Application.Interfaces;

namespace Application.UseCases
{
	public class DeleteEventUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteEventUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(int id)
		{
			if(id <= 0) 
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var eventItem = await _unitOfWork.EventRepository.GetEventByIdAsync(id);
			if (eventItem == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			await _unitOfWork.EventRepository.DeleteEventByIdAsync(id);
			await _unitOfWork.CommitAsync();
		}
	}
}
