using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.PostsMicroservice.GetPhoto;

public record GetPhotoResponse(
	[property: SwaggerSchema("Фото в виде масиива байт")]
	byte[] Photo);