using FluentValidation;

namespace Profile.Features.Unsubscribe;

public class UnsubscribeRequestValidator : AbstractValidator<UnsubscribeRequest>
{
	public UnsubscribeRequestValidator()
	{
		RuleFor(x => x.IdOfProfileUnsubscribedTo)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.SubscriberId)
			.NotEqual(Guid.Empty);
	}
}