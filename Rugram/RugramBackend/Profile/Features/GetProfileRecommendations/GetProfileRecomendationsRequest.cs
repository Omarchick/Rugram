using Infrastructure.MediatR.Contracts;

namespace Profile.Features.GetProfileRecommendations;

public record GetProfileRecommendationsRequest(Guid ProfileId, string SearchString, int PageSize, int PageNumber)
	: IGrpcRequest<GetProfileRecommendationsResponse>;