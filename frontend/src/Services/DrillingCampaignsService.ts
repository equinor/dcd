import { __BaseService } from "./__BaseService"

class DrillingCampaignsService extends __BaseService {
    public async updateCampaign(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.UpdateCampaignDto,
    ): Promise<void> {
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}`,
            { body },
        )
    }

    public async updateRigUpgradingCost(
        projectId: string,
        caseId: string,
        campaignId: string,
        cost: number,
    ): Promise<void> {
        console.log("updateRigUpgradingCost")
        console.log("projectId: ", projectId)
        console.log("caseId: ", caseId)
        console.log("campaignId: ", campaignId)
        console.log("cost: ", cost)
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}/rig-upgrading-cost`,
            { body: { cost } },
        )
    }

    public async updateRigMobDemobCost(
        projectId: string,
        caseId: string,
        campaignId: string,
        cost: number,
    ): Promise<void> {
        console.log("updateRigMobDemobCost")
        console.log("projectId: ", projectId)
        console.log("caseId: ", caseId)
        console.log("campaignId: ", campaignId)
        console.log("cost: ", cost)
        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}/rig-mobdemob-cost`,
            { body: { cost } },
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
