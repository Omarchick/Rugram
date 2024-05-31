namespace Posts.Features.GetPost;

public record GetPostResponse(
	Guid ProfileId,
	DateTime DateOfCreation,
	string Description,
	byte[][] Photos);