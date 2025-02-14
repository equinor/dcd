using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Campaigns.Create;

public class CreateCampaignService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task<Guid> CreateCampaign(Guid projectId, Guid caseId, CreateCampaignDto createCampaignDto)
    {
        ValidateDto(createCampaignDto);

        var caseItem = await context.Cases.SingleAsync(x => x.ProjectId == projectId && x.Id == caseId);

        var campaignId = Guid.NewGuid();

        caseItem.Campaigns.Add(new Campaign
        {
            Id = campaignId,
            CampaignType = createCampaignDto.CampaignType,
            RigUpgradingCostValues = [],
            RigMobDemobCostValues = []
        });

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return campaignId;
    }

    private static void ValidateDto(CreateCampaignDto createCampaignDto)
    {
        switch (createCampaignDto.CampaignType)
        {
            case CampaignType.ExplorationCampaign:
            case CampaignType.DevelopmentCampaign:
                return;
            default:
                throw new InvalidInputException("Invalid campaign type");
        }
    }
}
