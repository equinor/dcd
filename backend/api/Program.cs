using api.Authorization;
using api.Context;
using api.Helpers;
using api.Mappings;
using api.Repositories;
using api.SampleData.Generators;
using api.Services;
using api.Services.FusionIntegration;
using api.Services.GenerateCostProfiles;

using Azure.Identity;
using Azure.Storage.Blobs;

using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

using Serilog;

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
                "https://pro-s-portal-fprd.azurewebsites.net",
                "https://fusion-s-portal-ci.azurewebsites.net",
                "https://fusion-s-portal-fqa.azurewebsites.net",
                "https://fusion-s-portal-fprd.azurewebsites.net",
                "https://pr-3422.fusion-dev.net",
                "https://pr-*.fusion-dev.net"
            ).SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

var appInsightTelemetryOptions = new ApplicationInsightsServiceOptions
{
    ConnectionString = config["ApplicationInsightInstrumentationConnectionString"],
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
        _ => "CI",
    };

    Console.WriteLine("Fusion environment: " + fusionEnvironment);
    options.UseServiceInformation("ConceptApp", fusionEnvironment);

    options.UseDefaultEndpointResolver(fusionEnvironment);
    options.UseDefaultTokenProvider(opts =>
    {
        opts.ClientId = config["AzureAd:ClientId"];
        opts.ClientSecret = config["AzureAd:ClientSecret"];
    });
    options.AddFusionRoles();
    options.ApplicationMode = true;
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .ReadFrom.Configuration(config)
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Environment", environment ?? "localdev")
    .Enrich.FromLogContext()
    .CreateBootstrapLogger();
builder.Services.AddApplicationInsightsTelemetry(appInsightTelemetryOptions);

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IFusionService, FusionService>();
builder.Services.AddScoped<IDrainageStrategyService, DrainageStrategyService>();
builder.Services.AddScoped<IWellProjectService, WellProjectService>();
builder.Services.AddScoped<IExplorationService, ExplorationService>();
builder.Services.AddScoped<ISurfService, SurfService>();
builder.Services.AddScoped<ISubstructureService, SubstructureService>();
builder.Services.AddScoped<ITopsideService, TopsideService>();
builder.Services.AddScoped<IWellService, WellService>();
builder.Services.AddScoped<IWellProjectWellService, WellProjectWellService>();
builder.Services.AddScoped<IExplorationWellService, ExplorationWellService>();
builder.Services.AddScoped<ICostProfileFromDrillingScheduleHelper, CostProfileFromDrillingScheduleHelper>();
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IDuplicateCaseService, DuplicateCaseService>();
builder.Services.AddScoped<IExplorationOperationalWellCostsService, ExplorationOperationalWellCostsService>();

builder.Services.AddScoped<IDevelopmentOperationalWellCostsService, DevelopmentOperationalWellCostsService>();
builder.Services.AddScoped<ICaseWithAssetsService, CaseWithAssetsService>();

builder.Services.AddScoped<ITechnicalInputService, TechnicalInputService>();
builder.Services.AddScoped<IGenerateOpexCostProfile, GenerateOpexCostProfile>();
builder.Services.AddScoped<IGenerateStudyCostProfile, GenerateStudyCostProfile>();
builder.Services.AddScoped<IGenerateCo2EmissionsProfile, GenerateCo2EmissionsProfile>();
builder.Services.AddScoped<IGenerateGAndGAdminCostProfile, GenerateGAndGAdminCostProfile>();
builder.Services.AddScoped<IGenerateCessationCostProfile, GenerateCessationCostProfile>();
builder.Services.AddScoped<IGenerateImportedElectricityProfile, GenerateImportedElectricityProfile>();
builder.Services.AddScoped<IGenerateFuelFlaringLossesProfile, GenerateFuelFlaringLossesProfile>();
builder.Services.AddScoped<IGenerateNetSaleGasProfile, GenerateNetSaleGasProfile>();
builder.Services.AddScoped<IGenerateCo2IntensityProfile, GenerateCo2IntensityProfile>();
builder.Services.AddScoped<IGenerateCo2IntensityTotal, GenerateCo2IntensityTotal>();
builder.Services.AddScoped<ICompareCasesService, CompareCasesService>();
builder.Services.AddScoped<IGenerateCo2DrillingFlaringFuelTotals, GenerateCo2DrillingFlaringFuelTotals>();
builder.Services.AddScoped<ISTEAService, STEAService>();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ISubstructureRepository, SubstructureRepository>();
builder.Services.AddScoped<ITopsideRepository, TopsideRepository>();
builder.Services.AddScoped<IDrainageStrategyRepository, DrainageStrategyRepository>();

builder.Services.AddScoped<IMapperService, MapperService>();
builder.Services.AddScoped<IConversionMapperService, ConversionMapperService>();

builder.Services.AddHostedService<RefreshProjectService>();
builder.Services.AddScoped<ProspExcelImportService>();
builder.Services.AddScoped<ProspSharepointImportService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, ApplicationRoleAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ApplicationRolePolicyProvider>();

builder.Services.Configure<IConfiguration>(builder.Configuration);

builder.Services.AddAutoMapper(typeof(CaseProfile));

builder.Services.AddControllers(
    options => options.Conventions.Add(new RouteTokenTransformerConvention(new ApiEndpointTransformer()))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Concept App",
        Version = "v1",
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));
builder.Services.AddSingleton<IBlobStorageService>(x =>
    new BlobStorageService(x.GetRequiredService<BlobServiceClient>(), "dcdimagestorage"));


builder.Host.UseSerilog();

var app = builder.Build();
app.UseRouting();
if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Concept App");
    });
}

app.UseCors(_accessControlPolicyName);
app.UseAuthentication();
app.UseMiddleware<ClaimsMiddelware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
