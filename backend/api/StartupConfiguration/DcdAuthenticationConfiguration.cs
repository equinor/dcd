using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace api.StartupConfiguration;

public static class DcdAuthenticationConfiguration
{
    public static void AddDcdAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(builder.Configuration.GetSection("Graph"))
            .AddDownstreamApi("FusionPeople", builder.Configuration.GetSection("FusionPeople"))
            .AddInMemoryTokenCaches();
    }
}
