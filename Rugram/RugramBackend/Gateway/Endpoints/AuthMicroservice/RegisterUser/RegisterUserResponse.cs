using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.RegisterUser;

public record RegisterUserResponse(
	[property: SwaggerSchema("Jwt токен")]
	string JwtToken,
	[property: SwaggerSchema("RefreshToken")]
	string RefreshToken);