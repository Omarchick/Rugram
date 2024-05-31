using Infrastructure.MediatR.Contracts;
using Infrastructure.S3;
using Microsoft.EntityFrameworkCore;
using Minio.Exceptions;
using Profile.Data;

namespace Profile.Features.GetProfilePhoto;

public class GetProfilePhotoRequestHandler(IS3StorageService s3StorageService, AppDbContext appDbContext)
	: IGrpcRequestHandler<GetProfilePhotoRequest, GetProfilePhotoResponse>
{
	public async Task<GrpcResult<GetProfilePhotoResponse>> Handle(
		GetProfilePhotoRequest request,
		CancellationToken cancellationToken)
	{
		var userExist = appDbContext.UserProfiles
			.AnyAsync(x => x.Id == request.ProfileId, cancellationToken);

		MemoryStream photo = new MemoryStream();

		try
		{
			photo = await s3StorageService.GetFileFromBucketAsync(
				request.ProfileId,
				request.ProfileId,
				cancellationToken);
		}
		catch (BucketNotFoundException)
		{
			if (await userExist)
				throw new ApplicationException("У пользователя нет бакета в хранилище аваторок");
		}
		catch (ObjectNotFoundException)
		{
			if (await userExist)
				return StatusCodes.Status204NoContent;
		}

		if (!await userExist)
			return StatusCodes.Status404NotFound;


		return new GetProfilePhotoResponse(photo.ToArray());
	}
}