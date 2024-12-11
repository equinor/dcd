using System.Linq.Expressions;

using api.Features.CaseProfiles.Enums;
using api.Features.CaseProfiles.Repositories;
using api.Models;

namespace api.Features.Assets.CaseAssets.WellProjects.Repositories;

public interface IWellProjectRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<WellProject?> GetWellProjectWithIncludes(Guid wellProjectId, params Expression<Func<WellProject, object>>[] includes);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> WellProjectHasProfile(Guid wellProjectId, WellProjectProfileNames profileType);
    Task<WellProject?> GetWellProjectWithDrillingSchedule(Guid drillingScheduleId);
    WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule);
}
