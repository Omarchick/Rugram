using AutoMapper;
using Gateway.Contracts;
using Gateway.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileRecommendations;

public class GetProfileRecommendationsEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/recommendations/{pageSize}&{pageNumber}/{searchString?}", async (
				string? searchString,
				int pageSize,
				int pageNumber,
				IMapper mapper,
				ProfileMicroserviceClient profileClient,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetProfileRecommendationsAsync(
					new GetProfileRecommendationsGrpcRequest()
					{
						ProfileId = httpContextAccessor.HttpContext!.GetUserId().ToString(),
						PageSize = pageSize,
						PageNumber = pageNumber,
						SearchString = searchString ?? string.Empty
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetProfileRecommendationsResponse>(response)),
					400 => Results.BadRequest(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description = "Строка для поиска";
				generatedOperation.Parameters[1].Description = "Размер страницы";
				generatedOperation.Parameters[2].Description = "Номер старницы";

				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Поиск пользователя/рекомендации пользователей")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					$"Не пройдена валидация. searchString меньше 25 символов,  pageSize is > -1 and <= 1000," +
					$" pageNumber is > -1 and < 100000, {int.MaxValue} / pageNumber = pageSize"),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetProfileRecommendationsResponse))
			);
	}
}