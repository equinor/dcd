import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { DriveItem } from "../models/sharepoint/DriveItem"

export class __ProspService extends __BaseService {
    public async create(sourceCaseId: string, projectId: string, body: any) :Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "",
            { body },

            { params: { sourceCaseId, projectId } },
        )
        return res
    }

    async getSharePointFileNamesAndId(body:any) {
        const driveItem: DriveItem[] = await this.post("sharepoint", { body })
        return driveItem
    }

    public async importFromSharepoint(projectId: string, body: any): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            `${projectId}/sharepoint`,
            { body },

            { params: { projectId } },
        )
        return res
    }
}

export const GetProspService = async () => {
    return new __ProspService({
        ...config.UploadService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
