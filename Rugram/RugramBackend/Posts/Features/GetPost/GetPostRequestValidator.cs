using FluentValidation;

namespace Posts.Features.GetPost;

public class GetPostRequestValidator : AbstractValidator<GetPostRequest>
{
	public GetPostRequestValidator()
	{
		RuleFor(x => x.PostId)
			.NotEqual(Guid.Empty);
	}
}