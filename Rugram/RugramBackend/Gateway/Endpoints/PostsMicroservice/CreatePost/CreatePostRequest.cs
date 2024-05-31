using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.CreatePost;

public class CreatePostRequest()
{
	[property: SwaggerSchema("Описание поста")]
	public string Description { get; init; } = string.Empty;

	[property: SwaggerSchema("Фотки")]
	public IReadOnlyList<IFormFile> Photos { get; init; } = new List<IFormFile>();
}