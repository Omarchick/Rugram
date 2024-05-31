namespace Posts.Features.GetPosts;

public record GetPostsResponse(IReadOnlyList<ProfilePostDto> Posts, int TotalPostsCount);

public record ProfilePostDto(Guid PostId, string Description, DateTime DateOfCreation, Guid[] PhotoIds);