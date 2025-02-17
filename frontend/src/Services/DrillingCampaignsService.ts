import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class __DrillingCampaignsService extends __BaseService {
    public async updateCampaign(
        projectId: string,
        caseId: string,
        campaignId: string,
        body: Components.Schemas.CampaignDto,
    ): Promise<void> {
        const updateDto: Components.Schemas.UpdateCampaignDto = {
            rigUpgradingCost: body.rigUpgradingCost,
            rigUpgradingCostStartYear: body.rigUpgradingProfile.startYear,
            rigUpgradingCostValues: body.rigUpgradingProfile.values,
            rigMobDemobCost: body.rigMobDemobCost,
            rigMobDemobCostStartYear: body.rigMobDemobProfile.startYear,
            rigMobDemobCostValues: body.rigMobDemobProfile.values,
        }

        await this.put(
            `projects/${projectId}/cases/${caseId}/campaigns/${campaignId}`,
            { body: updateDto },
        )
    }
}

export const DrillingCampaignsService = new __DrillingCampaignsService({
    ...config.BaseUrl,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export const GetDrillingCampaignsService = async () => new __DrillingCampaignsService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
