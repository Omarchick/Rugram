using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;
using static PostForProfileMicroservice.PostForProfileMicroservice;

namespace Profile.Features.GetFeed;

public class GetFeedRequestHandler(
		AppDbContext appDbContext,
		PostForProfileMicroserviceClient postMicroserviceClient)
	: IGrpcRequestHandler<GetFeedRequest, GetFeedResponse>
{
	public async Task<GrpcResult<GetFeedResponse>> Handle(
		GetFeedRequest request,
		CancellationToken cancellationToken)
	{
		var profileData = await appDbContext.UserProfiles
			.Where(x => x.Id == request.ProfileId)
			.Select(x => new
			{
				SubscriptionIds = x.SubscribedTo
					.Select(profile => profile.Id.ToString()),
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (profileData == null) return StatusCodes.Status404NotFound;

		var response = await postMicroserviceClient.GetFeedAsync(
			new PostForProfileMicroservice.GetFeedGrpcRequest()
			{
				PageNumber = request.PageNumber,
				PageSize = request.PageSize,
				SubscriptionIds = { profileData.SubscriptionIds }
			}, cancellationToken: cancellationToken);

		if (response.HttpStatusCode != StatusCodes.Status200OK) return response.HttpStatusCode;

		var profileIdToName = await appDbContext.UserProfiles
			.Where(x => response.FeedPostDto
				.Select(postGrpcDto => new Guid(postGrpcDto.ProfileId))
				.Contains(x.Id))
			.ToDictionaryAsync(x => x.Id, x => x.ProfileName, cancellationToken);

		return new GetFeedResponse(response.FeedPostDto
			.Select(x => new FeedPostDto(
				new Guid(x.ProfileId),
				new Guid(x.PostId),
				profileIdToName[new Guid(x.ProfileId)],
				x.Description,
				x.DateOfCreation.ToDateTime(),
				x.PhotoIds
					.Select(id => new Guid(id))
					.ToArray()))
			.ToArray());
	}
}