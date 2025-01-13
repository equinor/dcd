using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos.Update;

namespace api.Features.Assets.CaseAssets.Topsides.Services;

public interface ITopsideService
{
    Task<TopsideDto> UpdateTopside<TDto>(Guid projectId, Guid caseId, Guid topsideId, TDto updatedTopsideDto) where TDto : BaseUpdateTopsideDto;
}
