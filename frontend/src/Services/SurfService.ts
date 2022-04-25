import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __SurfService extends __BaseService {
    public async createSurf(sourceCaseId: string, body: Components.Schemas.SurfDto) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async updateSurf(body: Components.Schemas.SurfDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetSurfService() {
    return new __SurfService({
        ...config.SurfService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
