namespace Posts.Features.GetFeed;

public record GetFeedResponse(FeedPostDto[] FeedPostDto);

public record FeedPostDto(
	Guid ProfileId,
	Guid PostId,
	string Description,
	DateTime DateOfCreation,
	Guid[] PhotoIds);