using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellService
{
    Task<ProjectDto> DeleteWell(Guid wellId);
    Task<Well> GetWell(Guid wellId);
}
