using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.GetProfileName;

public class GetProfileNameRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetProfileNameRequest, GetProfileNameResponse>
{
	public async Task<GrpcResult<GetProfileNameResponse>> Handle(
		GetProfileNameRequest request,
		CancellationToken cancellationToken)
	{
		var profileName = await appDbContext.UserProfiles
			.Where(x => x.Id == request.ProfileId)
			.Select(x => x.ProfileName)
			.FirstOrDefaultAsync(cancellationToken);

		if (profileName is null) return StatusCodes.Status404NotFound;

		return new GetProfileNameResponse(profileName);
	}
}