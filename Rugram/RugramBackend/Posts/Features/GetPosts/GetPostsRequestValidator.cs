using FluentValidation;

namespace Posts.Features.GetPosts;

public class GetPostsRequestValidator : AbstractValidator<GetPostsRequest>
{
	public GetPostsRequestValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.PageSize)
			.Must(x => x is > -1 and <= 1000);
		RuleFor(x => x.PageNumber)
			.Must(x => x is > -1 and < 100000);
		RuleFor(x => x)
			.Must(x => x.PageNumber == 0 || int.MaxValue / x.PageNumber >= x.PageSize);
	}
}