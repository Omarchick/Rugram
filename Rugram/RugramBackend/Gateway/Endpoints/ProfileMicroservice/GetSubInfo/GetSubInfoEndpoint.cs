using AutoMapper;
using Gateway.Contracts;
using Gateway.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static ProfileMicroservice;


namespace Gateway.Endpoints.ProfileMicroservice.GetSubInfo;

public class GetSubInfoEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("profile/subInfo/{profileId}", async (
				Guid profileId,
				ProfileMicroserviceClient profileClient,
				[FromServices] IHttpContextAccessor httpContextAccessor,
				IMapper mapper,
				CancellationToken cancellationToken) =>
			{
				var response = await profileClient.GetSubInfoAsync(new GetSubInfoGrpcRequest()
					{
						ThisProfileId = httpContextAccessor.HttpContext!.GetUserId().ToString(),
						OtherProfileId = profileId.ToString(),
					},
					cancellationToken: cancellationToken);

				return response.HttpStatusCode switch
				{
					200 => Results.Ok(mapper.Map<GetSubInfoResponse>(response)),
					400 => Results.BadRequest(),
					404 => Results.NotFound(),
					_ => Results.Problem(statusCode: 500)
				};
			})
			.RequireAuthorization()
			.WithOpenApi(generatedOperation =>
			{
				generatedOperation.Parameters[0].Description =
					"Id профиля относительо которого нужно получить информацию";

				return generatedOperation;
			})
			.WithTags("Profile")
			.WithSummary("Получить информацию о том кто на кого подписан")
			.WithDescription("Доступ: авторизованные пользователи")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(StatusCodes.Status401Unauthorized),
				new SwaggerResponseAttribute(StatusCodes.Status400BadRequest,
					"В запросе отправлен свой же id"),
				new SwaggerResponseAttribute(StatusCodes.Status404NotFound,
					"Профиль не найден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status200OK,
					"",
					typeof(GetSubInfoResponse))
			);
	}
}