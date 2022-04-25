import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __DrainageStrategyService extends __BaseService {
    public async createDrainageStrategy(sourceCaseId: string, body: Components.Schemas.DrainageStrategyDto) :
        Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async updateDrainageStrategy(body: Components.Schemas.DrainageStrategyDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetDrainageStrategyService() {
    return new __DrainageStrategyService({
        ...config.DrainageStrategyService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
