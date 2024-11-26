using api.Features.FusionIntegration.OrgChart.Models;

namespace api.Features.FusionIntegration.OrgChart;

public interface IOrgChartMemberService
{
    Task<List<FusionPersonV1>> GetAllPersonsOnProject(Guid projectMasterId, int top, int skip);
}
