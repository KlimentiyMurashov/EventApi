using Application.UseCase;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EventRegistrationController : ControllerBase
{
	private readonly AddEventRegistrationUseCase _addEventRegistrationUseCase;
	private readonly RemoveEventRegistrationUseCase _removeEventRegistrationUseCase;

	public EventRegistrationController(AddEventRegistrationUseCase addEventRegistrationUseCase, RemoveEventRegistrationUseCase removeEventRegistrationUseCase)
	{
		_addEventRegistrationUseCase = addEventRegistrationUseCase;
		_removeEventRegistrationUseCase = removeEventRegistrationUseCase;
	}

	[HttpPost("register")]
	public async Task<IActionResult> AddRegistration(int participantId, int eventId)
	{
		await _addEventRegistrationUseCase.ExecuteAsync(participantId, eventId);
		return Ok("Registration added successfully.");
	}

	[HttpDelete("unregister")]
	public async Task<IActionResult> RemoveRegistration(int participantId, int eventId)
	{
		await _removeEventRegistrationUseCase.ExecuteAsync(participantId, eventId);
		return Ok("Registration removed successfully.");
	}
}
