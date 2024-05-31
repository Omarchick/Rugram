using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.RegisterUser;

public record RegisterUserRequest(
	[property: SwaggerSchema("Токен для подтверждения почты")]
	string MailConfirmationToken,
	[property: SwaggerSchema("Логин")]
	string Email,
	[property: SwaggerSchema("Пароль для нового аккаунта")]
	string Password,
	[property: SwaggerSchema("Название профиля")]
	string ProfileName);