using Contracts.RabbitMq;
using Gateway.Contracts;
using Gateway.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.EditProfilePhoto;

public class EditProfilePhotoEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapPost("profile/editProfilePhoto/", async (
				[FromForm] EditProfilePhotoRequest request,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				IBus bus,
				CancellationToken cancellationToken) =>
			{
				var memoryStream = new MemoryStream();
				await request.ProfilePhoto.CopyToAsync(memoryStream, cancellationToken);

				await bus.Publish(
					new EditProfilePhotoMessage(
						httpContextAccessor.HttpContext!.GetUserId(),
						memoryStream.ToArray()),
					cancellationToken);

				return Results.NoContent();
			})
			.RequireAuthorization()
			.DisableAntiforgery()
			.WithOpenApi()
			.WithTags("Profile")
			.WithSummary("Поменять аватарку")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(
					StatusCodes.Status204NoContent,
					"Запрос выполенен успешно, аватарка должна поменяться в течении определнного времени. ")
			);
	}
}