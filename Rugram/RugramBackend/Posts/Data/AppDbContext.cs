using Microsoft.EntityFrameworkCore;
using Posts.Data.Models;

namespace Posts.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Post> Posts { get; set; } = null!;
	public DbSet<Photo> Photos { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}