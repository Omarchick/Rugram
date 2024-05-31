using MassTransit;

namespace Auth.Extensions;

public static class RabbitMqConfiguration
{
	public static void AddMasstransitRabbitMq(this WebApplicationBuilder builder, int attemptsCount = 20)
	{
		builder.Services.AddMassTransit(config =>
		{
			config.UsingRabbitMq((ctx, cfg) =>
			{
				cfg.Host(
					$"amqp://{builder.Configuration["RabbitMqConfig:Username"]}:{builder.Configuration["RabbitMqConfig:Password"]}" +
					$"@{builder.Configuration["RabbitMqConfig:Hostname"]}:{builder.Configuration["RabbitMqConfig:Port"]}");
				cfg.ConfigureEndpoints(ctx);
			});
		});
	}
}