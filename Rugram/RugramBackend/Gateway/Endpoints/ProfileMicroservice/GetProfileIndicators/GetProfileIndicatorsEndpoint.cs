using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileIndicators;

public class GetProfileIndicatorsEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/profileIndicators/{profileId}", async (
				Guid profileId,
				ProfileMicroserviceClient profileClient,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetProfileIndicatorsAsync(new GetProfileIndicatorsGrpcRequest()
					{
						ProfileId = profileId.ToString()
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(new GetProfileIndicatorsResponse(
						response.SubscribersCount,
						response.SubscriptionsCount)),
					400 => Results.BadRequest(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				var parameter = generatedOperation.Parameters[0];
				parameter.Description =
					"Id профиля(или пользователя) количество подписчиков и подписок которого надо получить";
				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Получить количество подписчиков и подписок пользоваетеля")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(StatusCodes.Status404NotFound,
					"Профиль с таким id не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetProfileIndicatorsResponse))
			);
	}
}