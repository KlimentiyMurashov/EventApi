using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;
using Application.Validators;
using FluentValidation;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
	private readonly IParticipantService _participantService;
	private readonly IValidator<ParticipantDto> _validator;

	public ParticipantController(IParticipantService participantService, IValidator<ParticipantDto> validator)
	{
		_participantService = participantService;
		_validator = validator;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetAllParticipants()
	{
		var participants = await _participantService.GetAllParticipantsAsync();
		return Ok(participants);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ParticipantDto>> GetParticipantById(int id)
	{
		var participant = await _participantService.GetParticipantByIdAsync(id);
		if (participant == null) return NotFound();
		return Ok(participant);
	}

	[HttpPost]
	public async Task<ActionResult<int>> AddParticipant([FromBody] ParticipantDto participantDto)
	{
		var validationResult = await _validator.ValidateAsync(participantDto);
		if (!validationResult.IsValid)
		{
			return BadRequest(validationResult.Errors);
		}

		var participantId = await _participantService.AddParticipantAsync(participantDto);
		return CreatedAtAction(nameof(GetParticipantById), new { id = participantId }, participantId);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantDto participantDto)
	{
		if (id != participantDto.Id) return BadRequest();
		var validationResult = await _validator.ValidateAsync(participantDto);

		if (!validationResult.IsValid)
		{
			return BadRequest(validationResult.Errors);
		}

		await _participantService.UpdateParticipantAsync(participantDto);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteParticipant(int id)
	{
		await _participantService.DeleteParticipantAsync(id);
		return NoContent();
	}
}
