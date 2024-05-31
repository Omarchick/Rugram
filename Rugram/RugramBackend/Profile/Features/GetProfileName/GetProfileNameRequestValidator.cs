using FluentValidation;

namespace Profile.Features.GetProfileName;

public class GetProfileNameRequestValidator : AbstractValidator<GetProfileNameRequest>
{
	public GetProfileNameRequestValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
	}
}