using System.Text.Json.Serialization;

using api.Context;
using api.SampleData;
using api.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var configBuilder = new ConfigurationBuilder();
var builder = WebApplication.CreateBuilder(args);
var azureAppConfigConnectionString = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("ConnectionString");
configBuilder.AddAzureAppConfiguration(azureAppConfigConnectionString);
var config = configBuilder.Build();

//POC For Azure App Config
Console.WriteLine(config["PoC2"] ?? "Hello world!");

// Dev-Note Add this configuration to prevent circular references in the database
// https://stackoverflow.com/questions/60197270/jsonexception-a-possible-object-cycle-was-detected-which-is-not-supported-this
builder.Services.AddMvc()
 .AddJsonOptions(o =>
 {
     o.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles;
 });
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

// Setting splitting behavior explicitly to avoid warning
builder.Services.AddDbContext<DcdDbContext>(
    options => options.UseSqlite(_sqlConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery))
);
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<DrainageStrategyService>();
builder.Services.AddScoped<WellProjectService>();
builder.Services.AddScoped<FacilityService>();
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

app.MapControllers();

app.Run();
