using Application.DTOs;
using AutoMapper;
using Application.Interfaces;

namespace Application.UseCase
{
	public class GetAllParticipantsUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllParticipantsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ParticipantDto>> ExecuteAsync()
		{
			var participants = await _unitOfWork.ParticipantRepository.GetAllParticipantsAsync();

			if (participants == null || !participants.Any())
			{
				throw new InvalidOperationException("The participant list is empty or not found.");
			}

			return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
		}
	}

}
