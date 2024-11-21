using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using FluentValidation;
using Application.UseCase;

[ApiController]
[Route("api/[controller]")]
public class ParticipantController : ControllerBase
{
	private readonly GetAllParticipantsUseCase _getAllParticipantsUseCase;
	private readonly GetParticipantByIdUseCase _getParticipantByIdUseCase;
	private readonly AddParticipantUseCase _addParticipantUseCase;
	private readonly UpdateParticipantUseCase _updateParticipantUseCase;
	private readonly DeleteParticipantUseCase _deleteParticipantUseCase;
	private readonly IValidator<ParticipantDto> _validator;

	public ParticipantController(
		GetAllParticipantsUseCase getAllParticipantsUseCase,
		GetParticipantByIdUseCase getParticipantByIdUseCase,
		AddParticipantUseCase addParticipantUseCase,
		UpdateParticipantUseCase updateParticipantUseCase,
		DeleteParticipantUseCase deleteParticipantUseCase,
		IValidator<ParticipantDto> validator)
	{
		_getAllParticipantsUseCase = getAllParticipantsUseCase;
		_getParticipantByIdUseCase = getParticipantByIdUseCase;
		_addParticipantUseCase = addParticipantUseCase;
		_updateParticipantUseCase = updateParticipantUseCase;
		_deleteParticipantUseCase = deleteParticipantUseCase;
		_validator = validator;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetAllParticipants()
	{
		var participants = await _getAllParticipantsUseCase.ExecuteAsync();
		return Ok(participants);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<ParticipantDto>> GetParticipantById(int id)
	{
		var participant = await _getParticipantByIdUseCase.ExecuteAsync(id);
		return Ok(participant);
	}

	[HttpPost]
	public async Task<ActionResult<int>> AddParticipant([FromBody] ParticipantDto participantDto)
	{
		var participantId = await _addParticipantUseCase.ExecuteAsync(participantDto);
		return CreatedAtAction(nameof(GetParticipantById), new { id = participantId }, participantId);
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateParticipant([FromBody] ParticipantDto participantDto)
	{
		await _updateParticipantUseCase.ExecuteAsync(participantDto);
		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteParticipant(int id)
	{
		await _deleteParticipantUseCase.ExecuteAsync(id);
		return NoContent();
	}
}
