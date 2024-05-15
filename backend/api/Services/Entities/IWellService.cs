using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellService
{
    Task<Well> GetWell(Guid wellId);
}
