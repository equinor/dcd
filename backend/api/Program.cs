using api.Context;
using api.SampleData.Generators;
using api.Services;

using Api.Authorization;
using Api.Services.FusionIntegration;

using Azure.Identity;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;

var configBuilder = new ConfigurationBuilder();
var builder = WebApplication.CreateBuilder(args);
var azureAppConfigConnectionString =
    builder.Configuration.GetSection("AppConfiguration").GetValue<string>("ConnectionString");
var environment = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("Environment");

Console.WriteLine("Loading config for: " + environment);
configBuilder.AddAzureAppConfiguration(options =>
    options
        .Connect(azureAppConfigConnectionString)
        .ConfigureKeyVault(x => x.SetCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
        { ExcludeSharedTokenCacheCredential = true })))
        .Select(KeyFilter.Any)
        .Select(KeyFilter.Any, environment)
);
var config = configBuilder.Build();
builder.Configuration.AddConfiguration(config);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddMicrosoftGraph(builder.Configuration.GetSection("Graph"))
    .AddInMemoryTokenCaches();

var sqlConnectionString = config["Db:ConnectionString"] + "MultipleActiveResultSets=True;";

// Setup in memory DB SQL lite for test purposes
var _sqlConnectionString = builder.Configuration.GetSection("Database").GetValue<string>("ConnectionString");

if (string.IsNullOrEmpty(sqlConnectionString) || string.IsNullOrEmpty(_sqlConnectionString))
{
    if (environment == "localdev")
    {
        DbContextOptionsBuilder<DcdDbContext> dBbuilder = new();
        _sqlConnectionString = new SqliteConnectionStringBuilder
        { DataSource = "file::memory:", Mode = SqliteOpenMode.ReadWriteCreate, Cache = SqliteCacheMode.Shared }
            .ToString();

        SqliteConnection _connectionToInMemorySqlite = new(_sqlConnectionString);
        _connectionToInMemorySqlite.Open();
        dBbuilder.UseSqlite(_connectionToInMemorySqlite);

        using DcdDbContext context = new(dBbuilder.Options);
        context.Database.EnsureCreated();
        SaveSampleDataToDB.PopulateDb(context);
    }
    else
    {
        DbContextOptionsBuilder<DcdDbContext> dbBuilder = new();
        dbBuilder.UseSqlServer(sqlConnectionString);
        using DcdDbContext context = new(dbBuilder.Options);
        context.Database.EnsureCreated();
    }
}

// Set up CORS
var _accessControlPolicyName = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(_accessControlPolicyName,
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.WithExposedHeaders("Location");
            builder.WithOrigins(
                "http://localhost:3000",
                "https://fusion.equinor.com",
                "https://pro-s-portal-ci.azurewebsites.net",
                "https://pro-s-portal-fqa.azurewebsites.net",
                "https://pro-s-portal-fprd.azurewebsites.net"
            ).SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

var appInsightTelemetryOptions = new ApplicationInsightsServiceOptions
{
    InstrumentationKey = config["ApplicationInsightInstrumentationKey"]
};

if (environment == "localdev")
{
    builder.Services.AddDbContext<DcdDbContext>(options =>
            options.UseSqlite(_sqlConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));
}
else
{
    builder.Services.AddDbContext<DcdDbContext>(options => options.UseSqlServer(sqlConnectionString));
}

builder.Services.AddFusionIntegration(options =>
{
    var fusionEnvironment = environment switch
    {
        "dev" => "CI",
        "qa" => "FQA",
        "prod" => "FPRD",
        "radix-prod" => "FPRD",
        "radix-qa" => "FQA",
        "radix-dev" => "CI",
        _ => "CI"
    };

    Console.WriteLine("Fusion environment: " + fusionEnvironment);
    options.UseServiceInformation("ConceptApp", fusionEnvironment);

    options.AddFusionAuthorization();

    options.UseDefaultEndpointResolver(fusionEnvironment);
    options.UseDefaultTokenProvider(opts =>
    {
        opts.ClientId = config["AzureAd:ClientId"];
        opts.ClientSecret = config["AzureAd:ClientSecret"];
    });
    options.AddFusionRoles();
    options.ApplicationMode = true;
});

builder.Services.AddApplicationInsightsTelemetry(appInsightTelemetryOptions);
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<FusionService>();
builder.Services.AddScoped<DrainageStrategyService>();
builder.Services.AddScoped<WellProjectService>();
builder.Services.AddScoped<ExplorationService>();
builder.Services.AddScoped<SurfService>();
builder.Services.AddScoped<SubstructureService>();
builder.Services.AddScoped<TopsideService>();
builder.Services.AddScoped<WellService>();
builder.Services.AddScoped<WellProjectWellService>();
builder.Services.AddScoped<ExplorationWellService>();
builder.Services.AddScoped<TransportService>();
builder.Services.AddScoped<CaseService>();
builder.Services.AddScoped<ExplorationOperationalWellCostsService>();
builder.Services.AddScoped<DevelopmentOperationalWellCostsService>();
builder.Services.AddScoped<GenerateOpexCostProfile>();
builder.Services.AddScoped<GenerateStudyCostProfile>();
builder.Services.AddScoped<GenerateGAndGAdminCostProfile>();
builder.Services.AddScoped<GenerateCessationCostProfile>();
builder.Services.AddScoped<STEAService>();
builder.Services.AddScoped<ProspExcelImportService>();
builder.Services.AddScoped<ProspSharepointImportService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, ApplicationRoleAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ApplicationRolePolicyProvider>();
builder.Services.Configure<IConfiguration>(builder.Configuration);

builder.Services.AddControllers(
    options => options.Conventions.Add(new RouteTokenTransformerConvention(new ApiEndpointTransformer()))
);
builder.Services.AddScoped<SurfService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseRouting();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Fom Program, running the host now");
if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(_accessControlPolicyName);
app.UseAuthentication();
app.UseMiddleware<ClaimsMiddelware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
