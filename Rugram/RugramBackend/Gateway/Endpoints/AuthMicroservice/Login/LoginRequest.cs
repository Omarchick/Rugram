using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.Login;

public record LoginRequest(
	[property: SwaggerSchema("Логин")]
	string Email,
	[property: SwaggerSchema("Пароль")]
	string Password);