using System.Linq.Expressions;

using api.Enums;
using api.Models;
using api.Repositories;

namespace api.Features.Assets.CaseAssets.WellProjects.Repositories;

public interface IWellProjectRepository : IBaseRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<WellProject?> GetWellProjectWithIncludes(Guid wellProjectId, params Expression<Func<WellProject, object>>[] includes);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> WellProjectHasProfile(Guid wellProjectId, WellProjectProfileNames profileType);
    Task<WellProject?> GetWellProjectWithDrillingSchedule(Guid drillingScheduleId);
    WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule);
}
