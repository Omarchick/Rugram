using AutoMapper;
using Gateway.Contracts;
using Gateway.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;

namespace Gateway.Endpoints.ProfileMicroservice.GetFeed;

public class GetFeedEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/feed/{pageSize}&{pageNumber}", async (
				int pageSize,
				int pageNumber,
				ProfileMicroserviceClient profileClient,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetFeedAsync(new GetFeedGrpcRequest()
					{
						ProfileId = httpContextAccessor.HttpContext!.GetUserId().ToString(),
						PageSize = pageSize,
						PageNumber = pageNumber
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetFeedResponse>(response)),
					400 => Results.BadRequest(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description = "Размер страницы";
				generatedOperation.Parameters[1].Description = "Номер страницы";

				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Получить ленту")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest,
					"Не пройдена валидация. pageSize is > -1 and <= 1000," +
					$" pageNumber is > -1 and < 100000, {int.MaxValue} / pageNumber >= pageSize"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetFeedResponse))
			);
	}
}