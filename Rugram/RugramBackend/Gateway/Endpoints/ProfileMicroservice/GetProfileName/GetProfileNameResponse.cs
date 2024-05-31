using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileName;

public record GetProfileNameResponse(
	[property: SwaggerSchema("Ник профиля")]
	string ProfileName);