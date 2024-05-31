using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Gateway.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using static AuthMicroservice;

namespace Gateway.Endpoints.AuthMicroservice.CheckEmailAvailability;

[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public class CheckEmailAvailabilityEndpoint : IEndpoint
{
	public void AddRoute(IEndpointRouteBuilder app)
	{
		app.MapGet("auth/checkEmailAvailability/{email}", (string email,
				AuthMicroserviceClient authClient,
				IMapper mapper,
				CancellationToken cancellationToken) => Results.NoContent())
			.AllowAnonymous()
			.WithOpenApi(generatedOperation =>
			{
				var parameter = generatedOperation.Parameters[0];
				parameter.Description = "Почта";
				return generatedOperation;
			})
			.WithTags("Auth")
			.WithSummary("Проверка доступности почты")
			.WithDescription("Доступ: все")
			.WithMetadata(
				new SwaggerResponseAttribute(StatusCodes.Status500InternalServerError),
				new SwaggerResponseAttribute(
					StatusCodes.Status400BadRequest,
					"Не пройдена валидация. email - должен быть валиден"),
				new SwaggerResponseAttribute(
					StatusCodes.Status409Conflict,
					"Почта занята"),
				new SwaggerResponseAttribute(
					StatusCodes.Status204NoContent,
					"Почта свободна")
			);
	}
}