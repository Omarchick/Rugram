using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetProfileIndicators;

public record GetProfileIndicatorsResponse(
	[property: SwaggerSchema("Количество подписчиков")]
	int SubscribersCount,
	[property: SwaggerSchema("Количество подписок на аккаунт")]
	int SubscriptionsCount);