using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profile.Data.Models;

namespace Profile.Data.Configuration;

public class ProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
	public void Configure(EntityTypeBuilder<UserProfile> builder)
	{
		builder
			.HasMany(x => x.Subscribers)
			.WithMany(x => x.SubscribedTo);
		builder
			.Property(x => x.ProfileName)
			.HasMaxLength(25);
		builder
			.HasIndex(x => x.ProfileName)
			.IsUnique();
	}
}