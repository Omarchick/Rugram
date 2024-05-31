using Auth.Features.Login;
using Auth.Features.RegisterUser;
using Auth.Features.SendEmailConfirmation;
using Auth.Features.UpdateJwtToken;
using Infrastructure.AutoMapper;

namespace Auth.AutoMapper;

public class MapperProfile : BaseMappingProfile
{
	public MapperProfile()
	{
		CreateMap<RegisterUserGrpcRequest, RegisterUserRequest>();
		CreateMapFromResult<RegisterUserResponse, RegisterUserGrpcResponse>();

		CreateMap<SendEmailConfirmationGrpcRequest, SendEmailConfirmationRequest>();
		CreateMapFromResult<SendEmailConfirmationGrpcResponse>();

		CreateMap<LoginGrpcRequest, LoginRequest>();
		CreateMapFromResult<LoginResponse, LoginGrpcResponse>();

		CreateMap<UpdateJwtTokenGrpcRequest, UpdateJwtTokenRequest>();
		CreateMapFromResult<UpdateJwtTokenResponse, UpdateJwtTokenGrpcResponse>();
	}
}