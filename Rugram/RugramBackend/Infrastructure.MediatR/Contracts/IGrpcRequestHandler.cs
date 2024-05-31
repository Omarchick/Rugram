using MediatR;

namespace Infrastructure.MediatR.Contracts;

public interface IGrpcRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, GrpcResult<TResponse>>
	where TRequest : IGrpcRequest<TResponse>
{
}

public interface IGrpcRequestHandler<in TRequest> : IRequestHandler<TRequest, GrpcResult>
	where TRequest : IGrpcRequest
{
}