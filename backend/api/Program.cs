using System.Text.Json.Serialization;

using api.Context;
using api.SampleData.Generators;
using api.Services;

using Azure.Identity;

using Equinor.TI.CommonLibrary.Client;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;

var configBuilder = new ConfigurationBuilder();
var builder = WebApplication.CreateBuilder(args);
var azureAppConfigConnectionString = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("ConnectionString");
var environment = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("Environment");
Console.WriteLine("Loding config for: " + environment);

configBuilder.AddAzureAppConfiguration(options =>
    options
    .Connect(azureAppConfigConnectionString)
    .ConfigureKeyVault(x =>
    {
        x.SetCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeSharedTokenCacheCredential = true }));
    })
    .Select(KeyFilter.Any, LabelFilter.Null)
    .Select(KeyFilter.Any, environment)
);
var config = configBuilder.Build();

string commonLibTokenConnection = CommonLibraryService.BuildTokenConnectionString(
                config["AzureAd:ClientId"],
                config["AzureAd:TenantId"],
                config["AzureAd:ClientSecret"]);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
// Setup in memory DB SQL lite for test purposes
string _sqlConnectionString = builder.Configuration.GetSection("Database").GetValue<string>("ConnectionString");
if (string.IsNullOrEmpty(_sqlConnectionString))
{
    DbContextOptionsBuilder<DcdDbContext> dBbuilder = new DbContextOptionsBuilder<DcdDbContext>();
    _sqlConnectionString = new SqliteConnectionStringBuilder { DataSource = "file::memory:", Mode = SqliteOpenMode.ReadWriteCreate, Cache = SqliteCacheMode.Shared }.ToString();

    // In-memory sqlite requires an open connection throughout the whole lifetime of the database
    SqliteConnection _connectionToInMemorySqlite = new SqliteConnection(_sqlConnectionString);
    _connectionToInMemorySqlite.Open();
    dBbuilder.UseSqlite(_connectionToInMemorySqlite);

    using (DcdDbContext context = new DcdDbContext(dBbuilder.Options))
    {
        context.Database.EnsureCreated();
        SaveSampleDataToDB.PopulateDb(context);
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
            builder.WithOrigins(
                "http://localhost:3000",
                "https://*.equinor.com",
                "https://ase-dcd-frontend-dev.azurewebsites.net/",
                "https://ase-dcd-frontend-qa.azurewebsites.net/"
            ).SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

// Setting splitting behavior explicitly to avoid warning
builder.Services.AddDbContext<DcdDbContext>(
    options => options.UseSqlite(_sqlConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
);
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<DrainageStrategyService>();
builder.Services.AddScoped<WellProjectService>();
builder.Services.AddScoped<ExplorationService>();
builder.Services.AddScoped<SurfService>();
builder.Services.AddScoped<SubstructureService>();
builder.Services.AddScoped<TopsideService>();
builder.Services.AddScoped<TransportService>();
builder.Services.AddScoped<CaseService>();
builder.Services.AddScoped<CommonLibraryClientOptions>(_ => new CommonLibraryClientOptions { TokenProviderConnectionString = commonLibTokenConnection });
builder.Services.AddScoped<CommonLibraryService>();
builder.Services.AddScoped<STEAService>();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ApiEndpointTransformer()));
}

);
builder.Services.AddScoped<SurfService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(_accessControlPolicyName);

app.MapControllers();

app.Run();

public partial class Program { }
