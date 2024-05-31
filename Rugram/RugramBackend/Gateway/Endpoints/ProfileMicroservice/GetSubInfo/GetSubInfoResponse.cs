using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetSubInfo;

public record GetSubInfoResponse(
	[property: SwaggerSchema("Подписан ли другой профиль(профиль который в запросе) на меня")]
	bool OtherProfileSubscribedToThisProfile,
	[property: SwaggerSchema("Подписан ли я на другой профиль(профиль который в запросе)")]
	bool ThisProfileSubscribedToOtherProfile);