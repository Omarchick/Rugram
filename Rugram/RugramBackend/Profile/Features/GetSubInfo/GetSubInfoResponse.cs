namespace Profile.Features.GetSubInfo;

public record GetSubInfoResponse(
	bool OtherProfileSubscribedToThisProfile,
	bool ThisProfileSubscribedToOtherProfile);