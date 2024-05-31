using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Posts.Data;

namespace Posts.Features.GetPosts;

public class GetPostsRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetPostsRequest, GetPostsResponse>
{
	public async Task<GrpcResult<GetPostsResponse>> Handle(
		GetPostsRequest request,
		CancellationToken cancellationToken)
	{
		var totalPostsCount = await appDbContext.Posts
			.Where(x => x.ProfileId == request.ProfileId)
			.CountAsync(cancellationToken);

		if (totalPostsCount <= request.PageSize * request.PageNumber)
			return new GetPostsResponse(new List<ProfilePostDto>(), totalPostsCount);

		var posts = await appDbContext.Posts
			.Where(x => x.ProfileId == request.ProfileId)
			.OrderByDescending(x => x.DateOfCreation)
			.Select(x => new ProfilePostDto(
				x.Id,
				x.Description,
				x.DateOfCreation,
				x.Photos
					.Select(photo => photo.Id)
					.ToArray()))
			.Skip(request.PageSize * request.PageNumber)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		return new GetPostsResponse(posts, totalPostsCount);
	}
}