using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.GetPosts;

public class GetPostsEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("post/{profileId}&{pageNumber}&{pageSize}", async (
				Guid profileId,
				int pageNumber,
				int pageSize,
				PostMicroservice.PostMicroserviceClient postClient,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await postClient.GetPostsAsync(new GetPostsGrpcRequest()
					{
						ProfileId = profileId.ToString(),
						PageSize = pageSize,
						PageNumber = pageNumber
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetPostsResponse>(response)),
					400 => Results.BadRequest(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description = "Id профиля";
				generatedOperation.Parameters[1].Description = "Номер страницы";
				generatedOperation.Parameters[2].Description = "Размер страницы";
				return generatedOperation;
			})
			.WithTags("Post")
			.WithSummary("Получить все посты пользователя в порядке публикации с пагинацией")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					$"Не пройдена валидация. pageSize is > -1 and <= 1000," +
					$" pageNumber is > -1 and < 100000, {int.MaxValue} / pageNumber >= pageSize"),
				new SwaggerResponseAttribute(
					StatusCodes.Status401Unauthorized,
					"Пользователь не авторизован"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetPostsResponse)));
	}
}