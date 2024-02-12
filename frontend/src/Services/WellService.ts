import { __BaseService } from "./__BaseService"

import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class WellService extends __BaseService {
    public async deleteWell(projectId: string, wellId: string): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.delete(`projects/${projectId}/wells/${wellId}`)
        return res
    }
}

export async function GetWellService() {
    return new WellService({
        ...config.WellService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
