import { __BaseService } from "./__BaseService"

import { Project } from "../models/Project"
import { config } from "./config"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"

class __WellService extends __BaseService {
    public async createWell(data: Components.Schemas.WellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.post("", { body: data })
        return Project.fromJSON(res)
    }

    public async updateWell(body: Components.Schemas.WellDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export const WellService = new __WellService({
    ...config.WellService,
    accessToken: window.sessionStorage.getItem("loginAccessToken")!,
})

export function GetWellService() {
    return new __WellService({
        ...config.WellService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
