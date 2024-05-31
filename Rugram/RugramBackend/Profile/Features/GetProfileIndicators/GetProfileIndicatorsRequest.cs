using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetProfileIndicators;

public record GetProfileIndicatorsRequest(Guid ProfileId) : IGrpcRequest<GetProfileIndicatorsResponse>;