using api.Context;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace api.StartupConfiguration;

public static class DcdDatabaseConfiguration
{
    public static void ConfigureDcdDatabase(this WebApplicationBuilder builder, string? environment, IConfigurationRoot config)
    {
        var sqlServerConnectionString = config["Db:ConnectionString"] + "MultipleActiveResultSets=True;";
        var sqliteConnectionString = builder.Configuration.GetSection("Database").GetValue<string>("ConnectionString");

        if (string.IsNullOrEmpty(sqlServerConnectionString) || string.IsNullOrEmpty(sqliteConnectionString))
        {
            if (environment == "localdev")
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<DcdDbContext>();
                sqliteConnectionString = new SqliteConnectionStringBuilder
                    {
                        DataSource = "file::memory:",
                        Mode = SqliteOpenMode.ReadWriteCreate,
                        Cache = SqliteCacheMode.Shared
                    }
                    .ToString();

                var sqliteConnection = new SqliteConnection(sqliteConnectionString);
                sqliteConnection.Open();
                dbContextOptionsBuilder.UseSqlite(sqliteConnection);

                using var context = new DcdDbContext(dbContextOptionsBuilder.Options, null);
                context.Database.EnsureCreated();
            }
            else
            {
                var dbBuilder = new DbContextOptionsBuilder<DcdDbContext>();
                dbBuilder.UseSqlServer(sqlServerConnectionString);
                using var context = new DcdDbContext(dbBuilder.Options, null);
            }
        }

        if (environment == "localdev")
        {
            builder.Services.AddDbContext<DcdDbContext>(
                options => options.UseLazyLoadingProxies()
                    .UseSqlite(sqliteConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }
        else
        {
            builder.Services.AddDbContext<DcdDbContext>(
                options => options.UseLazyLoadingProxies()
                    .UseSqlServer(sqlServerConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }
    }
}
