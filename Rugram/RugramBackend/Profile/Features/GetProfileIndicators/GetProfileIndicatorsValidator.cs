using FluentValidation;
using Profile.Features.GetProfileName;

namespace Profile.Features.GetProfileIndicators;

public class GetProfileIndicatorsValidator : AbstractValidator<GetProfileNameRequest>
{
	public GetProfileIndicatorsValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
	}
}