using Application.DTOs;
using Application.UseCase;
using FluentValidation;

namespace Application.Validators
{
	public class ParticipantDtoValidator : AbstractValidator<ParticipantDto>
	{
		public ParticipantDtoValidator(IsEmailUniqueUseCase _isEmailUniqueUseCase)
		{
			RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(20).WithMessage("First Name cannot exceed 20 characters.")
            .Matches(@"^[a-zA-Z]+$").WithMessage("First Name cannot contain numbers or special characters.");

			RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(20).WithMessage("Last Name cannot exceed 20 characters.")
            .Matches(@"^[a-zA-Z]+$").WithMessage("Last Name cannot contain numbers or special characters.");

			RuleFor(x => x.DateOfBirth)
			.Must(dob => dob < DateTime.Now).WithMessage("Date of Birth must be in the past.")
			.Must(dob => dob > DateTime.Now.AddYears(-99)).WithMessage("Participant must be less than 99 years old.");

			RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("A valid email is required.")
			.Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email format is not valid.")
			.MustAsync(async (email, cancellation) =>
			{
				var emailIsUnique = await _isEmailUniqueUseCase.ExecuteAsync(email);
				return emailIsUnique;
			}).WithMessage("Email is already in use.");

		}
	}

}
