using Infrastructure.MediatR.Contracts;

namespace Auth.Features.SendEmailConfirmation;

public record SendEmailConfirmationRequest(string Email) : IGrpcRequest;