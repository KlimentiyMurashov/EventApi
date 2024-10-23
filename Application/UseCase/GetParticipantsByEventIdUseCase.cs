using AutoMapper;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases
{
	public class GetParticipantsByEventIdUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetParticipantsByEventIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<ParticipantDto>> ExecuteAsync(int eventId)
		{
			if (eventId <= 0)
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var participants = await _unitOfWork.EventRepository.GetParticipantsByEventIdAsync(eventId);

			if (participants == null)
			{
				throw new InvalidOperationException("Participant not found.");
			}

			return _mapper.Map<IEnumerable<ParticipantDto>>(participants);
		}
	}
}
