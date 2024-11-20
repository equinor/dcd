using api.Models.Fusion;

namespace api.Services.Fusion;

public interface IOrgChartMemberService
{
    Task<List<FusionPersonV1>> GetAllPersonsOnProject(Guid projectMasterId, int top, int skip);
}
