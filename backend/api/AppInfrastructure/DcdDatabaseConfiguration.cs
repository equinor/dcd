using api.Context;
using api.Features.Profiles;

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

        foreach (var (tableName, profileName, isOverride, entityName, entityNamePlural) in MigrationQueries)
        {
            var countQuery = $"select count(*) as Value from sys.objects where object_id = object_id('z_backup_{tableName}')";

            var itemsFound = context.Database.SqlQueryRaw<int>(countQuery).First();

            if (itemsFound > 0)
            {
                continue;
            }

            var insertQuery = isOverride
                    ? $"""
                       insert into TimeSeriesProfiles
                       (
                           Id, StartYear, InternalData, CaseId, CreatedUtc, UpdatedUtc,
                           Override, ProfileType
                       )
                       select
                        p.Id, StartYear, InternalData, c.Id, getutcdate(), getutcdate(),
                        p.Override, '{profileName}'
                       from
                           {tableName} p
                           inner join {entityNamePlural} t on t.Id = p.[{entityName}.Id]
                           inner join Cases c on c.{entityName}Link = t.Id;
                       """
                    : $"""
                       insert into TimeSeriesProfiles
                       (
                           Id, StartYear, InternalData, CaseId, CreatedUtc, UpdatedUtc,
                           Override, ProfileType
                       )
                       select
                        p.Id, StartYear, InternalData, c.Id, getutcdate(), getutcdate(),
                        0, '{profileName}'
                       from
                           {tableName} p
                           inner join {entityNamePlural} t on t.Id = p.[{entityName}.Id]
                           inner join Cases c on c.{entityName}Link = t.Id;
                       """;

            context.Database.ExecuteSqlRaw(insertQuery);

            var backupQuery = $"""
                              if not exists (select * from sys.objects where object_id = object_id('z_backup_{tableName}'))
                              begin
                                  select * into dbo.z_backup_{tableName} from {tableName}
                              end
                              """;

            context.Database.ExecuteSqlRaw(backupQuery);
        }
    }

    private static readonly List<(string tableName, string profileName, bool isOverride, string entityName, string entityNamePlural)> MigrationQueries =
    [
        ("TopsideCostProfiles", ProfileTypes.TopsideCostProfile, false, "Topside", "Topsides"),
        ("TopsideCostProfileOverride", ProfileTypes.TopsideCostProfileOverride, true, "Topside", "Topsides"),
        ("TopsideCessationCostProfiles", ProfileTypes.TopsideCessationCostProfile, false, "Topside", "Topsides"),

        ("TransportCostProfile", ProfileTypes.TransportCostProfile, false, "Transport", "Transports"),
        ("TransportCostProfileOverride", ProfileTypes.TransportCostProfileOverride, true, "Transport", "Transports"),
        ("TransportCessationCostProfiles", ProfileTypes.TransportCessationCostProfile, false, "Transport", "Transports"),

        ("SurfCostProfile", ProfileTypes.SurfCostProfile, false, "Surf", "Surfs"),
        ("SurfCostProfileOverride", ProfileTypes.SurfCostProfileOverride, true, "Surf", "Surfs"),
        ("SurfCessationCostProfiles", ProfileTypes.SurfCessationCostProfile, false, "Surf", "Surfs"),

        ("SubstructureCostProfiles", ProfileTypes.SubstructureCostProfile, false, "Substructure", "Substructures"),
        ("SubstructureCostProfileOverride", ProfileTypes.SubstructureCostProfileOverride, true, "Substructure", "Substructures"),
        ("SubstructureCessationCostProfiles", ProfileTypes.SubstructureCessationCostProfile, false, "Substructure", "Substructures"),

        ("OnshorePowerSupplyCostProfile", ProfileTypes.OnshorePowerSupplyCostProfile, false, "OnshorePowerSupply", "OnshorePowerSupplies"),
        ("OnshorePowerSupplyCostProfileOverride", ProfileTypes.OnshorePowerSupplyCostProfileOverride, true, "OnshorePowerSupply", "OnshorePowerSupplies"),

        ("GAndGAdminCost", ProfileTypes.GAndGAdminCost, false, "Exploration", "Explorations"),
        ("GAndGAdminCostOverride", ProfileTypes.GAndGAdminCostOverride, true, "Exploration", "Explorations"),

        ("ExplorationWellCostProfile", ProfileTypes.ExplorationWellCostProfile, false, "Exploration", "Explorations"),
        ("AppraisalWellCostProfile", ProfileTypes.AppraisalWellCostProfile, false, "Exploration", "Explorations"),
        ("SidetrackCostProfile", ProfileTypes.SidetrackCostProfile, false, "Exploration", "Explorations"),
        ("SeismicAcquisitionAndProcessing", ProfileTypes.SeismicAcquisitionAndProcessing, false, "Exploration", "Explorations"),
        ("CountryOfficeCost", ProfileTypes.CountryOfficeCost, false, "Exploration", "Explorations"),
    ];
}
