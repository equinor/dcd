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
            _logger = logger;
            _logger.LogInformation("Attempting to create a Commmon Library client.");
            try
            {
                _commonLibraryClient = new CommonLibraryClient(clientOptions);
            }
            catch (Exception)
            {
                _logger.LogError("Failed to create a Commmon Library client.");
                throw;
            }
            _logger.LogInformation("Successfully created a Commmon Library client.");
        }

        public static string BuildTokenConnectionString(string clientId, string tenantId, string clientSecret)
        {
            return $"RunAs=App;AppId={clientId};TenantId={tenantId};AppKey={clientSecret}";
        }

        public async Task<List<CommonLibraryProjectDto>> GetProjectsFromCommonLibrary()
        {
            _logger.LogInformation("Attempting to retrieve project list from Common Library.");

            var projects = new List<CommonLibraryProjectDto>();
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

        private async Task<List<CommonLibraryProjectDto>> GetProjects()
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

        private static List<CommonLibraryProjectDto> FilterProjects(List<CommonLibraryProjectDto> projects) {
            string[] whiteList = new string[] {"PlatformFPSO", "Subsea", "FPSO", "Platform", "TieIn", "Null"};

            var filteredList = projects.Where(p => p.ProjectState != "COMPLETED" && whiteList.Contains(p.ProjectCategory.ToString()));

            return filteredList.OrderBy(p => p.Name).ToList();
        }

        private static List<CommonLibraryProjectDto> ConvertDynamicProjectsToProjectDtos(List<dynamic> dynamicProjects)
        {
            var projects = new List<CommonLibraryProjectDto>();
            foreach (dynamic project in dynamicProjects)
            {
                var convertedProject = CommonLibraryProjectDtoAdapter.Convert(project);
                if (convertedProject != null)
                {
                    projects.Add(convertedProject);
                }
            }
            return FilterProjects(projects);
        }
    }
}
