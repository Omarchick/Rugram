using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.GetProfileIndicators;

public class GetProfileIndicatorsRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetProfileIndicatorsRequest, GetProfileIndicatorsResponse>
{
	public async Task<GrpcResult<GetProfileIndicatorsResponse>> Handle(
		GetProfileIndicatorsRequest request,
		CancellationToken cancellationToken)
	{
		var userData = await appDbContext.UserProfiles
			.Where(x => x.Id == request.ProfileId)
			.Select(x => new
			{
				SubscribersCount = x.Subscribers
					.Select(subs => subs.Id)
					.Count(),
				SubscribtionsCount = x.SubscribedTo
					.Select(subscribeTo => subscribeTo.Id)
					.Count()
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (userData is null) return StatusCodes.Status404NotFound;

		return new GetProfileIndicatorsResponse(userData.SubscribersCount, userData.SubscribtionsCount);
	}
}