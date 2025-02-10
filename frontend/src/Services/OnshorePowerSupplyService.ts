import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class OnshorePowerSupplyService extends __BaseService {
    public async updateOnshorePowerSupply(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateOnshorePowerSupplyDto,
    ): Promise<Components.Schemas.OnshorePowerSupplyDto> {
        const res: Components.Schemas.OnshorePowerSupplyDto = await this.put(
            `projects/${projectId}/cases/${caseId}/onshore-power-supply`,
            { body: dto },
        )
        return res
    }
}

export const GetOnshorePowerSupplyService = async () => new OnshorePowerSupplyService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
