using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.AuthMicroservice.SendEmailConfirmation;

public record SendEmailConfirmationRequest(
	[property: SwaggerSchema("логин")]
	string Email);