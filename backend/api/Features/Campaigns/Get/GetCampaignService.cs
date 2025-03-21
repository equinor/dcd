using api.Context;
using api.Context.Extensions;
using api.Features.Cases.GetWithAssets.AssetMappers;
using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Get;

public class GetCampaignService(DcdDbContext context)
{
    public async Task<CampaignDto> Get(Guid projectId, Guid caseId, Guid campaignId)
    {
        var projectPk = await context.GetPrimaryKeyForProjectIdOrRevisionId(projectId);

        var campaign = await context.Campaigns
            .Where(x => x.Case.ProjectId == projectPk)
            .Where(x => x.CaseId == caseId)
            .Where(x => x.Id == campaignId)
            .SingleAsync();

        await context.CampaignWells
            .Include(x => x.Well)
            .Where(x => x.CampaignId == campaignId)
            .LoadAsync();

        return CampaignMapper.MapToDto(campaign);
    }
}
