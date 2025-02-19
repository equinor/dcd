import { __BaseService } from "./__BaseService"

class DrillingCampaignsService extends __BaseService {
    public async updateCampaign(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.CampaignDto,
    ): Promise<void> {
        const updateDto: Components.Schemas.UpdateCampaignDto = {
            rigUpgradingCost: body.rigUpgradingCost,
            rigUpgradingCostStartYear: body.rigUpgradingProfile?.startYear,
            rigUpgradingCostValues: body.rigUpgradingProfile?.values,
            rigMobDemobCost: body.rigMobDemobCost,
            rigMobDemobCostStartYear: body.rigMobDemobProfile?.startYear,
            rigMobDemobCostValues: body.rigMobDemobProfile?.values,
        }
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}`,
            { body: updateDto },
        )
    }
}

export const GetDrillingCampaignsService = async () => new DrillingCampaignsService()
