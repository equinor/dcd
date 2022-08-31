using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Linq;

using api.Dtos;

using Microsoft.AspNetCore.Authentication;

namespace api.Services
{
    public class GraphRestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private const string SiteId = "46439b79-0ac8-4b2c-86ae-0cd94983323c";
        private const string ListId = "CD2589D1-3E47-41E1-848C-FE1ADB3AA459";
        private const string driveId = "911990B3-C0F9-48D8-A50B-C218DA049BF7";
        private const string prospFolder = "f05c5890-4c71-4872-8b7d-aa4bd7826418";
        private const string BaseUrl = "https://statoilsrm.sharepoint.com/sites/Team-IAF";


        public GraphRestService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }



        private GraphServiceClient GetGraphClient()
        {
            var scopes = new[] { "Sites.Read.All" };
            var token = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString()?.Split(' ')?.LastOrDefault();

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

        public List<DriveItemDto> GetFilesFromSite()
        {
            var graphClient = GetGraphClient();
            var driveItemSearchCollectionPage = graphClient.Sites[SiteId].Drive.Root.Search("prosp").Request().GetAsync().GetAwaiter()
                .GetResult();

            var dto = new List<DriveItemDto>();

            foreach (var driveItem in driveItemSearchCollectionPage )
            {
                var item = new DriveItemDto()
                {
                    Name = driveItem.Name,
                    Id = driveItem.Id,
                    CreatedBy = driveItem.CreatedBy,
                    Content = driveItem.Content,
                    CreatedDateTime = driveItem.CreatedDateTime,
                    Size = driveItem.Size,
                    SharepointIds = driveItem.SharepointIds,
                    LastModifiedBy = driveItem.LastModifiedBy,
                    LastModifiedDateTime = driveItem.LastModifiedDateTime,
                };
                dto.Add(item);
            }

            return dto;
        }

        public Stream? ImportSharepointFile(string id)
        {
            var graphClient = GetGraphClient();
            var file = graphClient.Sites[SiteId].Drive.Items[id].Content.Request()
                .GetAsync().GetAwaiter().GetResult();

            if (file != null)
            {
                return file;
            }

            return null;
        }

    }
}
