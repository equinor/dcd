using System.Linq.Expressions;

using api.Enums;
using api.Models;

namespace api.Repositories;

public interface IWellProjectRepository : IBaseRepository
{
    Task<WellProject?> GetWellProject(Guid wellProjectId);
    Task<WellProject?> GetWellProjectWithIncludes(Guid wellProjectId, params Expression<Func<WellProject, object>>[] includes);
    Task<Well?> GetWell(Guid wellId);
    Task<bool> WellProjectHasProfile(Guid WellProjectId, WellProjectProfileNames profileType);
    WellProject UpdateWellProject(WellProject wellProject);
    Task<DrillingSchedule?> GetWellProjectWellDrillingSchedule(Guid drillingScheduleId);
    Task<WellProject?> GetWellProjectWithDrillingSchedule(Guid drillingScheduleId);
    DrillingSchedule UpdateWellProjectWellDrillingSchedule(DrillingSchedule drillingScheduleId);
    WellProjectWell CreateWellProjectWellDrillingSchedule(WellProjectWell wellProjectWellWithDrillingSchedule);
}
