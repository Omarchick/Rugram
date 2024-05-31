using Infrastructure.MediatR.Contracts;

namespace Posts.Features.GetPost;

public record GetPostRequest(Guid PostId) : IGrpcRequest<GetPostResponse>;