using Auth.Data;
using Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services.BackgroundServices;

public class DeleteOutdatedRefreshTokens(IServiceProvider serviceProvider) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		const string command = $"DELETE FROM \"{nameof(AppDbContext.RefreshTokens)}\" AS t " +
		                       $"WHERE t.\"{nameof(RefreshToken.ValidTo)}\" < now()";

		while (!cancellationToken.IsCancellationRequested)
		{
			await using var scope = serviceProvider.CreateAsyncScope();
			await using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			await dbContext.Database.ExecuteSqlRawAsync(
				command,
				cancellationToken: cancellationToken);

			await Task.Delay(1000 * 60 * 60, cancellationToken);
		}
	}
}