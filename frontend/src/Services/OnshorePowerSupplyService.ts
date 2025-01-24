import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class OnshorePowerSupplyService extends __BaseService {
    public async updateOnshorePowerSupply(
        projectId: string,
        caseId: string,
        onshorePowerSupplyId: string,
        dto: Components.Schemas.UpdateOnshorePowerSupplyDto,
    ): Promise<Components.Schemas.OnshorePowerSupplyDto> {
        const res: Components.Schemas.OnshorePowerSupplyDto = await this.put(
            `projects/${projectId}/cases/${caseId}/onshore-power-supplies/${onshorePowerSupplyId}`,
            { body: dto },
        )
        return res
    }

    public async createOnshorePowerSupplyCostProfileOverride(
        projectId: string,
        caseId: string,
        onshorePowerSupplyId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/onshore-power-supplies/${onshorePowerSupplyId}/cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateOnshorePowerSupplyCostProfileOverride(
        projectId: string,
        caseId: string,
        onshorePowerSupplyId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/onshore-power-supplies/${onshorePowerSupplyId}/cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetOnshorePowerSupplyService = async () => new OnshorePowerSupplyService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
