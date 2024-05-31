using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetFeed;

public record GetFeedResponse(
	[property: SwaggerSchema("Посты в новостной ленте. ")]
	FeedPostDto[] FeedPostDto);

public record FeedPostDto(
	[property: SwaggerSchema("Id профиля сделавшего пост")]
	Guid ProfileId,
    [property: SwaggerSchema("Id профиля сделавшего пост")]
	Guid PostId,
	[property: SwaggerSchema("Название профиля сделавшего пост")]
	string ProfileName,
	[property: SwaggerSchema("Описание поста")]
	string Description,
	[property: SwaggerSchema("Дата создания поста")]
	DateTime DateOfCreation,
	[property: SwaggerSchema("Ids фотографий поста")]
	Guid[] PhotoIds);