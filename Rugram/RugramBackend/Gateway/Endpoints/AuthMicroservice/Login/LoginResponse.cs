using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.Login;

public record LoginResponse(
	[property: SwaggerSchema("Рефреш токен")]
	string RefreshToken,
	[property: SwaggerSchema("Jwt токен")]
	string JwtToken);