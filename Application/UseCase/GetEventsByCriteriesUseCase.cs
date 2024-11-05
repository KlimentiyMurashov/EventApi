using Application.DTOs;
using AutoMapper;
using Application.Interfaces;
using Application.Requests;


namespace Application.UseCase
{
	public class GetEventsByCriteriesUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetEventsByCriteriesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<EventDto>> ExecuteAsync(GetEventsByCriteriesRequest request)
		{

			var events = await _unitOfWork.EventRepository.GetEventsByCriteriesAsync(request.Date, request.Location, request.Category);

			if (events == null || !events.Any())
			{
				throw new InvalidOperationException("No events were found matching the specified criteries.");
			}

			return _mapper.Map<IEnumerable<EventDto>>(events);
		}
	}
}

