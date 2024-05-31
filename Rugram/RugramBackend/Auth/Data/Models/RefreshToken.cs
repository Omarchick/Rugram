namespace Auth.Data.Models;

public class RefreshToken
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public required string Value { get; init; }
	public required DateTime ValidTo { get; init; }

	#region Navigation

	public User? User { get; set; }
	public required Guid UserId { get; init; }

	#endregion
}