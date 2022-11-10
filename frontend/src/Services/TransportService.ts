import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { IAssetService } from "./IAssetService"
import { Transport } from "../models/assets/transport/Transport"

export class __TransportService extends __BaseService implements IAssetService {
    public async create(sourceCaseId: string, body: Components.Schemas.TransportDto) : Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams("", { body }, { params: { sourceCaseId } })
        return Project.fromJSON(res)
    }

    public async update(body: Components.Schemas.TransportDto): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.put("", { body })
        return Project.fromJSON(res)
    }

    public async newUpdate(body: Components.Schemas.TransportDto): Promise<any> {
        const res: Components.Schemas.TransportDto = await this.put("/new", { body })
        return new Transport(res)
    }
}

export async function GetTransportService() {
    return new __TransportService({
        ...config.TransportService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
