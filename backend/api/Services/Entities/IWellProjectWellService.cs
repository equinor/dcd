using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectWellService
{
    Task<List<WellProjectWell>> GetWellProjectWellsForWellProject(Guid wellProjectId);
}
