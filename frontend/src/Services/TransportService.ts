import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __TransportService extends __BaseService {
    public async createTransport(sourceCaseId: string, body: Components.Schemas.TransportDto) : Promise<Project> {
        const res = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }
}

export function GetTransportService() {
    return new __TransportService({
        ...config.TransportService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
