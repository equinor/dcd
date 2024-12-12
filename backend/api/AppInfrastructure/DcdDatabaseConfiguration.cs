using api.Context;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure;

public static class DcdDatabaseConfiguration
{
    public static void ConfigureDcdDatabase(this WebApplicationBuilder builder, IConfigurationRoot config)
    {
        if (DcdEnvironments.IsLocal())
        {
            SetupSqliteDatabase(builder);

            return;
        }

        SetupAzureDatabase(builder, config);
    }

    private static void SetupSqliteDatabase(WebApplicationBuilder builder)
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<DcdDbContext>();

        var sqliteConnectionString = new SqliteConnectionStringBuilder
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

        builder.Services.AddDbContext<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlite(sqliteConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        builder.Services.AddDbContextFactory<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlite(sqliteConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            lifetime: ServiceLifetime.Scoped);
    }

    private static void SetupAzureDatabase(WebApplicationBuilder builder, IConfigurationRoot config)
    {
        var sqlServerConnectionString = config["Db:ConnectionString"] + "MultipleActiveResultSets=True;";

        var dbBuilder = new DbContextOptionsBuilder<DcdDbContext>();
        dbBuilder.UseSqlServer(sqlServerConnectionString);
        using var context = new DcdDbContext(dbBuilder.Options, null);

        builder.Services.AddDbContext<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlServer(sqlServerConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        builder.Services.AddDbContextFactory<DcdDbContext>(options => options.UseLazyLoadingProxies()
                .UseSqlServer(sqlServerConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            lifetime: ServiceLifetime.Scoped);
    }
}
