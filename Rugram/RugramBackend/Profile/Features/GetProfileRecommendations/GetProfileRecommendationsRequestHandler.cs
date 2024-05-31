using Infrastructure.MediatR.Contracts;
using Microsoft.EntityFrameworkCore;
using Profile.Data;

namespace Profile.Features.GetProfileRecommendations;

public class GetProfileRecommendationsRequestHandler(AppDbContext appDbContext)
	: IGrpcRequestHandler<GetProfileRecommendationsRequest, GetProfileRecommendationsResponse>
{
	public async Task<GrpcResult<GetProfileRecommendationsResponse>> Handle(
		GetProfileRecommendationsRequest request,
		CancellationToken cancellationToken)
	{
		var profilesQuery = appDbContext.UserProfiles
			.Where(x =>
				x.SubscribedTo
					.Select(profile => profile.Id)
					.Contains(request.ProfileId)
				&& !x.Subscribers
					.Select(profile => profile.Id)
					.Contains(request.ProfileId))
			.Where(x =>
				request.SearchString == string.Empty
				|| x.ProfileName
					.ToLower()
					.Contains(request.SearchString
						.ToLower()));

		if (request.SearchString != string.Empty)
			profilesQuery = profilesQuery
				.OrderByDescending(x => x.ProfileName
					.ToLower()
					.StartsWith(request.SearchString
						.ToLower()))
				.ThenBy(x => x.Id);
		else
			profilesQuery = profilesQuery
				.OrderBy(x => x.Id);

		var profiles = await profilesQuery
			.Skip(request.PageNumber * request.PageSize)
			.Take(request.PageSize)
			.Select(x => new ProfileDto(x.Id, x.ProfileName))
			.ToListAsync(cancellationToken);

		var count = await profilesQuery.CountAsync(cancellationToken);

		if (profiles.Count < request.PageSize)
		{
			profilesQuery = appDbContext.UserProfiles
				.Where(x =>
					!x.SubscribedTo
						.Select(profile => profile.Id)
						.Contains(request.ProfileId)
					&& !x.Subscribers
						.Select(profile => profile.Id)
						.Contains(request.ProfileId))
				.Where(x =>
					request.SearchString == string.Empty
					|| x.ProfileName
						.ToLower()
						.Contains(request.SearchString
							.ToLower()));

			if (request.SearchString != string.Empty)
				profilesQuery = profilesQuery
					.OrderByDescending(x => x.ProfileName
						.ToLower()
						.StartsWith(request.SearchString
							.ToLower()))
					.ThenBy(x => x.Id);
			else
				profilesQuery = profilesQuery
					.OrderBy(x => x.Id);

			profiles.AddRange(
				await profilesQuery
					.Skip(Math.Max(request.PageNumber * request.PageSize - count, 0))
					.Take(request.PageSize - profiles.Count)
					.Select(x => new ProfileDto(x.Id, x.ProfileName))
					.ToListAsync(cancellationToken));
		}

		if (profiles.Count < request.PageSize)
		{
			count += await profilesQuery.CountAsync(cancellationToken);

			profilesQuery = appDbContext.UserProfiles
				.Where(x =>
					x.Subscribers
						.Select(profile => profile.Id)
						.Contains(request.ProfileId))
				.Where(x =>
					request.SearchString == string.Empty
					|| x.ProfileName
						.ToLower()
						.Contains(request.SearchString
							.ToLower()));

			if (request.SearchString != string.Empty)
				profilesQuery = profilesQuery
					.OrderByDescending(x => x.ProfileName
						.ToLower()
						.StartsWith(request.SearchString
							.ToLower()))
					.ThenBy(x => x.Id);
			else
				profilesQuery = profilesQuery
					.OrderBy(x => x.Id);

			profiles.AddRange(
				await profilesQuery
					.Skip(Math.Max(request.PageNumber * request.PageSize - count, 0))
					.Take(request.PageSize - profiles.Count)
					.Select(x => new ProfileDto(x.Id, x.ProfileName))
					.ToListAsync(cancellationToken));
		}

		return new GetProfileRecommendationsResponse(profiles.ToArray());
	}
}