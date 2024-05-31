using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileName;

public class GetProfileNameEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/profileName/{profileId}", async (
				Guid profileId,
				ProfileMicroserviceClient profileClient,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetProfileNameAsync(new GetProfileNameGrpcRequest()
					{
						ProfileId = profileId.ToString()
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(new GetProfileNameResponse(response.ProfileName)),
					400 => Results.BadRequest(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				var parameter = generatedOperation.Parameters[0];
				parameter.Description = "Id профиля(или пользователя) ник которого надо получить";
				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Получить ник профиля")
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
					typeof(GetProfileNameResponse))
			);
	}
}