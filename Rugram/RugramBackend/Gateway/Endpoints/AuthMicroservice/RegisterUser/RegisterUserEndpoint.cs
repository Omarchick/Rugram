using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static AuthMicroservice;

namespace Gateway.Endpoints.AuthMicroservice.RegisterUser;

public class RegisterUserEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPost("auth/registr-user", async (
				RegisterUserRequest request,
				AuthMicroserviceClient authClient,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await authClient.RegisterUserAsync(
					mapper.Map<RegisterUserGrpcRequest>(request), cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<RegisterUserResponse>(response)),
					400 => Results.BadRequest(),
					403 => Results.Forbid(),
					404 => Results.NotFound(),
					409 => Results.Conflict(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.AllowAnonymous()
			.WithOpenApi()
			.WithTags("Auth")
			.WithSummary("Регистрация пользователя")
			.WithDescription("Доступ: все")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest,
					"Не пройдена валидация. email должен быть валиден," +
					" password length от 5 до 25, ProfileName length от 5 до 25"),
				new SwaggerResponseAttribute(StatusCodes.Status404NotFound,
					"Токен для подтверждения пароля не найден. "),
				new SwaggerResponseAttribute(StatusCodes.Status403Forbidden,
					"Токен просрочен"),
				new SwaggerResponseAttribute(StatusCodes.Status409Conflict,
					"Пользователь с таким email или profile name уже существует"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(RegisterUserResponse)));
	}
}