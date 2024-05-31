using FluentValidation;

namespace Profile.Features.CreateProfile;

public class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
{
	public CreateProfileRequestValidator()
	{
		RuleFor(x => x.ProfileId)
			.NotEqual(Guid.Empty);
		RuleFor(x => x.ProfileName)
			.Must(x => x.Length is >= 5 and <= 25);
	}
}