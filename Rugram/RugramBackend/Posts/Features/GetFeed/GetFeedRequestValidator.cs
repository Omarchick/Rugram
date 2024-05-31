using FluentValidation;

namespace Posts.Features.GetFeed;

public class GetFeedRequestValidator : AbstractValidator<GetFeedRequest>
{
	public GetFeedRequestValidator()
	{
		RuleFor(x => x.PageSize)
			.Must(x => x is > -1 and <= 1000);
		RuleFor(x => x.PageNumber)
			.Must(x => x is > -1 and < 100000);
		RuleFor(x => x)
			.Must(x => x.PageNumber == 0 || int.MaxValue / x.PageNumber >= x.PageSize);
		RuleFor(x => x.SubscriptionIds)
			.Must(x => x.All(id => id != Guid.Empty));
	}
}