using api.Models;

namespace api.Repositories;

public interface IWellRepository : IBaseRepository
{
    Task<Well?> GetWell(Guid wellId);
    Well UpdateWell(Well well);
}
