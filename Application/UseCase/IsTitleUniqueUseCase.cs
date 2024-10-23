using Application.Interfaces;

namespace Application.UseCases
{
	public class IsTitleUniqueUseCase
	{
		private readonly IUnitOfWork _unitOfWork;

		public IsTitleUniqueUseCase(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> ExecuteAsync(string title)
		{
			return await _unitOfWork.EventRepository.IsTitleUniqueAsync(title);
		}
	}
}
