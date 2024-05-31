using System.Reflection;
using Auth.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Extensions;

public static class DbConfiguratorAndMigrator
{
	/// <summary>
	///     Создание и настройка подключения к бд
	/// </summary>
	/// <param name="builder">WebApplicationBuilder</param>
	public static void ConfigurePostgresqlConnection(this WebApplicationBuilder builder)
	{
		builder.Services.AddDbContext<AppDbContext>(
			options =>
			{
				options.UseNpgsql(
					builder.Configuration.GetConnectionString("PostreSQL"),
					opt => { opt.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name); });
			});
	}

	/// <summary>
	///     Миграция бд
	/// </summary>
	/// <param name="host">хост</param>
	public static async Task MigrateDbAsync(this WebApplication host)
	{
		try
		{
			await using var scope = host.Services.CreateAsyncScope();
			var sp = scope.ServiceProvider;

			await using var db = sp.GetRequiredService<AppDbContext>();

			await db.Database.MigrateAsync();
		}
		catch (Exception e)
		{
			host.Logger.LogError(e, "Error while migrating the database");
			Environment.Exit(-1);
		}
	}
}