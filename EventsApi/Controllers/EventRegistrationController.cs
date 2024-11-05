using Application.Requests;
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
	public async Task<IActionResult> AddRegistration(AddEventRegistrationRequest request)
	{
		await _addEventRegistrationUseCase.ExecuteAsync(request);
		return Ok("Registration added successfully.");
	}

	[HttpDelete("unregister")]
	public async Task<IActionResult> RemoveRegistration(RemoveEventRegistrationRequest request)
	{
		await _removeEventRegistrationUseCase.ExecuteAsync(request);
		return Ok("Registration removed successfully.");
	}
}
