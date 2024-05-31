using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.UpdateJwtToken;

public record UpdateJwtTokenResponse(
	[property: SwaggerSchema("jwt токен")]
	string JwtToken);