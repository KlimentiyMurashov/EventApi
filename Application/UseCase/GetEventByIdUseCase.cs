using Application.DTOs;
using AutoMapper;
using Application.Interfaces;

namespace Application.UseCases
{
	public class GetEventByIdUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<EventDto> ExecuteAsync(int id)
		{
			if (id <= 0)
				throw new ArgumentNullException("Event ID must be greater than zero.");

			var eventItem = await _unitOfWork.EventRepository.GetEventByIdAsync(id);

			if (eventItem == null)
			{
				throw new InvalidOperationException("Event not found.");
			}

			return _mapper.Map<EventDto>(eventItem);
		}
	}
}
