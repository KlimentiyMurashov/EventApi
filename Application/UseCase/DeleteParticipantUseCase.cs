using Application.Interfaces;

namespace Application.UseCase
{
	public class DeleteParticipantUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public DeleteParticipantUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentNullException("Participant ID must be greater than zero.");

			var prticipant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);
			if (prticipant == null)
			{
				throw new InvalidOperationException("Participant not found.");
			}

			await _unitOfWork.ParticipantRepository.DeleteParticipantByIdAsync(id);
			await _unitOfWork.CommitAsync();
		}
	}

}
