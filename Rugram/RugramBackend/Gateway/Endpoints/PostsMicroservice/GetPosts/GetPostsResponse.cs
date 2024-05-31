using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.GetPosts;

public record GetPostsResponse(
	[property: SwaggerSchema("Посты")]
	ProfilePostDto[] Posts,
	[property: SwaggerSchema("Общее колличество постов")]
	int TotalPostsCount);

public record ProfilePostDto(
	[property: SwaggerSchema("Id поста")]
	Guid PostId,
	[property: SwaggerSchema("Описание фотки")]
	string Description,
	[property: SwaggerSchema("Дата создания поста")]
	DateTime DateOfCreation,
	[property: SwaggerSchema("Ids фоток данного поста")]
	Guid[] PhotoIds);