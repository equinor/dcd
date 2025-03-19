import { __BaseService } from "./__BaseService"

class DrillingCampaignsService extends __BaseService {
    public async createCampaign(
        projectId: string,
        caseId: string,
        body: Components.Schemas.CreateCampaignDto,
    ): Promise<Components.Schemas.CampaignDto> {
        const dto = await this.post(
            `projects/${projectId}/cases/${caseId}/campaigns`,
            { body },
        )

        return dto
    }

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
