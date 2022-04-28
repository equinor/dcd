import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __SubstructureService extends __BaseService {
    public async create(sourceCaseId: string, body: Components.Schemas.SubstructureDto) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.SubstructureDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetSubstructureService() {
    return new __SubstructureService({
        ...config.SubstructureService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
