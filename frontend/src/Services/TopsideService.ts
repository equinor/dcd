import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"

export class __TopsideService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.TopsideDto) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.TopsideDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export async function GetTopsideService() {
    return new __TopsideService({
        ...config.TopsideService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
