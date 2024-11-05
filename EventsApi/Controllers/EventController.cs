using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Application.UseCases;
using Application.UseCase;
using Application.Requests;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
	private readonly GetAllEventsUseCase _getAllEventsUseCase;
	private readonly GetEventByIdUseCase _getEventByIdUseCase;
	private readonly AddEventUseCase _addEventUseCase;
	private readonly UpdateEventUseCase _updateEventUseCase;
	private readonly DeleteEventUseCase _deleteEventUseCase;
	private readonly GetEventsByCriteriesUseCase _getEventsByCriteriesUseCase;
	private readonly AddImageUrlToEventUseCase _addImageUrlUseCase;
	private readonly IValidator<EventDto> _validator;

	public EventController(
		GetAllEventsUseCase getAllEventsUseCase,
		GetEventByIdUseCase getEventByIdUseCase,
		AddEventUseCase addEventUseCase,
		UpdateEventUseCase updateEventUseCase,
	    DeleteEventUseCase deleteEventUseCase,
		GetEventsByCriteriesUseCase getEventsByCriteriaUseCase,
		AddImageUrlToEventUseCase addImageUrlUseCase,
		IValidator<EventDto> validator)
	{
		_getAllEventsUseCase = getAllEventsUseCase;
		_getEventByIdUseCase = getEventByIdUseCase;
		_addEventUseCase = addEventUseCase;
		_updateEventUseCase = updateEventUseCase;
		_deleteEventUseCase = deleteEventUseCase;
		_getEventsByCriteriesUseCase = getEventsByCriteriaUseCase;
		_addImageUrlUseCase = addImageUrlUseCase;
		_validator = validator;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
	{
		var events = await _getAllEventsUseCase.ExecuteAsync();
		return Ok(events);
	}

	[Authorize(Roles = "Admin")]
	[HttpGet("{id}")]
	public async Task<ActionResult<EventDto>> GetEventById(int id)
	{
		var eventItem = await _getEventByIdUseCase.ExecuteAsync(id);
		return Ok(eventItem);
	}

	[HttpPost]
	public async Task<ActionResult<int>> AddEvent([FromBody] EventDto eventDto)
	{
		var validationResult = await _validator.ValidateAsync(eventDto);
		if (!validationResult.IsValid)
		{
			return BadRequest(validationResult.Errors);
		}

		var eventId = await _addEventUseCase.ExecuteAsync(eventDto);
		return CreatedAtAction(nameof(GetEventById), new { id = eventId }, eventId);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateEvent([FromBody] EventDto eventDto)
	{
		var validationResult = await _validator.ValidateAsync(eventDto);
		if (!validationResult.IsValid)
		{
			return BadRequest(validationResult.Errors);
		}

		await _updateEventUseCase.ExecuteAsync(eventDto);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEvent(int id)
	{
		await _deleteEventUseCase.ExecuteAsync(id);
		return NoContent();
	}

	[HttpGet("filter")]
	public async Task<IActionResult> GetEventsByCriteria([FromQuery] GetEventsByCriteriesRequest request)
	{
		var events = await _getEventsByCriteriesUseCase.ExecuteAsync(request);
		return Ok(events);
	}

	[HttpPut("{eventId}/add-image")]
	public async Task<IActionResult> AddImageUrl(AddImageUrlToEventRequest request)
	{
		await _addImageUrlUseCase.ExecuteAsync(request);
		return Ok("Image added successfully.");
	}
}
