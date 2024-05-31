namespace Profile.Features.GetFeed;

public record GetFeedResponse(FeedPostDto[] FeedPostDto);

public record FeedPostDto(
	Guid ProfileId,
	Guid PostId,
	string ProfileName,
	string Description,
	DateTime DateOfCreation,
	Guid[] PhotoIds);