using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IWellProjectRepository : IBaseRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<bool> WellProjectHasProfile(Guid WellProjectId, WellProjectProfileNames profileType);
    WellProject UpdateWellProject(WellProject wellProject);
    Task<WellProjectWell?> GetWellProjectWell(Guid wellProjectId, Guid wellId);
    WellProjectWell UpdateWellProjectWell(WellProjectWell wellProjectWell);
}
