import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __TopsideService extends __BaseService {
    public async createTopside(sourceCaseId: string, body: Components.Schemas.TopsideDto) :Promise<Project> {
        const res = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }
}

export function GetTopsideService() {
    return new __TopsideService({
        ...config.TopsideService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
