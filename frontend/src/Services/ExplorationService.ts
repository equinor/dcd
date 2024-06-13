import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class ExplorationService extends __BaseService {
    public async updateExploration(
        projectId: string,
        caseId: string,
        explorationId: string,
        dto: Components.Schemas.UpdateExplorationDto,
    ): Promise<Components.Schemas.ExplorationDto> {
        const res: Components.Schemas.ExplorationDto = await this.put(
            `projects/${projectId}/cases/${caseId}/explorations/${explorationId}`,
            { body: dto },
        )
        return res
    }

    public async createSeismicAcquisitionAndProcessing(
        projectId: string,
        caseId: string,
        explorationId: string,
        dto: Components.Schemas.CreateSeismicAcquisitionAndProcessingDto,
    ): Promise<Components.Schemas.SeismicAcquisitionAndProcessingDto> {
        const res: Components.Schemas.SeismicAcquisitionAndProcessingDto = await this.post(
            `projects/${projectId}/cases/${caseId}/explorations/${explorationId}/seismic-acquisition-and-processing/`,
            { body: dto },
        )
        return res
    }

    public async updateSeismicAcquisitionAndProcessing(
        projectId: string,
        caseId: string,
        explorationId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateSeismicAcquisitionAndProcessingDto,
    ): Promise<Components.Schemas.SeismicAcquisitionAndProcessingDto> {
        const res: Components.Schemas.SeismicAcquisitionAndProcessingDto = await this.put(
            `projects/${projectId}/cases/${caseId}/explorations/${explorationId}/seismic-acquisition-and-processing/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createCountryOfficeCost(
        projectId: string,
        caseId: string,
        topsideId: string,
        dto: Components.Schemas.CreateCountryOfficeCostDto,
    ): Promise<Components.Schemas.CountryOfficeCostDto> {
        const res: Components.Schemas.CountryOfficeCostDto = await this.post(
            `projects/${projectId}/cases/${caseId}/explorations/${topsideId}/country-office-cost/`,
            { body: dto },
        )
        return res
    }

    public async updateCountryOfficeCost(
        projectId: string,
        caseId: string,
        topsideId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateCountryOfficeCostDto,
    ): Promise<Components.Schemas.CountryOfficeCostDto> {
        const res: Components.Schemas.CountryOfficeCostDto = await this.put(
            `projects/${projectId}/cases/${caseId}/explorations/${topsideId}/country-office-cost/${costProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetExplorationService = async () => new ExplorationService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
