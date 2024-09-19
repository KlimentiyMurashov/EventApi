using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.DTOs;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
	private readonly IParticipantService _participantService;

	public ParticipantController(IParticipantService participantService)
	{
		_participantService = participantService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllParticipants()
	{
		var participants = await _participantService.GetAllParticipantsAsync();
		return Ok(participants);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetParticipantById(int id)
	{
		var participant = await _participantService.GetParticipantByIdAsync(id);
		if (participant == null) return NotFound();
		return Ok(participant);
	}

	[HttpPost]
	public async Task<IActionResult> AddParticipant([FromBody] ParticipantDto participantDto)
	{
		var participantId = await _participantService.AddParticipantAsync(participantDto);
		return CreatedAtAction(nameof(GetParticipantById), new { id = participantId }, participantDto);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantDto participantDto)
	{
		if (id != participantDto.Id) return BadRequest();
		await _participantService.UpdateParticipantAsync(participantDto);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteParticipant(int id)
	{
		await _participantService.DeleteParticipantAsync(id);
		return NoContent();
	}

	[HttpPost("{participantId}/register/{eventId}")]
	public async Task<IActionResult> RegisterParticipant(int participantId, int eventId)
	{
		await _participantService.RegisterParticipantForEventAsync(participantId, eventId);
		return Ok("Participant registered successfully");
	}

	[HttpDelete("{participantId}/unregister/{eventId}")]
	public async Task<IActionResult> UnregisterParticipant(int participantId, int eventId)
	{
		await _participantService.CancelParticipationAsync(participantId, eventId);
		return Ok("Participant unregistered successfully");
	}
}




