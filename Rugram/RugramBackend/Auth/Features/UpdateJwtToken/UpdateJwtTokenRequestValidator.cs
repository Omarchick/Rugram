using FluentValidation;

namespace Auth.Features.UpdateJwtToken;

public class UpdateJwtTokenRequestValidator : AbstractValidator<UpdateJwtTokenRequest>
{
	public UpdateJwtTokenRequestValidator()
	{
		RuleFor(request => request.RefreshToken)
			.NotEmpty();
		RuleFor(request => request.OldJwtToken)
			.NotEmpty();
	}
}