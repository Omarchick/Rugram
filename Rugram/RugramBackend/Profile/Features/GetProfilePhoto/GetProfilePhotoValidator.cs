using FluentValidation;

namespace Profile.Features.GetProfilePhoto;

public class GetProfilePhotoValidator : AbstractValidator<GetProfilePhotoRequest>
{
	public GetProfilePhotoValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
	}
}