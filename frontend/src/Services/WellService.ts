import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Well } from "../models/Well"

class __WellService extends __BaseService {
    public async deleteWell(wellId: string): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.delete(`${wellId}`)
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
