using System.Globalization;

using api.AppInfrastructure;
using api.AppInfrastructure.Middleware;
using api.Features.BackgroundServices.DisableConcurrentJobExecution;
using api.Features.BackgroundServices.LogCleanup;
using api.Features.BackgroundServices.ProjectMaster;
using api.Features.BackgroundServices.ProjectRecalculation;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Logging;

var cultureInfo = new CultureInfo("en-US");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
DcdEnvironments.CurrentEnvironment = builder.Configuration["AppConfiguration:Environment"] ?? "CI";
Console.WriteLine($"Loading config for: {DcdEnvironments.CurrentEnvironment}");

await DcdTypescriptGenerator.GenerateTypescriptFiles();

builder.AddDcdAzureAppConfiguration();
builder.ConfigureDcdDatabase();
builder.AddDcdFusionConfiguration();
builder.AddDcdAppInsights();
builder.ConfigureDcdLogging();
builder.AddDcdBlobStorage();

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

builder.Services.AddDcdIocConfiguration();

builder.Services.AddHostedService<DisableConcurrentJobExecutionService>();
builder.Services.AddHostedService<ProjectMasterBackgroundService>();
builder.Services.AddHostedService<ProjectRecalculationBackgroundService>();
builder.Services.AddHostedService<JobCleanupBackgroundService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseRouting();
app.UseResponseCompression();
app.UseMiddleware<DcdResponseTimerMiddleware>();
app.UseMiddleware<DcdExceptionHandlingMiddleware>();

if (DcdEnvironments.EnableSwagger)
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Concept App"));
}

app.UseCors(DcdCorsPolicyConfiguration.AccessControlPolicyName);
app.UseAuthentication();
app.UseMiddleware<DcdClaimsMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<DcdRequestLogMiddleware>();

app.Run();
