using Infrastructure.AutoMapper;
using Profile.Features.CreateProfile;
using Profile.Features.GetFeed;
using Profile.Features.GetProfileIndicators;
using Profile.Features.GetProfileName;
using Profile.Features.GetProfilePhoto;
using Profile.Features.GetProfileRecommendations;
using Profile.Features.GetSubInfo;
using Profile.Features.Subscribe;
using Profile.Features.Unsubscribe;

namespace Profile.AutoMapper;

public class MapperProfile : BaseMappingProfile
{
	public MapperProfile()
	{
		CreateMap<CreateProfileGrpcRequest, CreateProfileRequest>();
		CreateMapFromResult<CreateProfileGrpcResponse>();

		CreateMap<SubscribeGrpcRequest, SubscribeRequest>();
		CreateMapFromResult<SubscribeGrpcResponse>();

		CreateMap<UnsubscribeGrpcRequest, UnsubscribeRequest>();
		CreateMapFromResult<UnsubscribeGrpcResponse>();

		CreateMap<GetProfileNameGrpcRequest, GetProfileNameRequest>();
		CreateMapFromResult<GetProfileNameResponse, GetProfileNameGrpcResponse>();

		CreateMap<GetProfilePhotoGrpcRequest, GetProfilePhotoRequest>();
		CreateMapFromResult<GetProfilePhotoResponse, GetProfilePhotoGrpcResponse>();

		CreateMap<GetProfileIndicatorsGrpcRequest, GetProfileIndicatorsRequest>();
		CreateMapFromResult<GetProfileIndicatorsResponse, GetProfileIndicatorsGrpcResponse>();
		
		CreateMap<GetSubInfoGrpcRequest, GetSubInfoRequest>();
		CreateMapFromResult<GetSubInfoResponse, GetSubInfoGrpcResponse>();

		CreateMap<GetFeedGrpcRequest, GetFeedRequest>();
		CreateMap<FeedPostDto, FeedPostGrpcDto>();
		CreateMapFromResult<GetFeedResponse, GetFeedGrpcResponse>();

		CreateMap<ProfileDto, ProfileGrpcDto>();
		CreateMap<GetProfileRecommendationsGrpcRequest, GetProfileRecommendationsRequest>();
		CreateMapFromResult<GetProfileRecommendationsResponse, GetProfileRecommendationsGrpcResponse>();
	}
}