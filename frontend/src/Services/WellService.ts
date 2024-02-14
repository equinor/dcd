import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { loginAccessTokenKey, getToken } from "../Utils/common"

class WellService extends __BaseService {
    public async deleteWell(projectId: string, wellId: string): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.delete(`projects/${projectId}/wells/${wellId}`)
        return res
    }
}

export const GetWellService = async () => {
    return new WellService({
        ...config.WellService,
        accessToken: await getToken(loginAccessTokenKey)!,
    })
}
