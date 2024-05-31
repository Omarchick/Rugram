using Infrastructure.MediatR.Contracts;

namespace Posts.Features.GetPosts;

public record GetPostsRequest(Guid ProfileId, int PageSize, int PageNumber) : IGrpcRequest<GetPostsResponse>;