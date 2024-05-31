namespace Auth.Data.Models;

public class MailConfirmationToken
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public required string Email { get; init; }
	public required string Value { get; init; }
	public required DateTime ValidTo { get; init; }
}