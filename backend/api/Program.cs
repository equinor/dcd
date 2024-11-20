using System.Globalization;

using api.Authorization;
using api.Mappings;
using api.Middleware;
using api.Services;
using api.StartupConfiguration;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Logging;

using Serilog;

var cultureInfo = new CultureInfo("en-US");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Configuration.GetSection("AppConfiguration").GetValue<string>("Environment");

Console.WriteLine($"Loading config for: {environment}");

DcdEnvironments.CurrentEnvironment = environment!;
var config = builder.CreateDcdConfigurationRoot(environment);
builder.Configuration.AddConfiguration(config);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.AddDcdAuthentication();
builder.ConfigureDcdDatabase(environment, config);
builder.Services.AddDcdCorsPolicy();
builder.Services.AddDcdFusionConfiguration(environment, config);
DcdLogConfiguration.ConfigureDcdLogging(environment, config);
builder.Services.AddDcdAppInsights(config);
builder.AddDcdAzureAppConfiguration();
builder.Services.AddDcdIocConfiguration();
builder.Services.AddHostedService<RefreshProjectService>();
builder.Services.AddMemoryCache();
builder.Services.Configure<IConfiguration>(builder.Configuration);
builder.Services.AddAutoMapper(typeof(CaseProfile));
builder.Services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new DcdApiEndpointTransformer())));
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureDcdSwagger();
builder.AddDcdBlogStorage();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Concept App"));
}

app.UseCors(DcdCorsPolicyConfiguration.AccessControlPolicyName);
app.UseAuthentication();
app.UseMiddleware<ClaimsMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
