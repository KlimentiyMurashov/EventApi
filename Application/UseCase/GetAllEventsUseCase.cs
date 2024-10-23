using AutoMapper;
using Application.DTOs;
using Application.Interfaces;

namespace Application.UseCases
{
	public class GetAllEventsUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetAllEventsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDto>> ExecuteAsync()
		{
			var events = await _unitOfWork.EventRepository.GetAllEventsAsync();

			if (events == null || !events.Any())
			{
				throw new InvalidOperationException("The event list is empty or not found.");
			}

			return _mapper.Map<IEnumerable<EventDto>>(events);
		}
	}
}
