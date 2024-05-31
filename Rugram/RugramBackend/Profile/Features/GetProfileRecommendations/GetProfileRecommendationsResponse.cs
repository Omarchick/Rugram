namespace Profile.Features.GetProfileRecommendations;

public record GetProfileRecommendationsResponse(ProfileDto[] Profiles);

public record ProfileDto(Guid Id, string ProfileName);