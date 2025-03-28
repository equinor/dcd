using api.Context;
using api.Context.Extensions;
using api.Exceptions;
using api.Features.Cases.Recalculation;
using api.Models;
using api.Models.Enums;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Campaigns.Create;

public class CreateCampaignService(DcdDbContext context, RecalculationService recalculationService)
{
    public async Task<Guid> CreateCampaign(Guid projectId, Guid caseId, CreateCampaignDto createCampaignDto)
    {
        ValidateDto(createCampaignDto);

        var projectPk = await context.GetPrimaryKeyForProjectId(projectId);

        var caseItem = await context.Cases.SingleAsync(x => x.ProjectId == projectPk && x.Id == caseId);

        var campaign = new Campaign
        {
            CaseId = caseItem.Id,
            CampaignType = createCampaignDto.CampaignType,
            RigUpgradingCostValues = [],
            RigMobDemobCostValues = []
        };

        context.Campaigns.Add(campaign);

        await context.UpdateCaseUpdatedUtc(caseId);
        await recalculationService.SaveChangesAndRecalculateCase(caseId);

        return campaign.Id;
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
