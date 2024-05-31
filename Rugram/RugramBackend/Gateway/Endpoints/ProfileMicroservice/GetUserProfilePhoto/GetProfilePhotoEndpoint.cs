using Gateway.Contracts;
using Gateway.Endpoints.PostsMicroservice.GetPhoto;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;

namespace Gateway.Endpoints.ProfileMicroservice.GetUserProfilePhoto;

public class GetProfilePhotoEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/profilePhoto/{profileId}", async (
				Guid profileId,
				ProfileMicroserviceClient profileClient,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetProfilePhotoAsync(new GetProfilePhotoGrpcRequest()
					{
						ProfileId = profileId.ToString()
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(new GetProfilePhotoResponse(response.ProfilePhoto.ToByteArray())),
					204 => Results.NoContent(),
					400 => Results.BadRequest(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				var parameter = generatedOperation.Parameters[0];
				parameter.Description = "Id профиля(или пользователя) аву которого надо получить";
				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Получить аватарку пользователя")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(StatusCodes.Status204NoContent,
					"У пользователя нет аватарки"),
				new SwaggerResponseAttribute(StatusCodes.Status404NotFound,
					"Профиль с таким id не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetProfilePhotoResponse))
			);
	}
}