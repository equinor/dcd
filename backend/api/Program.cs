using System.Globalization;

using api.AppInfrastructure;
using api.AppInfrastructure.Authorization;
using api.AppInfrastructure.Middleware;
using api.Features.BackgroundServices.ProjectMaster;
using api.ModelMapping.AutoMapperProfiles;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Logging;

var cultureInfo = new CultureInfo("en-US");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
DcdEnvironments.CurrentEnvironment = builder.Configuration["AppConfiguration:Environment"] ?? "CI";
Console.WriteLine($"Loading config for: {DcdEnvironments.CurrentEnvironment}");

var azureConfig = builder.CreateDcdConfigurationRoot();

builder.Configuration.AddConfiguration(azureConfig);
builder.ConfigureDcdDatabase(azureConfig);
builder.Services.AddDcdFusionConfiguration(azureConfig, builder.Configuration);
builder.Services.AddDcdAppInsights(azureConfig);
builder.ConfigureDcdLogging(azureConfig);
builder.AddDcdBlobStorage(azureConfig);

builder.Services.AddDcdCorsPolicy();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new RouteTokenTransformerConvention(new DcdApiEndpointTransformer())));
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureDcdSwagger();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.AddDcdAuthentication();

builder.Services.AddAutoMapper(typeof(CaseProfile));
builder.Services.AddDcdIocConfiguration();

builder.Services.AddHostedService<ProjectMasterBackgroundService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseMiddleware<DcdResponseTimerMiddleware>();
app.UseMiddleware<DcdExceptionHandlingMiddleware>();
app.UseRouting();
app.UseResponseCompression();

if (DcdEnvironments.IsLocal() || DcdEnvironments.IsCi())
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
