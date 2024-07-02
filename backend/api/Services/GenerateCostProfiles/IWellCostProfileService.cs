using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellCostProfileService
{
    Task UpdateCostProfilesForWellsFromDrillingSchedules(List<Guid> drillingScheduleIds);
    Task UpdateCostProfilesForWells(List<Guid> wellIds);
}
