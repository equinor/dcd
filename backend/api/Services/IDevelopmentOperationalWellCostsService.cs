using api.Dtos;
using api.Models;

namespace api.Services;

public interface IDevelopmentOperationalWellCostsService
{
    Task<DevelopmentOperationalWellCosts?> GetOperationalWellCosts(Guid id);
}
