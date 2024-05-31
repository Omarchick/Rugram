using Infrastructure.MediatR.Contracts;

namespace Auth.Features.Login;

public record LoginRequest(string Email, string Password) : IGrpcRequest<LoginResponse>;