using Application.Interfaces;

namespace Application.UseCase
{
	public class IsEmailUniqueUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public IsEmailUniqueUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> ExecuteAsync(string email)
		{
			return !await _unitOfWork.ParticipantRepository.EmailExistsAsync(email);
		}
	}

}
