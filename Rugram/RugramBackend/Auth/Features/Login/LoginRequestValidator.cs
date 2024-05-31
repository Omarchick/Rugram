using FluentValidation;

namespace Auth.Features.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
	public LoginRequestValidator()
	{
		RuleFor(request => request.Email)
			.EmailAddress();
		RuleFor(request => request.Password)
			.MinimumLength(5)
			.MaximumLength(25);
	}
}