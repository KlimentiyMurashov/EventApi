using Application.DTOs;
using Application.Services;
using FluentValidation;

namespace Application.Validators
{
	public class EventDtoValidator : AbstractValidator<EventDto>
	{
		public EventDtoValidator(IEventService eventService)
		{
			RuleFor(x => x.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(20).WithMessage("Title cannot exceed 20 characters.")
			.MustAsync(async (title, cancellation) =>
			{
				var titleIsUnique = await eventService.IsTitleUniqueAsync(title);
				return titleIsUnique;
			}).WithMessage("Title is already in use.");

			RuleFor(x => x.Description)
				.MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
			
			RuleFor(x => x.Category)
				.MaximumLength(20).WithMessage("Description cannot exceed 20 characters.")
				.Matches(@"^[a-zA-Z]+$").WithMessage("Location cannot contain numbers or special characters.");

			RuleFor(x => x.DateTime)
				.GreaterThanOrEqualTo(DateTime.Now).WithMessage("Event date must be in the future.");

			RuleFor(x => x.Location)
				.NotEmpty().WithMessage("Location is required.")
			    .Matches(@"^[a-zA-Z]+$").WithMessage("Location cannot contain numbers or special characters.");


			RuleFor(x => x.MaxParticipants)
				.GreaterThan(0).WithMessage("Max participants should be more than 0.");
		}
	}
}
