using AutoMapper;
using Grpc.Core;
using MediatR;
using Posts.Features.GetPhoto;
using Posts.Features.GetPost;
using Posts.Features.GetPosts;

namespace Posts.Grpc.PostService;

public class PostGrpcService(IMediator mediator, IMapper mapper) : PostMicroservice.PostMicroserviceBase
{
	public override async Task<GetPhotoGrpcResponse> GetPhoto(
		GetPhotoGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetPhotoGrpcResponse>(
			await mediator.Send(mapper.Map<GetPhotoRequest>(request), context.CancellationToken));
	}

	public override async Task<GetPostsGrpcResponse> GetPosts(
		GetPostsGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetPostsGrpcResponse>(
			await mediator.Send(mapper.Map<GetPostsRequest>(request), context.CancellationToken));
	}

	public override async Task<GetPostGrpcResponse> GetPost(
		GetPostGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetPostGrpcResponse>(
			await mediator.Send(mapper.Map<GetPostRequest>(request), context.CancellationToken));
	}
}