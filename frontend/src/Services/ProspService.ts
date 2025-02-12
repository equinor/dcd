import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { loginAccessTokenKey, getToken } from "../Utils/common"

export class __ProspService extends __BaseService {
    async getSharePointFileNamesAndId(body: Components.Schemas.UrlDto, projectId: string) {
        const driveItem: Components.Schemas.DriveItemDto[] = await this.post(`projects/${projectId}/sharepoint`, { body })
        return driveItem
    }

    public async importFromSharepoint(
        projectId: string,
        body: Components.Schemas.SharePointImportDto[],
    ): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.postWithParams(
            `${projectId}/sharepoint`,
            { body },

            { params: { projectId } },
        )
        return res
    }
}

export const GetProspService = async () => new __ProspService({
    ...config.UploadService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
