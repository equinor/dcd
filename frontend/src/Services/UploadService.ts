import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { DriveItem } from "../models/sharepoint/DriveItem"

export class __UploadService extends __BaseService {
    public async create(sourceCaseId: string, projectId: string, body: any) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "",
            { body },

            { params: { sourceCaseId, projectId } },
        )
        return Project.fromJSON(res)
    }

    async getSharePointFileNamesAndId(id: string) {
        const driveItem: DriveItem[] = await this.get<Components.Schemas.DriveItemDto[]>("")
        return driveItem
    }
}

export async function GetUploadService() {
    return new __UploadService({
        ...config.UploadService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
