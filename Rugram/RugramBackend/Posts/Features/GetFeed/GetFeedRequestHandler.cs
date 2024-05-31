using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Posts.Data;

namespace Posts.Features.GetFeed;

public class GetFeedRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetFeedRequest, GetFeedResponse>
{
	public async Task<GrpcResult<GetFeedResponse>> Handle(
		GetFeedRequest request,
		CancellationToken cancellationToken)
	{
		var posts = await appDbContext.Posts
			.Where(x => request.SubscriptionIds.Contains(x.ProfileId))
			.OrderByDescending(x => x.DateOfCreation)
			.Skip(request.PageSize * request.PageNumber)
			.Take(request.PageSize)
			.Select(x => new FeedPostDto(
				x.ProfileId,
				x.Id,
				x.Description,
				x.DateOfCreation,
				x.Photos
					.Select(photo => photo.Id)
					.ToArray()))
			.ToArrayAsync(cancellationToken);

		return new GetFeedResponse(posts);
	}
}