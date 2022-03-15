import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __WellProjectService extends __BaseService {
    public async createWellProject(sourceCaseId: string, body: Components.Schemas.WellProjectDto) : Promise<Project> {
        const res = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }
}

export function GetWellProjectService() {
    return new __WellProjectService({
        ...config.WellProjectService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
