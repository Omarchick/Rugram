using AutoMapper;
using Grpc.Core;
using MediatR;
using PostForProfileMicroservice;
using Posts.Features.GetFeed;
using static PostForProfileMicroservice.PostForProfileMicroservice;

namespace Posts.Grpc.PostForProfileService;

public class PostForProfileService(IMapper mapper, IMediator mediator)
	: PostForProfileMicroserviceBase
{
	public override async Task<GetFeedGrpcResponse> GetFeed(
		GetFeedGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetFeedGrpcResponse>(
			await mediator.Send(mapper.Map<GetFeedRequest>(request), context.CancellationToken));
	}
}