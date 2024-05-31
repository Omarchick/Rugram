using Infrastructure.MediatR.Contracts;

namespace Posts.Features.GetFeed;

public record GetFeedRequest(Guid[] SubscriptionIds, int PageSize, int PageNumber) : IGrpcRequest<GetFeedResponse>;