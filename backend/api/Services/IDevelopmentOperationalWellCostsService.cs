using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDevelopmentOperationalWellCostsService
    {
        Task<DevelopmentOperationalWellCostsDto?> UpdateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto);
        Task<DevelopmentOperationalWellCostsDto> CreateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto);
        Task<DevelopmentOperationalWellCosts?> GetOperationalWellCostsByProjectId(Guid id);
        Task<DevelopmentOperationalWellCosts?> GetOperationalWellCosts(Guid id);
    }
}
