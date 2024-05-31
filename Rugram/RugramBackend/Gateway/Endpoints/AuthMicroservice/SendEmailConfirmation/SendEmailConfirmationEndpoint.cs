using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static AuthMicroservice;

namespace Gateway.Endpoints.AuthMicroservice.SendEmailConfirmation;

public class SendEmailConfirmationEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPost("auth/confirm-email", async (
				IMapper mapper,
				AuthMicroserviceClient authClient,
				SendEmailConfirmationRequest request,
				CancellationToken cancellationToken) =>
			{
				var response = await authClient.SendEmailConfirmationAsync(
					mapper.Map<SendEmailConfirmationGrpcRequest>(request), cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					204 => Results.NoContent(),
					400 => Results.BadRequest(),
					409 => Results.Conflict(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.AllowAnonymous()
			.WithOpenApi()
			.WithTags("Auth")
			.WithSummary("Скинуть на почту письмо для подтверждения почты")
			.WithDescription("Доступ: все")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status204NoContent, "OK"),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest,
					"Не пройдена валидация. email должен быть валиден. "),
				new SwaggerResponseAttribute(StatusCodes.Status409Conflict,
					"Пользователь с таким email уже существует. "));
	}
}