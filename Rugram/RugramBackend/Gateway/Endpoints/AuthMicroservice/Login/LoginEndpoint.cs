using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static AuthMicroservice;

namespace Gateway.Endpoints.AuthMicroservice.Login;

public class LoginEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPost("auth/login", async (
				LoginRequest request,
				AuthMicroserviceClient authClient,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await authClient.LoginAsync(
					mapper.Map<LoginGrpcRequest>(request), cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<LoginResponse>(response)),
					400 => Results.BadRequest(),
					403 => Results.Forbid(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.AllowAnonymous()
			.WithOpenApi()
			.WithTags("Auth")
			.WithSummary("Вход по логину и паролю")
			.WithDescription("Доступ: все")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					"Не пройдена валидация. email - должен быть валиден, password length от 5 до 25 "),
				new SwaggerResponseAttribute(
					StatusCodes.Status403Forbidden,
					"Пароль не верный"),
				new SwaggerResponseAttribute(
					StatusCodes.Status404NotFound,
					"Пользователь с таким email не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(LoginResponse))
			);
	}
}