using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Create;
using api.Features.Assets.CaseAssets.Surfs.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Surfs.Services;

public interface ISurfTimeSeriesService
{
    Task<SurfCostProfileDto> AddOrUpdateSurfCostProfile(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        UpdateSurfCostProfileDto dto
    );
    Task<SurfCostProfileOverrideDto> CreateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        CreateSurfCostProfileOverrideDto dto
    );

    Task<SurfCostProfileOverrideDto> UpdateSurfCostProfileOverride(
        Guid projectId,
        Guid caseId,
        Guid surfId,
        Guid costProfileId,
        UpdateSurfCostProfileOverrideDto updatedSurfCostProfileOverrideDto
    );
}
