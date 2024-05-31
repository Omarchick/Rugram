using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static PostMicroservice;

namespace Gateway.Endpoints.PostsMicroservice.GetPost;

public class GetPostEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("post/{postId}", async (
				Guid postId,
				PostMicroserviceClient postClient,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await postClient.GetPostAsync(new GetPostGrpcRequest()
				{
					PostId = postId.ToString()
				}, cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetPostResponse>(response)),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description = "Id поста";

				return generatedOperation;
			})
			.WithTags("Post")
			.WithSummary("Получить пост по его id")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status404NotFound,
					"Пост не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status401Unauthorized,
					"Пользователь не авторизован"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetPostResponse)));
	}
}