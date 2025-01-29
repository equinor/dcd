import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TopsideService extends __BaseService {
    public async updateTopside(
        projectId: string,
        caseId: string,
        topsideId: string,
        dto: Components.Schemas.UpdateTopsideDto,
    ): Promise<Components.Schemas.TopsideDto> {
        const res: Components.Schemas.TopsideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/topsides/${topsideId}`,
            { body: dto },
        )
        return res
    }
}

export const GetTopsideService = async () => new TopsideService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
