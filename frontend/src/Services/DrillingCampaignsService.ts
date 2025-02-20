import { __BaseService } from "./__BaseService"

class DrillingCampaignsService extends __BaseService {
    public async updateCampaign(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.UpdateCampaignDto,
    ): Promise<void> {
        // const updateDto: Components.Schemas.UpdateCampaignDto = {
        //     rigUpgradingCostStartYear: body.rigUpgradingProfile?.startYear,
        //     rigUpgradingCostValues: body.rigUpgradingProfile?.values,
        //     rigMobDemobCostStartYear: body.rigMobDemobProfile?.startYear,
        //     rigMobDemobCostValues: body.rigMobDemobProfile?.values,
        //     campaignWells: body.campaignWells,
        // }
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}`,
            { body },
        )
    }

    public async updateCampaignCost(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.UpdateCampaignCostDto,
    ): Promise<void> {
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}/cost`,
            { body },
        )
    }

    public async updateCampaignWells(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.SaveCampaignWellDto[],
    ): Promise<void> {
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}/wells`,
            { body },
        )
    }
}

export const GetDrillingCampaignsService = () => new DrillingCampaignsService()
