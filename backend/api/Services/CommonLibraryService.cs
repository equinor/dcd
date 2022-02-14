using api.Adapters;
using api.Dtos;

using Equinor.TI.CommonLibrary.Client;

using Statoil.TI.CommonLibrary.Entities.GenericView;

namespace api.Services
{
    public class CommonLibraryService
    {
        private readonly CommonLibraryClient _commonLibraryClient;
        private readonly ILogger<CommonLibraryService> _logger;

        public CommonLibraryService(ILogger<CommonLibraryService> logger, CommonLibraryClientOptions clientOptions)
        {
            _commonLibraryClient = new CommonLibraryClient(clientOptions);
            _logger = logger;
        }

        public static string BuildTokenConnectionString(string clientId, string tenantId, string clientSecret)
        {
            return $"RunAs=App;AppId={clientId};TenantId={tenantId};AppKey={clientSecret}";
        }

        public async Task<List<ProjectDto>> GetProjectsFromCommonLibrary()
        {
            _logger.LogInformation("Attempting to retrieve project list from Common Library.");

            var projects = new List<ProjectDto>();
            try
            {
                projects = await GetProjects();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to retrieve project list from Common Library.");
                throw;
            }

            _logger.LogInformation("Successfully retrieved project list from Common Library.");

            return projects;
        }

        private async Task<List<ProjectDto>> GetProjects()
        {
            var query = QuerySpec
                .Library("ProjectMaster")
                .WhereIsValid()
                .Include(new[] {"Name", "Description", "GUID", "CVPID",
                        "ProjectState", "Phase", "PortfolioOrganizationalUnit", "OrganizationalUnit",
                        "ProjectCategory", "Country", "GeographicalArea", "IsOffshore",
                        "DGADate", "DGBDate", "DGCDate", "DG0FDate",
                        "DG0Date", "DG1Date", "DG2Date", "DG3Date",
                        "DG4Date", "ProductionStartupDate", "InternalComment"}
                );
            var dynamicProjects = await _commonLibraryClient.GenericViewsQueryAsync(query);
            return ConvertDynamicProjectsToProjectDtos(dynamicProjects);
        }

        private static List<ProjectDto> ConvertDynamicProjectsToProjectDtos(List<dynamic> dynamicProjects)
        {
            var projects = new List<ProjectDto>();
            foreach (dynamic project in dynamicProjects)
            {
                projects.Add(ProjectDtoAdapter.Convert(project));
            }
            return projects;
        }
    }
}
