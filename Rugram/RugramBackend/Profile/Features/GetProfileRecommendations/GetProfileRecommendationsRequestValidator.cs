using FluentValidation;

namespace Profile.Features.GetProfileRecommendations;

public class GetProfileRecommendationsRequestValidator : AbstractValidator<GetProfileRecommendationsRequest>
{
	public GetProfileRecommendationsRequestValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.SearchString)
			.Must(x => x.Length < 25);
		RuleFor(x => x.PageSize)
			.Must(x => x is > -1 and <= 1000);
		RuleFor(x => x.PageNumber)
			.Must(x => x is > -1 and < 100000);
		RuleFor(x => x)
			.Must(x => x.PageNumber == 0 || int.MaxValue / x.PageNumber >= x.PageSize);
	}
}