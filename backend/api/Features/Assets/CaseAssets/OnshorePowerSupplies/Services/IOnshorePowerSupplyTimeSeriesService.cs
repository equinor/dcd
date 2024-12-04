using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Create;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos.Update;

namespace api.Features.Assets.CaseAssets.OnshorePowerSupplies.Services;

public interface IOnshorePowerSupplyTimeSeriesService
{
    Task<OnshorePowerSupplyCostProfileDto> AddOrUpdateOnshorePowerSupplyCostProfile(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        UpdateOnshorePowerSupplyCostProfileDto dto
    );
    Task<OnshorePowerSupplyCostProfileOverrideDto> CreateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        CreateOnshorePowerSupplyCostProfileOverrideDto dto
    );
    Task<OnshorePowerSupplyCostProfileOverrideDto> UpdateOnshorePowerSupplyCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid onshorePowerSupplyId,
        Guid costProfileId,
        UpdateOnshorePowerSupplyCostProfileOverrideDto dto);
}
