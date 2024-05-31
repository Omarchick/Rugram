namespace Auth.Features.Login;

public record LoginResponse(string JwtToken, string RefreshToken);