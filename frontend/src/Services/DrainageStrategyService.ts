import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"

export class __DrainageStrategyService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.DrainageStrategyDto) :
        Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.DrainageStrategyDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async newUpdate(body: Components.Schemas.DrainageStrategyDto): Promise<any> {
        const res: Components.Schemas.DrainageStrategyDto = await this.put("/new", { body })
        return new DrainageStrategy(res)
    }
}

export async function GetDrainageStrategyService() {
    return new __DrainageStrategyService({
        ...config.DrainageStrategyService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
