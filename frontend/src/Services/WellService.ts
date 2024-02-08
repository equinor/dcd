import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __WellService extends __BaseService {
    public async deleteWell(projectId: string, wellId: string): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.delete(`projects/${projectId}/wells/${wellId}`)
        return Project.fromJSON(res)
    }
}

export const WellService = new __WellService({
    ...config.WellService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export async function GetWellService() {
    return new __WellService({
        ...config.WellService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
