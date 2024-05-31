using Auth.Data;
using Auth.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Services.BackgroundServices;

public class DeleteOutdatedMailConfirmationTokens(IServiceProvider serviceProvider) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		const string command = $"DELETE FROM \"{nameof(AppDbContext.MailConfirmationTokens)}\" AS t " +
		                       $"WHERE t.\"{nameof(MailConfirmationToken.ValidTo)}\" < now()";

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