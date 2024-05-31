using Auth.Features.Login;
using Auth.Features.RegisterUser;
using Auth.Features.SendEmailConfirmation;
using Auth.Features.UpdateJwtToken;
using AutoMapper;
using Grpc.Core;
using MediatR;

namespace Auth.Grpc.AuthService;

public class AuthGrpcService(IMediator mediator, IMapper mapper) : AuthMicroservice.AuthMicroserviceBase
{
	public override async Task<RegisterUserGrpcResponse> RegisterUser(
		RegisterUserGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<RegisterUserGrpcResponse>(
			await mediator.Send(mapper.Map<RegisterUserRequest>(request)));
	}

	public override async Task<SendEmailConfirmationGrpcResponse> SendEmailConfirmation(
		SendEmailConfirmationGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<SendEmailConfirmationGrpcResponse>(
			await mediator.Send(mapper.Map<SendEmailConfirmationRequest>(request), context.CancellationToken));
	}

	public override async Task<LoginGrpcResponse> Login(
		LoginGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<LoginGrpcResponse>(
			await mediator.Send(mapper.Map<LoginRequest>(request), context.CancellationToken));
	}

	public override async Task<UpdateJwtTokenGrpcResponse> UpdateJwtTokenGrpc(
		UpdateJwtTokenGrpcRequest request,
		ServerCallContext context)
	{
		return mapper.Map<UpdateJwtTokenGrpcResponse>(
			await mediator.Send(mapper.Map<UpdateJwtTokenRequest>(request), context.CancellationToken));
	}
}