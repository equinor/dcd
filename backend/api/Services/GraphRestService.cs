using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Linq;

using Microsoft.AspNetCore.Authentication;

namespace api.Services
{
    public class GraphRestService
    {
        private readonly IHttpContextService _httpContextService;
        private const string SiteId = "46439b79-0ac8-4b2c-86ae-0cd94983323c";
        private const string BaseUrl = "https://statoilsrm.sharepoint.com/sites/Team-IAF";


        public GraphRestService(IHttpContextService httpContextService)
        {
            _httpContextService = httpContextService;
        }



        private GraphServiceClient GetGraphClient()
        {
            var scopes = new[] { "Sites.ReadWrite.All" };
            var token = _httpContextService.GetToken().GetAwaiter().GetResult();

            // Multi-tenant apps can use "common",
            // single-tenant apps must use the tenant ID from the Azure portal
            var tenantId = "3aa4a235-b6e2-48d5-9195-7fcf05b459b0";

            // Values from app registration
            var clientId = "9b125a0c-4907-43b9-8db2-ff405d6b0524";
            var clientSecret = "";


            var cca = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithTenantId(tenantId)
                .WithClientSecret(clientSecret)
                .Build();

            // DelegateAuthenticationProvider is a simple auth provider implementation
            // that allows you to define an async function to retrieve a token
            // Alternatively, you can create a class that implements IAuthenticationProvider
            // for more complex scenarios
            var authProvider = new DelegateAuthenticationProvider(async (request) => {
                // Use Microsoft.Identity.Client to retrieve token
                var assertion = new UserAssertion(token);
                var result = await cca.AcquireTokenOnBehalfOf(scopes, assertion).ExecuteAsync();

                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            });

            var graphClient = new GraphServiceClient(authProvider);
            return graphClient;
        }

        public List<Stream> GetAllFilesFromSite()
        {
            // var graphClient = new GraphServiceClient(BaseUrl, authenticationProvider);
            // var driveItem =  graphClient.Me.Drive.Root.ListItem.Request().GetAsync();
            var graphClient = GetGraphClient();
            var stream = graphClient.Sites[SiteId].Drive.Items
                .Request().GetAsync().GetAwaiter().GetResult();

            var files = (stream.Where(item => item != null).Select(item => item.Content)).ToList();
            // var files = (from item in stream where item != null && item.Name.Contains("prosp") select item.Content).ToList();

            // var queryOptions = new List<QueryOption>()
            // {
            //     new QueryOption("expand", "fields")
            // };
            //
            // var items = await graphClient.Sites[SiteId].Drive.Items
            //     .Request(queryOptions)
            //     .GetAsync();
            return files;

        }

    }
}
