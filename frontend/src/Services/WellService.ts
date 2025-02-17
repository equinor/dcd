import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class WellService extends __BaseService {
    public async isWellInUse(
        projectId: string,
        wellId: string,
    ): Promise<boolean> {
        const res = await this.get(`projects/${projectId}/wells/${wellId}/is-in-use`)
        return res
    }
}

export const GetWellService = async () => new WellService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
