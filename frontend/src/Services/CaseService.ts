import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

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

    public async createProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/profiles`,
            { body: dto },
        )
        return res
    }

    public async createOverrideProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/override-profiles`,
            { body: dto },
        )
        return res
    }

    public async updateProfile(
        projectId: string,
        caseId: string,
        profileId: string,
        dto: Components.Schemas.UpdateTimeSeriesDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/profiles/${profileId}`,
            { body: dto },
        )
        return res
    }

    public async updateOverrideProfile(
        projectId: string,
        caseId: string,
        profileId: string,
        dto: Components.Schemas.UpdateTimeSeriesOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/override-profiles/${profileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetCaseService = async () => new CaseService({
    ...config.CaseService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
