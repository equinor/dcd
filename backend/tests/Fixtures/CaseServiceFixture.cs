using api.Context;
using api.SampleData.Generators;
using api.Services;

using api.Services.FusionIntegration;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace tests.Fixtures;

public class CaseServiceFixture : IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public ICaseService CaseService
    {
        get
        {
            return _serviceProvider.GetService<ICaseService>();
        }
    }

    public IProjectService ProjectService
    {
        get
        {
            return _serviceProvider.GetService<IProjectService>();
        }
    }

    public DcdDbContext DbContext
    {
        get
        {
            return _serviceProvider.GetService<DcdDbContext>();
        }
    }

    public CaseServiceFixture()
    {
        var services = new ServiceCollection();

        DbContextOptionsBuilder<DcdDbContext> dBbuilder = new();
        var _sqlConnectionString = new SqliteConnectionStringBuilder
        { DataSource = "file::memory:", Mode = SqliteOpenMode.ReadWriteCreate, Cache = SqliteCacheMode.Shared }
            .ToString();

        SqliteConnection _connectionToInMemorySqlite = new(_sqlConnectionString);
        _connectionToInMemorySqlite.Open();
        dBbuilder.UseSqlite(_connectionToInMemorySqlite);

        using DcdDbContext context = new(dBbuilder.Options);
        context.Database.EnsureCreated();
        SaveSampleDataToDB.PopulateDb(context);

        services.AddDbContext<DcdDbContext>(options =>
            options.UseSqlite(_sqlConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

        services.AddLogging(l => l.AddProvider(NullLoggerProvider.Instance));

        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IFusionService, FusionService>();
        services.AddScoped<IDrainageStrategyService, DrainageStrategyService>();
        services.AddScoped<IWellProjectService, WellProjectService>();
        services.AddScoped<IExplorationService, ExplorationService>();
        services.AddScoped<ISurfService, SurfService>();
        services.AddScoped<ISubstructureService, SubstructureService>();
        services.AddScoped<ITopsideService, TopsideService>();
        services.AddScoped<IWellService, WellService>();
        services.AddScoped<IWellProjectWellService, WellProjectWellService>();
        services.AddScoped<IExplorationWellService, ExplorationWellService>();
        services.AddScoped<ICostProfileFromDrillingScheduleHelper, CostProfileFromDrillingScheduleHelper>();
        services.AddScoped<ITransportService, TransportService>();
        services.AddScoped<ICaseService, CaseService>();
        services.AddScoped<IDuplicateCaseService, DuplicateCaseService>();
        services.AddScoped<IExplorationOperationalWellCostsService, ExplorationOperationalWellCostsService>();

        // build provider
        _serviceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
