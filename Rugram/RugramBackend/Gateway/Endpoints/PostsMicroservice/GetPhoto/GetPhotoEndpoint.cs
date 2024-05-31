using AutoMapper;
using Gateway.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static PostMicroservice;

namespace Gateway.Endpoints.PostsMicroservice.GetPhoto;

public class GetPhotoEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("post/photo/{photoId}&{profileId}", async (
				Guid photoId,
				Guid profileId,
				PostMicroserviceClient postClient,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await postClient.GetPhotoAsync(new GetPhotoGrpcRequest()
					{
						ProfileId = profileId.ToString(),
						PhotoId = photoId.ToString()
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetPhotoResponse>(response)),
					400 => Results.BadRequest(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description = "Id фотки";
				generatedOperation.Parameters[1].Description = "Id профиля";
				return generatedOperation;
			})
			.WithTags("Post")
			.WithSummary("Получить фото по id профиля и id фото")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status404NotFound,
					"Файл не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					"Один из id имеет неккоректный формат"),
				new SwaggerResponseAttribute(
					StatusCodes.Status401Unauthorized,
					"Пользователь не авторизован"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetPhotoResponse)));
	}
}