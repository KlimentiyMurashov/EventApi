using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
	private readonly IEventService _eventService;


	public EventController(IEventService eventService)
	{
		_eventService = eventService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllEvents()
	{
		var events = await _eventService.GetAllEventsAsync();
		return Ok(events);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetEventById(int id)
	{
		var eventItem = await _eventService.GetEventByIdAsync(id);
		if (eventItem == null) return NotFound();
		return Ok(eventItem);
	}

	[HttpPost]
	public async Task<IActionResult> AddEvent([FromBody] EventDto eventDto)
	{
		var eventId = await _eventService.AddEventAsync(eventDto);
		return CreatedAtAction(nameof(GetEventById), new { id = eventId }, eventDto);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto eventDto)
	{
		if (id != eventDto.Id)
		{
			return BadRequest("Id mismatch");
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
}


