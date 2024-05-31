using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetProfileName;

public record GetProfileNameRequest(Guid ProfileId) : IGrpcRequest<GetProfileNameResponse>;