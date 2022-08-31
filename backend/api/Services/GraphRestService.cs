using Azure.Identity;

using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Linq;

using api.Dtos;

using DocumentFormat.OpenXml.Office.CustomUI;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class GraphRestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public GraphRestService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        private string GetSiteId() => _config["SharePoint:Prosp:SiteId"];

        private GraphServiceClient GetGraphClient()
        {
            var tenantId = _config["AzureAd:TenantId"];
            var clientId = _config["AzureAd:ClientId"];
            var clientSecret = _config["AzureAd:ClientSecret"];
            var scopes = new[] { "Sites.Read.All" };

            var token = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"]
                .ToString()
                ?.Split(" ")
                ?.LastOrDefault();

            var cca = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithTenantId(tenantId)
                .WithClientSecret(clientSecret)
                .Build();

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
            var siteId = GetSiteId();
            var dto = new List<DriveItemDto>();
            var query = _config["SharePoint:Prosp:FileQuery"];
            var validMimeTypes = new List<string>
            {
                ExcelMimeTypes.XLS,
                ExcelMimeTypes.XLSB,
                ExcelMimeTypes.XLSM,
                ExcelMimeTypes.XLSX
            };

            var driveItemSearchCollectionPage = graphClient.Sites[siteId]
                .Drive.Root.Search(query)
                .Request()
                .GetAsync()
                .GetAwaiter()
                .GetResult();

            foreach (var driveItem in driveItemSearchCollectionPage.Where(item =>
                         item.File != null && validMimeTypes.Contains(item.File.MimeType)))
            {
                ConvertToDto(driveItem, dto);
            }

            return dto;
        }

        private static void ConvertToDto(DriveItem driveItem, List<DriveItemDto> dto)
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

        public Stream GetSharepointFileStream(string id)
        {
            var graphClient = GetGraphClient();
            var siteId = GetSiteId();
            var driveItemStream = graphClient.Sites[siteId]
                .Drive.Items[id]
                .Content.Request()
                .GetAsync()
                .GetAwaiter()
                .GetResult();

            return driveItemStream;
        }

    }

    public class ExcelMimeTypes
    {
        public static string XLSX => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string XLSB => "application/vnd.ms-excel.sheet.binary.macroEnabled.12";
        public static string XLS => "application/vnd.ms-excel";
        public static string XLSM => "application/vnd.ms-excel.sheet.macroEnabled.12";
    }
}
