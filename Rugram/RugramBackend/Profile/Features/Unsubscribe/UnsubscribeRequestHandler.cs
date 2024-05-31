using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.Unsubscribe;

public class UnsubscribeRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<UnsubscribeRequest>
{
	public async Task<GrpcResult> Handle(
		UnsubscribeRequest request,
		CancellationToken cancellationToken)
	{
		var subscriber = await appDbContext.UserProfiles
			                 .Include(x => x.SubscribedTo)
			                 .FirstOrDefaultAsync(x => x.Id == request.SubscriberId, cancellationToken)
		                 ?? throw new ApplicationException("Пользоваетль не найден");
		var unsubscribedTo = await appDbContext.UserProfiles
			.FirstOrDefaultAsync(x => x.Id == request.IdOfProfileUnsubscribedTo, cancellationToken);

		if (unsubscribedTo is null) return StatusCodes.Status404NotFound;

		if (subscriber.SubscribedTo.All(x => x.Id != request.IdOfProfileUnsubscribedTo))
			return StatusCodes.Status204NoContent;

		subscriber.SubscribedTo.Remove(unsubscribedTo);
		await appDbContext.SaveChangesAsync(cancellationToken);

		return StatusCodes.Status204NoContent;
	}
}