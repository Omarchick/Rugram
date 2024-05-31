using Gateway.Endpoints.AuthMicroservice.Login;
using Gateway.Endpoints.AuthMicroservice.RegisterUser;
using Gateway.Endpoints.AuthMicroservice.SendEmailConfirmation;
using Gateway.Endpoints.AuthMicroservice.UpdateJwtToken;
using Gateway.Endpoints.PostsMicroservice.GetPhoto;
using Gateway.Endpoints.PostsMicroservice.GetPost;
using Gateway.Endpoints.PostsMicroservice.GetPosts;
using Gateway.Endpoints.ProfileMicroservice.GetFeed;
using Gateway.Endpoints.ProfileMicroservice.GetProfileRecommendations;
using Gateway.Endpoints.ProfileMicroservice.GetSubInfo;
using Infrastructure.AutoMapper;

namespace Gateway.AutoMapper;

public class MapperProfile : BaseMappingProfile
{
	public MapperProfile()
	{
		CreateMap<RegisterUserRequest, RegisterUserGrpcRequest>();
		CreateMap<RegisterUserGrpcResponse, RegisterUserResponse>();

		CreateMap<SendEmailConfirmationRequest, SendEmailConfirmationGrpcRequest>();

		CreateMap<LoginRequest, LoginGrpcRequest>();
		CreateMap<LoginGrpcResponse, LoginResponse>();

		CreateMap<UpdateJwtTokenRequest, UpdateJwtTokenGrpcRequest>();
		CreateMap<UpdateJwtTokenGrpcResponse, UpdateJwtTokenResponse>();

		CreateMap<ProfilePostGrpc, ProfilePostDto>();
		CreateMap<GetPostsGrpcResponse, GetPostsResponse>();

		CreateMap<FeedPostGrpcDto, FeedPostDto>();
		CreateMap<GetFeedGrpcResponse, GetFeedResponse>();

		CreateMap<GetSubInfoGrpcResponse, GetSubInfoResponse>();
		
		CreateMap<ProfileGrpcDto, ProfileDto>();
		CreateMap<GetPhotoGrpcResponse, GetPhotoResponse>();

		CreateMap<GetProfileRecommendationsGrpcResponse, GetProfileRecommendationsResponse>();
		
		CreateMap<GetPostGrpcResponse, GetPostResponse>();
	}
}