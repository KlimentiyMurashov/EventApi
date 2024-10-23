using Application.DTOs;
using AutoMapper;
using Application.Interfaces;

namespace Application.UseCase
{
	public class GetParticipantByIdUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetParticipantByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ParticipantDto> ExecuteAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentNullException("Participant ID must be greater than zero.");

			var participant = await _unitOfWork.ParticipantRepository.GetParticipantByIdAsync(id);

			if (participant == null)
			{
				throw new InvalidOperationException("Participant not found.");
			}

			return _mapper.Map<ParticipantDto>(participant);
		}
	}

}
