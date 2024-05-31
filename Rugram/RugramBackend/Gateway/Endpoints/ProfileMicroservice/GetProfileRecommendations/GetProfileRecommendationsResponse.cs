using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileRecommendations;

public record GetProfileRecommendationsResponse(
	[property: SwaggerSchema("Профили")]
	ProfileDto[] Profiles);

public record ProfileDto(
	[property: SwaggerSchema("Id профиля")]
	Guid Id,
	[property: SwaggerSchema("Ник профиля")]
	string ProfileName);