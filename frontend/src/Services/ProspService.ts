import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { LoginAccessTokenKey, GetToken } from "../Utils/common"
import { Project } from "../models/Project"
import { DriveItem } from "../models/sharepoint/DriveItem"

export class __ProspService extends __BaseService {
    public async create(sourceCaseId: string, projectId: string, body: any) :Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "",
            { body },

            { params: { sourceCaseId, projectId } },
        )
        return Project.fromJSON(res)
    }

    async getSharePointFileNamesAndId(body:any) {
        const driveItem: DriveItem[] = await this.post("sharepointfileinfo", { body })
        return driveItem
    }

    public async importFromSharepoint(projectId: string, body: any): Promise<Project> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            "sharepointfiledata",
            { body },

            { params: { projectId } },
        )
        return Project.fromJSON(res)
    }
}

export async function GetProspService() {
    return new __ProspService({
        ...config.UploadService,
        accessToken: await GetToken(LoginAccessTokenKey)!,
    })
}
