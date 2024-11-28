using api.Models;

namespace api.Repositories;

public interface IWellRepository : IBaseRepository
{
    Task<Well?> GetWell(Guid wellId);
    Well AddWell(Well well);
    void DeleteWell(Well well);
    Task<IEnumerable<Case>> GetCasesAffectedByDeleteWell(Guid wellId);
}
