using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posts.Data.Models;

namespace Posts.Data.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder
			.HasIndex(x => x.ProfileId);
		builder
			.HasIndex(x => x.Id);
		builder
			.Property(x => x.Description)
			.HasMaxLength(3000);
	}
}