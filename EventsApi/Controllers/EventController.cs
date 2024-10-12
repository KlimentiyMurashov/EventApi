using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
	private readonly IEventService _eventService;
	private readonly IValidator<EventDto> _validator;

	public EventController(IEventService eventService, IValidator<EventDto> validator)
	{
		_eventService = eventService;
		_validator = validator;

	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
	{
		var events = await _eventService.GetAllEventsAsync();
		return Ok(events);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<EventDto>> GetEventById(int id)
	{
		var eventItem = await _eventService.GetEventByIdAsync(id);
		if (eventItem == null) return NotFound();
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

		var eventId = await _eventService.AddEventAsync(eventDto);
		return CreatedAtAction(nameof(GetEventById), new { id = eventId }, eventId);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto eventDto)
	{
		if (id != eventDto.Id) return BadRequest();

		var validationResult = await _validator.ValidateAsync(eventDto);
		if (!validationResult.IsValid)
		{
			return BadRequest(validationResult.Errors);
		}

		await _eventService.UpdateEventAsync(eventDto);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEvent(int id)
	{
		await _eventService.DeleteEventAsync(id);
		return NoContent();
	}

	[HttpGet("filter")]
	public async Task<IActionResult> GetEventsByCriteria([FromQuery] DateTime? date = null, [FromQuery] string? location = null, [FromQuery] string? category = null)
	{
		var events = await _eventService.GetEventsByCriteriesAsync(date, location, category);

		if (!events.Any())
		{
			return NotFound("No events found with the specified criteria.");
		}

		return Ok(events);
	}

	[HttpPut("{eventId}/add-image")]
	public async Task<IActionResult> AddImageUrl(int eventId, [FromBody] string imageUrl)
	{
		await _eventService.AddImageUrlToEventAsync(eventId, imageUrl);
		return Ok("Image added successfully.");
	}
}
