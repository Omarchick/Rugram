using Infrastructure.MediatR.Contracts;

namespace Auth.Features.RegisterUser;

public record RegisterUserRequest(
	string MailConfirmationToken,
	string Email,
	string Password,
	string ProfileName) : IGrpcRequest<RegisterUserResponse>;