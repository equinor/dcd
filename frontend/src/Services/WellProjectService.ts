import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class WellProjectService extends __BaseService {
    public async updateWellProject(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.UpdateWellProjectDto,
    ): Promise<Components.Schemas.WellProjectDto> {
        const res: Components.Schemas.WellProjectDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}`,
            { body: dto },
        )
        return res
    }

    public async createOilProducerCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/oil-producer-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateOilProducerCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/oil-producer-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createGasProducerCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/gas-producer-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateGasProducerCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/gas-producer-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createWaterInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/water-injector-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateWaterInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/water-injector-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createGasInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/gas-injector-cost-profile-override/`,
            { body: dto },
        )
        return res
    }

    public async updateGasInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        costProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesCostOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesCostOverrideDto> {
        const res: Components.Schemas.TimeSeriesCostOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/gas-injector-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createWellProjectWellDrillingSchedule(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        wellId: string,
        dto: Components.Schemas.CreateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.post(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/wells/${wellId}/drilling-schedule/`,
            { body: dto },
        )
        return res
    }

    public async updateWellProjectWellDrillingSchedule(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        wellId: string,
        drillingScheuleId: string,
        dto: Components.Schemas.UpdateTimeSeriesScheduleDto,
    ): Promise<Components.Schemas.TimeSeriesScheduleDto> {
        const res: Components.Schemas.TimeSeriesScheduleDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/wells/${wellId}/drilling-schedule/${drillingScheuleId}`,
            { body: dto },
        )
        return res
    }
}

export const GetWellProjectService = async () => new WellProjectService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
