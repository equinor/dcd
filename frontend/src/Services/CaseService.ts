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

    public async createCessationWellsCostOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/cessation-wells-cost-override/`,
            { body: dto },
        )
        return res
    }

    public async updateCessationWellsCostOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/cessation-wells-cost-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createCessationOffshoreFacilitiesCostOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/cessation-offshore-facilities-cost-override/`,
            { body: dto },
        )
        return res
    }

    public async updateCessationOffshoreFacilitiesCostOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/cessation-offshore-facilities-cost-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createCessationOnshoreFacilitiesCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/cessation-onshore-facilities-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateCessationOnshoreFacilitiesCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/cessation-onshore-facilities-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createTotalFeasibilityAndConceptStudiesOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/total-feasibility-and-concept-studies-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTotalFeasibilityAndConceptStudiesOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/total-feasibility-and-concept-studies-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createTotalFEEDStudiesOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/total-feed-studies-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTotalFEEDStudiesOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/total-feed-studies-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createTotalOtherStudiesCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/total-other-studies-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateTotalOtherStudiesCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/total-other-studies-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createHistoricCostCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/historic-cost-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateHistoricCostCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/historic-cost-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createOffshoreFacilitiesOperationsCostProfileOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/offshore-facilities-operations-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateOffshoreFacilitiesOperationsCostProfileOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/offshore-facilities-operations-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createWellInterventionCostProfileOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-intervention-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateWellInterventionCostProfileOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-intervention-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createOnshoreRelatedOPEXCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/onshore-related-opex-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateOnshoreRelatedOPEXCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/onshore-related-opex-cost-profile/${costProfileId}`,
            { body: dto },
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

    public async createAdditionalOPEXCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/additional-opex-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateAdditionalOPEXCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostDto,
    ): Promise<Components.Schemas.TimeSeriesCostDto> {
        const res: Components.Schemas.TimeSeriesCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/additional-opex-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetCaseService = async () => new CaseService({
    ...config.CaseService,
    accessToken: await getToken(loginAccessTokenKey)!,
})
