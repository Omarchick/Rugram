namespace Posts.Data.Models;

public class Photo(Guid postId)
{
	public Guid Id { get; init; } = Guid.NewGuid();

	#region Navigation

	public Post? Post { get; init; }
	public Guid PostId { get; init; } = postId;

	#endregion
}