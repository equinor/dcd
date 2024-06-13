import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class CaseService extends __BaseService {
    public async create(
        projectId: string,
        data: Components.Schemas.CreateCaseDto,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.post(
            `projects/${projectId}/cases`,
            { body: data },
        )
        return res
    }

    public async updateCase(
        projectId: string,
        caseId: string,
        body: Components.Schemas.APIUpdateCaseDto,
    ): Promise<Components.Schemas.CaseDto> {
        const res: Components.Schemas.CaseDto = await this.put(
            `projects/${projectId}/cases/${caseId}`,
            { body },
        )
        return res
    }

    public async updateCaseAndProfiles(
        projectId: string,
        caseId: string,
        body: Components.Schemas.APIUpdateCaseWithProfilesDto,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.put(
            `projects/${projectId}/cases/${caseId}/update-case-and-profiles`,
            { body },
        )
        return res
    }

    public async getCase(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res = await this.get(`projects/${projectId}/cases/${caseId}`)
        return res
    }

    public async getCaseWithAssets(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res = await this.get(`projects/${projectId}/cases/${caseId}/case-with-assets`)
        return res
    }

    public async duplicateCase(
        projectId: string,
        copyCaseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.postWithParams(
            `projects/${projectId}/cases/copy`,
            { body: {} },
            { params: { copyCaseId } },
        )
        return res
    }

    public async deleteCase(
        projectId: string,
        caseId: string,
    ): Promise<Components.Schemas.ProjectDto> {
        const res: Components.Schemas.ProjectDto = await this.delete(
            `projects/${projectId}/cases/${caseId}`,
        )
        return res
    }

    public async createCessationWellsCostOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateCessationWellsCostOverrideDto,
    ): Promise<Components.Schemas.CessationWellsCostOverrideDto> {
        const res: Components.Schemas.CessationWellsCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/cessation-wells-cost-override/`,
            { body: dto },
        )
        return res
    }

    public async updateCessationWellsCostOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateCessationWellsCostOverrideDto,
    ): Promise<Components.Schemas.CessationWellsCostOverrideDto> {
        const res: Components.Schemas.CessationWellsCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/cessation-wells-cost-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createCessationOffshoreFacilitiesCostOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateCessationOffshoreFacilitiesCostOverrideDto,
    ): Promise<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto> {
        const res: Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/cessation-offshore-facilities-cost-override/`,
            { body: dto },
        )
        return res
    }

    public async updateCessationOffshoreFacilitiesCostOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateCessationOffshoreFacilitiesCostOverrideDto,
    ): Promise<Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto> {
        const res: Components.Schemas.CessationOffshoreFacilitiesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/cessation-offshore-facilities-cost-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createTotalFeasibilityAndConceptStudiesOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTotalFeasibilityAndConceptStudiesOverrideDto,
    ): Promise<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto> {
        const res: Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/total-feasibility-and-concept-studies-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTotalFeasibilityAndConceptStudiesOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTotalFeasibilityAndConceptStudiesOverrideDto,
    ): Promise<Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto> {
        const res: Components.Schemas.TotalFeasibilityAndConceptStudiesOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/total-feasibility-and-concept-studies-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createTotalFEEDStudiesOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateTotalFEEDStudiesOverrideDto,
    ): Promise<Components.Schemas.TotalFEEDStudiesOverrideDto> {
        const res: Components.Schemas.TotalFEEDStudiesOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/total-feed-studies-override/`,
            { body: dto },
        )
        return res
    }

    public async updateTotalFEEDStudiesOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTotalFEEDStudiesOverrideDto,
    ): Promise<Components.Schemas.TotalFEEDStudiesOverrideDto> {
        const res: Components.Schemas.TotalFEEDStudiesOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/total-feed-studies-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createHistoricCostCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateHistoricCostCostProfileDto,
    ): Promise<Components.Schemas.HistoricCostCostProfileDto> {
        const res: Components.Schemas.HistoricCostCostProfileDto = await this.post(
            `projects/${projectId}/cases/${caseId}/historic-cost-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateHistoricCostCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateHistoricCostCostProfileDto,
    ): Promise<Components.Schemas.HistoricCostCostProfileDto> {
        const res: Components.Schemas.HistoricCostCostProfileDto = await this.put(
            `projects/${projectId}/cases/${caseId}/historic-cost-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createOffshoreFacilitiesOperationsCostProfileOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateOffshoreFacilitiesOperationsCostProfileOverrideDto,
    ): Promise<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto> {
        const res: Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/offshore-facilities-operations-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateOffshoreFacilitiesOperationsCostProfileOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateOffshoreFacilitiesOperationsCostProfileOverrideDto,
    ): Promise<Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto> {
        const res: Components.Schemas.OffshoreFacilitiesOperationsCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/offshore-facilities-operations-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createWellInterventionCostProfileOverride(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateWellInterventionCostProfileOverrideDto,
    ): Promise<Components.Schemas.WellInterventionCostProfileOverrideDto> {
        const res: Components.Schemas.WellInterventionCostProfileOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-intervention-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateWellInterventionCostProfileOverride(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateWellInterventionCostProfileOverrideDto,
    ): Promise<Components.Schemas.WellInterventionCostProfileOverrideDto> {
        const res: Components.Schemas.WellInterventionCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-intervention-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createOnshoreRelatedOPEXCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateOnshoreRelatedOPEXCostProfileDto,
    ): Promise<Components.Schemas.OnshoreRelatedOPEXCostProfileDto> {
        const res: Components.Schemas.OnshoreRelatedOPEXCostProfileDto = await this.post(
            `projects/${projectId}/cases/${caseId}/onshore-related-opex-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateOnshoreRelatedOPEXCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateOnshoreRelatedOPEXCostProfileDto,
    ): Promise<Components.Schemas.OnshoreRelatedOPEXCostProfileDto> {
        const res: Components.Schemas.OnshoreRelatedOPEXCostProfileDto = await this.put(
            `projects/${projectId}/cases/${caseId}/onshore-related-opex-cost-profile/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createAdditionalOPEXCostProfile(
        projectId: string,
        caseId: string,
        dto: Components.Schemas.CreateAdditionalOPEXCostProfileDto,
    ): Promise<Components.Schemas.AdditionalOPEXCostProfileDto> {
        const res: Components.Schemas.AdditionalOPEXCostProfileDto = await this.post(
            `projects/${projectId}/cases/${caseId}/additional-opex-cost-profile/`,
            { body: dto },
        )
        return res
    }

    public async updateAdditionalOPEXCostProfile(
        projectId: string,
        caseId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateAdditionalOPEXCostProfileDto,
    ): Promise<Components.Schemas.AdditionalOPEXCostProfileDto> {
        const res: Components.Schemas.AdditionalOPEXCostProfileDto = await this.put(
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
