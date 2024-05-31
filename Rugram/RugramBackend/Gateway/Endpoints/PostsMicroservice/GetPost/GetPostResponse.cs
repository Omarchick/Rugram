using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.GetPost;

public record GetPostResponse(
	[property: SwaggerSchema("Id профиля")]
	Guid ProfileId,
	[property: SwaggerSchema("Дата создания")]
	DateTime DateOfCreation,
	[property: SwaggerSchema("Описание фотки")]
	string Description,
	[property: SwaggerSchema("Фотки")]
	byte[][] Photos);