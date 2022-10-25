import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"
import { WellProject } from "../models/assets/wellproject/WellProject"

export class __WellProjectService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.WellProjectDto) : Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.WellProjectDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async newUpdate(body: Components.Schemas.WellProjectDto): Promise<any> {
        const res: Components.Schemas.WellProjectDto = await this.put("/new", { body })
        return new WellProject(res)
    }
}

export async function GetWellProjectService() {
    return new __WellProjectService({
        ...config.WellProjectService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
