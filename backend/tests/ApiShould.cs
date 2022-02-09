using System.Security.Claims;
using System.Text.Encodings.Web;

using api.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;

using Xunit;

namespace inttests;

public class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DummyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions>
        options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock
        clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}

public class ApiShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/projects")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(
                            "Test", options => { });
                    });
            }).CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
        var responseProjects = await response.Content.ReadFromJsonAsync<List<Project>>();
        foreach (var responseProject in responseProjects)
        {
            Assert.NotNull(responseProject.DrainageStrategies);
        }
    }
}
