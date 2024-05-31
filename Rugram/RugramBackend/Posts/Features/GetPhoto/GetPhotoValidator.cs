using FluentValidation;

namespace Posts.Features.GetPhoto;

public class GetPhotoValidator : AbstractValidator<GetPhotoRequest>
{
	public GetPhotoValidator()
	{
		RuleFor(x => x.PhotoId)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
	}
}