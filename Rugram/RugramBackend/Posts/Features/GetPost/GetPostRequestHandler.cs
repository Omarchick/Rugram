using Infrastructure.MediatR.Contracts;
using Infrastructure.S3;
using Microsoft.EntityFrameworkCore;
using Posts.Data;

namespace Posts.Features.GetPost;

public class GetPostRequestHandler(IS3StorageService storageService, AppDbContext appDbContext)
	: IGrpcRequestHandler<GetPostRequest, GetPostResponse>
{
	public async Task<GrpcResult<GetPostResponse>> Handle(
		GetPostRequest request,
		CancellationToken cancellationToken)
	{
		var post = await appDbContext.Posts
			.Where(x => x.Id == request.PostId)
			.Select(x => new
			{
				x.ProfileId,
				x.Description,
				x.DateOfCreation,
				PhotoIds = x.Photos
					.Select(photo => photo.Id)
					.ToList()
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (post is null) return StatusCodes.Status404NotFound;

		var tasks = new List<Task<MemoryStream>>();

		foreach (var photoId in post.PhotoIds)
		{
			tasks.Add(
				storageService.GetFileFromBucketAsync(photoId, post.ProfileId, cancellationToken));
		}

		await Task.WhenAll(tasks);

		return new GetPostResponse(
			post.ProfileId,
			post.DateOfCreation,
			post.Description,
			tasks
				.Select(x => x.Result.ToArray())
				.ToArray());
	}
}