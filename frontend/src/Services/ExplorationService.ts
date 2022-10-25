import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"
import { Exploration } from "../models/assets/exploration/Exploration"

export class __ExplorationService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.ExplorationDto) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.ExplorationDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async newUpdate(body: Components.Schemas.ExplorationDto): Promise<any> {
        const res: Components.Schemas.ExplorationDto = await this.put("/new", { body })
        return new Exploration(res)
    }
}

export async function GetExplorationService() {
    return new __ExplorationService({
        ...config.ExplorationService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
