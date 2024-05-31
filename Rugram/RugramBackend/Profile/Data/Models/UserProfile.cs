namespace Profile.Data.Models;

public class UserProfile(Guid id, string profileName)
{
	public Guid Id { get; init; } = id;
	public string ProfileName { get; init; } = profileName;
	public DateTime DateOfCreation { get; init; } = DateTime.UtcNow;

	#region Navigation

	public List<UserProfile> Subscribers { get; init; } = null!;
	public List<UserProfile> SubscribedTo { get; init; } = null!;

	#endregion
}