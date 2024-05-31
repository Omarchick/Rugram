using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.GetSubInfo;

public class GetSubInfoRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetSubInfoRequest, GetSubInfoResponse>
{
	public async Task<GrpcResult<GetSubInfoResponse>> Handle(
		GetSubInfoRequest request,
		CancellationToken cancellationToken)
	{
		var subInfo = await appDbContext.UserProfiles
			.Where(x => x.Id == request.OtherProfileId)
			.Select(x => new
			{
				OtherProfileSubscribedToThisProfile = x.SubscribedTo
					.Select(profile => profile.Id)
					.Contains(request.ThisProfileId),
				ThisProfileSubscribedToOtherProfile = x.Subscribers
					.Select(profile => profile.Id)
					.Contains(request.ThisProfileId)
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (subInfo is null) return StatusCodes.Status404NotFound;

		return new GetSubInfoResponse(
			subInfo.OtherProfileSubscribedToThisProfile,
			subInfo.ThisProfileSubscribedToOtherProfile);
	}
}