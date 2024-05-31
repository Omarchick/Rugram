using AutoMapper;
using Grpc.Core;
using MediatR;
using Profile.Features.GetFeed;
using Profile.Features.GetProfileIndicators;
using Profile.Features.GetProfileName;
using Profile.Features.GetProfilePhoto;
using Profile.Features.GetProfileRecommendations;
using Profile.Features.GetSubInfo;
using Profile.Features.Subscribe;
using Profile.Features.Unsubscribe;

namespace Profile.Grpc.ProfileService;

public class ProfileGrpcService(IMapper mapper, IMediator mediator)
	: ProfileMicroservice.ProfileMicroserviceBase
{
	public override async Task<SubscribeGrpcResponse> Subscribe(
		SubscribeGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<SubscribeGrpcResponse>(
			await mediator.Send(mapper.Map<SubscribeRequest>(request), context.CancellationToken));
	}

	public override async Task<UnsubscribeGrpcResponse> Unsubscribe(
		UnsubscribeGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<UnsubscribeGrpcResponse>(
			await mediator.Send(mapper.Map<UnsubscribeRequest>(request), context.CancellationToken));
	}

	public override async Task<GetProfileNameGrpcResponse> GetProfileName(
		GetProfileNameGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetProfileNameGrpcResponse>(
			await mediator.Send(mapper.Map<GetProfileNameRequest>(request), context.CancellationToken));
	}

	public override async Task<GetProfilePhotoGrpcResponse> GetProfilePhoto(
		GetProfilePhotoGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetProfilePhotoGrpcResponse>(
			await mediator.Send(mapper.Map<GetProfilePhotoRequest>(request), context.CancellationToken));
	}

	public override async Task<GetProfileIndicatorsGrpcResponse> GetProfileIndicators(
		GetProfileIndicatorsGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetProfileIndicatorsGrpcResponse>(
			await mediator.Send(mapper.Map<GetProfileIndicatorsRequest>(request), context.CancellationToken));
	}

	public override async Task<GetFeedGrpcResponse> GetFeed(
		GetFeedGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetFeedGrpcResponse>(
			await mediator.Send(mapper.Map<GetFeedRequest>(request), context.CancellationToken));
	}

	public override async Task<GetSubInfoGrpcResponse> GetSubInfo(
		GetSubInfoGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetSubInfoGrpcResponse>(
			await mediator.Send(mapper.Map<GetSubInfoRequest>(request), context.CancellationToken));
	}

	public override async Task<GetProfileRecommendationsGrpcResponse> GetProfileRecommendations(
		GetProfileRecommendationsGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<GetProfileRecommendationsGrpcResponse>(
			await mediator.Send(mapper.Map<GetProfileRecommendationsRequest>(request), context.CancellationToken));
	}
}