using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetFeed;

public record GetFeedRequest(Guid ProfileId, int PageSize, int PageNumber) : IGrpcRequest<GetFeedResponse>;