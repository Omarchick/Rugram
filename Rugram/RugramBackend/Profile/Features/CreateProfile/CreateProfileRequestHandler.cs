using Infrastructure.MediatR.Contracts;
using Infrastructure.S3;
using Microsoft.EntityFrameworkCore;
using Profile.Data;
using Profile.Data.Models;

namespace Profile.Features.CreateProfile;

public class CreateProfileRequestHandler(AppDbContext appDbContext, IS3StorageService s3StorageService)
	: IGrpcRequestHandler<CreateProfileRequest>
{
	public async Task<GrpcResult> Handle(
		CreateProfileRequest request,
		CancellationToken cancellationToken)
	{
		var existUserWithThisProfileName = await appDbContext.UserProfiles
			.AnyAsync(x => x.ProfileName == request.ProfileName, cancellationToken);

		if (existUserWithThisProfileName) return StatusCodes.Status409Conflict;

		var profile = new UserProfile(request.ProfileId, request.ProfileName);

		var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);

		appDbContext.UserProfiles.Add(profile);
		await appDbContext.SaveChangesAsync(cancellationToken);

		try
		{
			await s3StorageService.CreateBucketAsync(profile.Id, cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}
		catch (Exception)
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}

		return StatusCodes.Status204NoContent;
	}
}