using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDevelopmentOperationalWellCostsService
    {
        Task<DevelopmentOperationalWellCostsDto?> UpdateOperationalWellCostsAsync(DevelopmentOperationalWellCostsDto dto);
        Task<DevelopmentOperationalWellCostsDto> CreateOperationalWellCostsAsync(DevelopmentOperationalWellCostsDto dto);
        Task<DevelopmentOperationalWellCosts?> GetOperationalWellCostsByProjectIdAsync(Guid id);
        Task<DevelopmentOperationalWellCosts?> GetOperationalWellCostsAsync(Guid id);
    }
}
