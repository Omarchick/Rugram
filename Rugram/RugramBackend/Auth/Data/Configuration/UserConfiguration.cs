using Auth.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder
			.HasIndex(user => user.Email)
			.IsUnique();
		builder
			.Property(user => user.Password)
			.HasMaxLength(25);
	}
}