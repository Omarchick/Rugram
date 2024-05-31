using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.Subscribe;

public class SubscribeRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<SubscribeRequest>
{
	public async Task<GrpcResult> Handle(
		SubscribeRequest request,
		CancellationToken cancellationToken)
	{
		var subscriber = await appDbContext.UserProfiles
			                 .Include(x => x.SubscribedTo)
			                 .FirstOrDefaultAsync(x => x.Id == request.SubscriberId, cancellationToken)
		                 ?? throw new ApplicationException("Пользоваетль не найден");
		var subscribedTo = await appDbContext.UserProfiles
			.FirstOrDefaultAsync(x => x.Id == request.IdOfProfileSubscribedTo, cancellationToken);

		if (subscribedTo is null) return StatusCodes.Status404NotFound;

		if (subscriber.SubscribedTo.Any(x => x.Id == request.IdOfProfileSubscribedTo))
			return StatusCodes.Status204NoContent;

		subscriber.SubscribedTo.Add(subscribedTo);
		await appDbContext.SaveChangesAsync(cancellationToken);

		return StatusCodes.Status204NoContent;
	}
}