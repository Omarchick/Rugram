using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static AuthMicroservice;

namespace Gateway.Endpoints.AuthMicroservice.UpdateJwtToken;

public class UpdateJwtTokenEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPut("auth/update-jwt", async (
				UpdateJwtTokenRequest request,
				AuthMicroserviceClient authClient,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await authClient.UpdateJwtTokenGrpcAsync(
					mapper.Map<UpdateJwtTokenGrpcRequest>(request), cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<UpdateJwtTokenResponse>(response)),
					400 => Results.BadRequest(),
					403 => Results.Forbid(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.AllowAnonymous()
			.WithOpenApi()
			.WithTags("Auth")
			.WithSummary("Обновить jwt токен")
			.WithDescription("Доступ: все")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest,
					"Валидация не пройдена (не одно поле не должно быть пустым)," +
					" или jwt токен не валиден, там нет нужных Claims"),
				new SwaggerResponseAttribute(StatusCodes.Status403Forbidden,
					"Рефреш токен устарел"),
				new SwaggerResponseAttribute(StatusCodes.Status404NotFound,
					"Рефреш токен не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(UpdateJwtTokenResponse)));
	}
}