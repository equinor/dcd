import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"

export class __SubstructureService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.SubstructureDto) :Promise<Project> {
        const res = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.SubstructureDto): Promise<Project> {
        const res = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetSubstructureService() {
    return new __SubstructureService({
        ...config.SubstructureService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
