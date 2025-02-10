import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class SurfService extends __BaseService {
    public async updateSurf(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.UpdateSurfDto,
    ): Promise<Components.Schemas.SurfDto> {
        const res: Components.Schemas.SurfDto = await this.put(
            `projects/${projectId}/cases/${caseId}/surf`,
            { body: dto },
        )
        return res
    }
}

export const GetSurfService = async () => new SurfService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
