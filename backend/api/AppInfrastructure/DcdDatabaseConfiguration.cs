using api.Context;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace api.AppInfrastructure;

public static class DcdDatabaseConfiguration
{
    public static void ConfigureDcdDatabase(this WebApplicationBuilder builder)
    {
        if (DcdEnvironments.UseSqlite)
        {
            SetupSqliteDatabase(builder);

            return;
        }

        SetupAzureDatabase(builder);
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

        using var context = new DcdDbContext(dbContextOptionsBuilder.Options, null, null);
        context.Database.EnsureCreated();

        builder.Services.AddDbContext<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlite(sqliteConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        builder.Services.AddDbContextFactory<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlite(sqliteConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            lifetime: ServiceLifetime.Scoped);
    }

    private static void SetupAzureDatabase(WebApplicationBuilder builder)
    {
        var sqlServerConnectionString = builder.Configuration["Db:ConnectionString"]!;

        builder.Services.AddDbContext<DcdDbContext>(
            options => options.UseLazyLoadingProxies()
                .UseSqlServer(sqlServerConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        builder.Services.AddDbContextFactory<DcdDbContext>(options => options.UseLazyLoadingProxies()
                .UseSqlServer(sqlServerConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)),
            lifetime: ServiceLifetime.Scoped);

        if (!DcdEnvironments.AllowMigrationsToBeApplied)
        {
            return;
        }

        var dbBuilder = new DbContextOptionsBuilder<DcdDbContext>();
        dbBuilder.UseSqlServer(sqlServerConnectionString);
        using var context = new DcdDbContext(dbBuilder.Options, null, null);

        context.Database.Migrate();

        MigrateProfileData(dbBuilder.Options);
    }

    private static void MigrateProfileData(DbContextOptions<DcdDbContext> dbBuilderOptions)
    {
        using var context = new DcdDbContext(dbBuilderOptions, null, null);

        foreach (var migrationQuery in MigrationQueries)
        {
            var type = migrationQuery.Key;
            var countQuery = $"select count(*) as Value from TimeSeriesProfiles where ProfileType = '{type}'";

            var itemsFound = context.Database.SqlQueryRaw<int>(countQuery).First();

            if (itemsFound != 0)
            {
                continue;
            }

            var insertQuery = migrationQuery.Value
                    ? $"""
                       insert into TimeSeriesProfiles
                       (
                           Id, StartYear, InternalData, CaseId, CreatedUtc, UpdatedUtc,
                           Override, ProfileType
                       )
                       select
                           Id, StartYear, InternalData, [Case.Id], getutcdate(), getutcdate(),
                       Override, '{type}'
                       from
                           {type};
                       """
                    : $"""
                       insert into TimeSeriesProfiles
                       (
                           Id, StartYear, InternalData, CaseId, CreatedUtc, UpdatedUtc,
                           Override, ProfileType
                       )
                       select
                           Id, StartYear, InternalData, [Case.Id], getutcdate(), getutcdate(),
                       0, '{type}'
                       from {type};
                       """;

            context.Database.ExecuteSqlRaw(insertQuery);
        }
    }

    private static readonly Dictionary<string, bool> MigrationQueries = new()
    {
        { "CessationWellsCost", false },
        { "CessationWellsCostOverride", true },
        { "CessationOffshoreFacilitiesCost", false },
        { "CessationOffshoreFacilitiesCostOverride", true },
        { "WellInterventionCostProfile", false },
        { "WellInterventionCostProfileOverride", true },
        { "OffshoreFacilitiesOperationsCostProfile", false },
        { "OffshoreFacilitiesOperationsCostProfileOverride", true },
        { "TotalFeasibilityAndConceptStudies", false },
        { "TotalFeasibilityAndConceptStudiesOverride", true },
    };
}
