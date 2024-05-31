using Infrastructure.MediatR.Contracts;

namespace Auth.Features.UpdateJwtToken;

public record UpdateJwtTokenRequest
	(string RefreshToken, string OldJwtToken) : IGrpcRequest<UpdateJwtTokenResponse>;