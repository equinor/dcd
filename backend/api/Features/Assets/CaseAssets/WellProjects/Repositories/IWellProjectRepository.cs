using api.Features.CaseProfiles.Enums;
using api.Models;

namespace api.Features.Assets.CaseAssets.WellProjects.Repositories;

public interface IWellProjectRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> WellProjectHasProfile(Guid wellProjectId, WellProjectProfileNames profileType);
    Task<WellProject?> GetWellProjectWithDrillingSchedule(Guid drillingScheduleId);
    WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule);
}
