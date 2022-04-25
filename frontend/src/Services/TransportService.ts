import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"

export class __TransportService extends __BaseService {
    public async createTransport(sourceCaseId: string, body: Components.Schemas.TransportDto) : Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async updateTransport(body: Components.Schemas.TransportDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }
}

export function GetTransportService() {
    return new __TransportService({
        ...config.TransportService,
        accessToken: GetToken(LoginAccessTokenKey)!,
    })
}
