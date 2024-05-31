using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.GetUserProfilePhoto;

public record GetProfilePhotoResponse(
	[property: SwaggerSchema("Фото профиля")]
	byte[] ProfilePhoto);