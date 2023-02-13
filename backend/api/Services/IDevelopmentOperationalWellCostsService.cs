using api.Dtos;
using api.Models;

namespace api.Services
{
    public interface IDevelopmentOperationalWellCostsService
    {
        DevelopmentOperationalWellCostsDto CreateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto);
        DevelopmentOperationalWellCosts? GetOperationalWellCosts(Guid id);
        DevelopmentOperationalWellCosts? GetOperationalWellCostsByProjectId(Guid id);
        DevelopmentOperationalWellCostsDto? UpdateOperationalWellCosts(DevelopmentOperationalWellCostsDto dto);
    }
}
