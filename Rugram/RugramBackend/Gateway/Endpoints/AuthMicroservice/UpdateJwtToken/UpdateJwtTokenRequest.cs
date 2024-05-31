using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.UpdateJwtToken;

public record UpdateJwtTokenRequest(
	[property: SwaggerSchema("Рефреш токен")]
	string RefreshToken,
	[property: SwaggerSchema("Устаревший jwt токен")]
	string OldJwtToken);