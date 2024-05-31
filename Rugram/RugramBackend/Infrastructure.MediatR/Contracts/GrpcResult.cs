namespace Infrastructure.MediatR.Contracts;

public class GrpcResult
{
	protected GrpcResult(int httpStatusCode)
	{
		HttpStatusCode = httpStatusCode;
	}

	public int HttpStatusCode { get; private init; }

	public static implicit operator GrpcResult(int httpStatusCode)
	{
		return new GrpcResult(httpStatusCode);
	}
}

public class GrpcResult<TBody> : GrpcResult
{
	private GrpcResult(int httpStatusCode) : base(httpStatusCode)
	{
	}

	private GrpcResult(TBody body) : base(200)
	{
		Body = body;
	}

	public TBody? Body { get; private init; }

	public static implicit operator GrpcResult<TBody>(TBody body)
	{
		return new GrpcResult<TBody>(body);
	}

	public static implicit operator GrpcResult<TBody>(int httpStatusCode)
	{
		return new GrpcResult<TBody>(httpStatusCode);
	}
}