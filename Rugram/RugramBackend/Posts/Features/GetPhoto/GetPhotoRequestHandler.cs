using Infrastructure.MediatR.Contracts;
using Infrastructure.S3;
using Minio.Exceptions;

namespace Posts.Features.GetPhoto;

public class GetPhotoRequestHandler(IS3StorageService s3StorageService)
	: IGrpcRequestHandler<GetPhotoRequest, GetPhotoResponse>
{
	public async Task<GrpcResult<GetPhotoResponse>> Handle(
		GetPhotoRequest request,
		CancellationToken cancellationToken)
	{
		MemoryStream fileStream;

		try
		{
			fileStream = await s3StorageService.GetFileFromBucketAsync(
				request.PhotoId,
				request.ProfileId,
				cancellationToken);
		}
		catch (BucketNotFoundException)
		{
			return StatusCodes.Status404NotFound;
		}
		catch (ObjectNotFoundException)
		{
			return StatusCodes.Status404NotFound;
		}

		return new GetPhotoResponse(fileStream.ToArray());
	}
}