using MediatR;

namespace Infrastructure.MediatR.Contracts;

public interface IGrpcRequest<TResponse> : IRequest<GrpcResult<TResponse>>
{
}

public interface IGrpcRequest : IRequest<GrpcResult>
{
}