using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Posts.Data.Models;

namespace Posts.Data.Configuration;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
	public void Configure(EntityTypeBuilder<Photo> builder)
	{
		builder
			.HasIndex(x => x.PostId);
	}
}