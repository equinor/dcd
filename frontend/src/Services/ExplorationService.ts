import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __ExplorationService extends __BaseService {
    public async createExploration(sourceCaseId: string, body: Components.Schemas.ExplorationDto) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async updateExploration(body: Components.Schemas.ExplorationDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetExplorationService() {
    return new __ExplorationService({
        ...config.ExplorationService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
