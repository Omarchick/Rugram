namespace Auth.Features.RegisterUser;

public record RegisterUserResponse(
	string RefreshToken,
	string JwtToken);