using Swashbuckle.AspNetCore.Annotations;

namespace Gateway.Endpoints.ProfileMicroservice.EditProfilePhoto;

public record EditProfilePhotoRequest(
	[property: SwaggerSchema("Фотка для профиля")]
	IFormFile ProfilePhoto);