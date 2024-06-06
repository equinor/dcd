using api.Models;

namespace api.Repositories;

public interface IWellRepository
{
    Task<Well?> GetWell(Guid wellId);
    Task<Well> UpdateWell(Well well);
}
