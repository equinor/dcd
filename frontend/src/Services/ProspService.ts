import { config } from "./config"
import { __BaseService } from "./__BaseService"

import { loginAccessTokenKey, getToken } from "../Utils/common"

export class __ProspService extends __BaseService {
    async getSharePointFileNamesAndId(body: Components.Schemas.SharePointSiteUrlDto, projectId: string) {
        const driveItem: Components.Schemas.SharePointFileDto[] = await this.post(`projects/${projectId}/prosp/list`, { body })
        return driveItem
    }

    public async importFromSharePoint(
        projectId: string,
        body: Components.Schemas.SharePointImportDto[],
    ): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.post(`projects/${projectId}/prosp/import`, { body })
        return res
    }
}

export const GetProspService = async () => new __ProspService({
    ...config.UploadService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
