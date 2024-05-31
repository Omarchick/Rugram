using FluentValidation;

namespace Profile.Features.Subscribe;

public class SubscribeRequestValidator : AbstractValidator<SubscribeRequest>
{
	public SubscribeRequestValidator()
	{
		RuleFor(x => x.IdOfProfileSubscribedTo)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.SubscriberId)
			.NotEqual(Guid.Empty);
	}
}