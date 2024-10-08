using Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventRegistrationController : ControllerBase
{
	private readonly IEventRegistrationService _eventRegistrationService;

	public EventRegistrationController(IEventRegistrationService eventRegistrationService)
	{
		_eventRegistrationService = eventRegistrationService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> AddRegistration(int participantId, int eventId)
	{
		await _eventRegistrationService.AddRegistrationAsync(participantId, eventId);
		return Ok("Registration added successfully.");
	}

	[HttpDelete("unregister")]
	public async Task<IActionResult> RemoveRegistration(int participantId, int eventId)
	{
		await _eventRegistrationService.RemoveRegistrationAsync(participantId, eventId);
		return Ok("Registration removed successfully.");
	}
}
