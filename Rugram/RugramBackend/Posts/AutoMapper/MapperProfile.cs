using Infrastructure.AutoMapper;
using PostForProfileMicroservice;
using Posts.Features.GetFeed;
using Posts.Features.GetPhoto;
using Posts.Features.GetPost;
using Posts.Features.GetPosts;

namespace Posts.AutoMapper;

public class MapperProfile : BaseMappingProfile
{
	public MapperProfile()
	{
		CreateMap<GetPhotoGrpcRequest, GetPhotoRequest>();
		CreateMapFromResult<GetPhotoResponse, GetPhotoGrpcResponse>();
		
		CreateMap<GetPhotoGrpcRequest, GetPhotoRequest>();
		CreateMapFromResult<GetPhotoResponse, GetPhotoGrpcResponse>();
		
		CreateMap<GetPostGrpcRequest, GetPostRequest>();
		CreateMapFromResult<GetPostResponse, GetPostGrpcResponse>();

		CreateMap<GetPostsGrpcRequest, GetPostsRequest>();
		CreateMap<ProfilePostDto, ProfilePostGrpc>();
		CreateMapFromResult<GetPostsResponse, GetPostsGrpcResponse>();

		CreateMap<GetFeedGrpcRequest, GetFeedRequest>();
		CreateMap<FeedPostDto, FeedPostGrpcDto>();
		CreateMapFromResult<GetFeedResponse, GetFeedGrpcResponse>();
	}
}