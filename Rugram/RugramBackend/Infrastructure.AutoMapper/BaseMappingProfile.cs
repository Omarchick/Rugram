using AutoMapper;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Infrastructure.MediatR.Contracts;

namespace Infrastructure.AutoMapper;

public abstract class BaseMappingProfile : Profile
{
	protected BaseMappingProfile()
	{
		BaseConfiguration();
	}

	private void BaseConfiguration()
	{
		CreateMap<ByteString, byte[]>()
			.ConvertUsing(x => x.ToArray());

		CreateMap<byte[], ByteString>()
			.ConvertUsing(x => ByteString.CopyFrom(x));

		CreateMap<Timestamp, DateTime>()
			.ConvertUsing(x => x.ToDateTime());

		CreateMap<DateTime, Timestamp>()
			.ConvertUsing(x => Timestamp.FromDateTime(x));

		CreateMap<string, Guid>()
			.ConvertUsing(x => new Guid(x));

		CreateMap<Guid, string>()
			.ConvertUsing(x => x.ToString());
	}

	protected void CreateMapFromResult<TSource, TDestination>()
	{
		CreateMap<TSource, TDestination>();
		CreateMap<GrpcResult<TSource>, TDestination>()
			.AfterMap((src, dest, context) =>
			{
				if (src.Body != null) context.Mapper.Map(src.Body, dest);
			});
	}

	protected void CreateMapFromResult<TDestination>() =>
		CreateMap<GrpcResult, TDestination>();
}