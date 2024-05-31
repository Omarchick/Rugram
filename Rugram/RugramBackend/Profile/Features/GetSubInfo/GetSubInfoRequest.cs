using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetSubInfo;

public record GetSubInfoRequest(Guid ThisProfileId, Guid OtherProfileId) : IGrpcRequest<GetSubInfoResponse>;