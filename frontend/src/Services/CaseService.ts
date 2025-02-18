import { __BaseService } from "./__BaseService"

class CaseService extends __BaseService {
    public async create(
        projectId: string,
        data: Components.Schemas.CreateCaseDto,
    ): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.post(
            `projects/${projectId}/cases`,
            { body: data },
        )
        return res
    }

    public async updateCase(
        projectId: string,
        caseId: string,
        body: Components.Schemas.UpdateCaseDto,
    ): Promise<void> {
        await this.put(`projects/${projectId}/cases/${caseId}`, { body })
    }

    public async getCaseWithAssets(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.CaseWithAssetsDto> {
        const res = await this.get(`projects/${projectId}/cases/${caseId}/case-with-assets`)
        return res
    }

    public async duplicateCase(
        projectId: string,
        copyCaseId: string,
    ): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.postWithParams(
            `projects/${projectId}/cases/copy`,
            { body: {} },
            { params: { copyCaseId } },
        )
        return res
    }

    public async deleteCase(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDataDto> {
        const res: Components.Schemas.ProjectDataDto = await this.delete(
            `projects/${projectId}/cases/${caseId}`,
        )
        return res
    }

    public async saveProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.SaveTimeSeriesDto,
    ): Promise<Components.Schemas.TimeSeriesDto> {
        const res: Components.Schemas.TimeSeriesDto = await this.post(
            `projects/${projectId}/cases/${caseId}/profiles/save`,
            { body: dto },
        )
        return res
    }

    public async saveOverrideProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.SaveTimeSeriesOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesOverrideDto> {
        const res: Components.Schemas.TimeSeriesOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/override-profiles/save`,
            { body: dto },
        )
        return res
    }

    public async saveProfiles(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.SaveTimeSeriesListDto,
    ): Promise<object> {
        const res: object = await this.post(
            `projects/${projectId}/cases/${caseId}/profiles/save-batch`,
            { body: dto },
        )
        return res
    }
}

export const GetCaseService = () => new CaseService()
