import { __BaseService } from "./__BaseService"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class TopsideService extends __BaseService {
    public async updateTopside(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateTopsideDto,
    ): Promise<Components.Schemas.TopsideDto> {
        const res: Components.Schemas.TopsideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/topside`,
            { body: dto },
        )
        return res
    }
}

export const GetTopsideService = async () => new TopsideService({
    accessToken: await getToken(loginAccessTokenKey)!,
})
