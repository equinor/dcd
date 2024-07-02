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
        dto: Components.Schemas.CreateOilProducerCostProfileOverrideDto,
    ): Promise<Components.Schemas.OilProducerCostProfileOverrideDto> {
        const res: Components.Schemas.OilProducerCostProfileOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateOilProducerCostProfileOverrideDto,
    ): Promise<Components.Schemas.OilProducerCostProfileOverrideDto> {
        const res: Components.Schemas.OilProducerCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/oil-producer-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createGasProducerCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateGasProducerCostProfileOverrideDto,
    ): Promise<Components.Schemas.GasProducerCostProfileOverrideDto> {
        const res: Components.Schemas.GasProducerCostProfileOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateGasProducerCostProfileOverrideDto,
    ): Promise<Components.Schemas.GasProducerCostProfileOverrideDto> {
        const res: Components.Schemas.GasProducerCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/gas-producer-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createWaterInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateWaterInjectorCostProfileOverrideDto,
    ): Promise<Components.Schemas.WaterInjectorCostProfileOverrideDto> {
        const res: Components.Schemas.WaterInjectorCostProfileOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateWaterInjectorCostProfileOverrideDto,
    ): Promise<Components.Schemas.WaterInjectorCostProfileOverrideDto> {
        const res: Components.Schemas.WaterInjectorCostProfileOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/well-projects/${wellProjectId}/water-injector-cost-profile-override/${costProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createGasInjectorCostProfileOverride(
        projectId: string,
        caseId: string,
        wellProjectId: string,
        dto: Components.Schemas.CreateGasInjectorCostProfileOverrideDto,
    ): Promise<Components.Schemas.GasInjectorCostProfileOverrideDto> {
        const res: Components.Schemas.GasInjectorCostProfileOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateGasInjectorCostProfileOverrideDto,
    ): Promise<Components.Schemas.GasInjectorCostProfileOverrideDto> {
        const res: Components.Schemas.GasInjectorCostProfileOverrideDto = await this.put(
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
        dto: Components.Schemas.CreateDrillingScheduleDto,
    ): Promise<Components.Schemas.DrillingScheduleDto> {
        const res: Components.Schemas.CountryOfficeCostDto = await this.post(
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
        dto: Components.Schemas.UpdateDrillingScheduleDto,
    ): Promise<Components.Schemas.DrillingScheduleDto> {
        const res: Components.Schemas.CountryOfficeCostDto = await this.put(
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
