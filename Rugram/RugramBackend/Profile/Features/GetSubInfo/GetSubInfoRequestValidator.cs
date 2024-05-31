using FluentValidation;

namespace Profile.Features.GetSubInfo;

public class GetSubInfoRequestValidator : AbstractValidator<GetSubInfoRequest>
{
	public GetSubInfoRequestValidator()
	{
		RuleFor(x => x)
			.Must(x => x.ThisProfileId != x.OtherProfileId);
	}
}