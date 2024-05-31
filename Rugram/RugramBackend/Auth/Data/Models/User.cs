namespace Auth.Data.Models;

public class User
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public required string Email { get; init; }
	public required Role Role { get; init; }
	public required string Password { get; set; }

	#region Navigation

	public List<RefreshToken> RefreshTokens { get; init; } = null!;

	#endregion
}

public enum Role
{
	User = 1,
	Admin = 2
}