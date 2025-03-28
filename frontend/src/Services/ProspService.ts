import { __BaseService } from "./__BaseService"

export class ProspService extends __BaseService {
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

    public async checkForUpdate(
        projectId: string,
        caseId: string,
    ): Promise<boolean> {
        const res: boolean = await this.get(`projects/${projectId}/cases/${caseId}/prosp/check-for-update`)

        return res
    }
}

export const GetProspService = () => new ProspService()
