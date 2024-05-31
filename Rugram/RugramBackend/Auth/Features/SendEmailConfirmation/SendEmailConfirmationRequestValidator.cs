using FluentValidation;

namespace Auth.Features.SendEmailConfirmation;

public class SendEmailConfirmationRequestValidator : AbstractValidator<SendEmailConfirmationRequest>
{
	public SendEmailConfirmationRequestValidator()
	{
		RuleFor(request => request.Email)
			.EmailAddress();
	}
}