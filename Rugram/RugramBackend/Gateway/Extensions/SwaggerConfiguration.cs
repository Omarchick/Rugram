using Microsoft.OpenApi.Models;

namespace Gateway.Extensions;

public static class SwaggerConfiguration
{
	/// <summary>
	/// Конфигурация сваггера
	/// </summary>
	/// <param name="builder">WebApplicationBuilder</param>
	public static void AddSwagger(this WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(options =>
		{
			options.EnableAnnotations();
			options.SwaggerDoc($"v{builder.Configuration["Api:Version"]}",
				new OpenApiInfo
				{
					Title = "Rugram API",
					Version = $"v{builder.Configuration["Api:Version"]}",
					Description = "Rugram API"
				});
			options.AddSecurityDefinition(
				"bearerAuth",
				new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT",
					Description = "JWT Authorization header using the Bearer scheme."
				});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "bearerAuth"
						}
					},
					Array.Empty<string>()
				}
			});
		});
	}
}