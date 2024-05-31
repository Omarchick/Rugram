using Contracts.RabbitMq;
using Gateway.Contracts;
using Gateway.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.CreatePost;

public class CreatePostEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPost("post", async (
				[FromForm] CreatePostRequest request,
				IBus bus,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				CancellationToken cancellationToken) =>
			{
				if (request.Description.Length > 3000) return Results.BadRequest();
				if (request.Photos.Count is > 10 or < 1) return Results.BadRequest();

				await bus.Publish(
					new CreatePostMessage(
						Guid.NewGuid(),
						httpContextAccessor.HttpContext!.GetUserId(),
						request.Description,
						(await Task.WhenAll(request.Photos
							.Select(async x =>
							{
								var memoryStream = new MemoryStream();
								await x.CopyToAsync(memoryStream, cancellationToken);
								return new PhotoStream(memoryStream.ToArray());
							}))).ToList()),
					cancellationToken);

				return Results.NoContent();
			})
			.RequireAuthorization()
			.DisableAntiforgery()
			.WithOpenApi()
			.WithTags("Post")
			.WithSummary("Создание поста")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					"Не пройдена валидация. Количестов фото, не больше 10 и не меньше 1, " +
					"Description не больше 3000 символов"),
				new SwaggerResponseAttribute(
					StatusCodes.Status401Unauthorized,
					"Пользователь не авторизован"),
				new SwaggerResponseAttribute(
					StatusCodes.Status204NoContent,
					"Пост будет создан в течении определенного времени. "));
	}
}