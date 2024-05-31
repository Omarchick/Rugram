using FluentValidation;

namespace Auth.Features.RegisterUser;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
	public RegisterUserRequestValidator()
	{
		RuleFor(request => request.Email)
			.EmailAddress();
		RuleFor(request => request.Password)
			.MinimumLength(5)
			.MaximumLength(25);
	}
}