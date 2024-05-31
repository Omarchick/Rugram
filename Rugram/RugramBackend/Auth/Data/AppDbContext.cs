using Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<User> Users { get; init; } = null!;
	public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
	public DbSet<MailConfirmationToken> MailConfirmationTokens { get; init; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}