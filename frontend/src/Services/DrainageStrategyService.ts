import { __BaseService } from "./__BaseService"
import { config } from "./config"
import { getToken, loginAccessTokenKey } from "../Utils/common"

class DrainageStrategyService extends __BaseService {
    public async updateDrainageStrategy(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.UpdateDrainageStrategyDto,
    ): Promise<Components.Schemas.DrainageStrategyDto> {
        const res: Components.Schemas.DrainageStrategyDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileOil(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-oil/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileOil(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-oil/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createAdditionalProductionProfileOil(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/additional-production-profile-oil/`,
            { body: dto },
        )
        return res
    }

    public async updateAdditionalProductionProfileOil(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/additional-production-profile-oil/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileGas(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-gas/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileGas(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-gas/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createAdditionalProductionProfileGas(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/additional-production-profile-gas/`,
            { body: dto },
        )
        return res
    }

    public async updateAdditionalProductionProfileGas(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/additional-production-profile-gas/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileWater(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileWater(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileWaterInjection(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water-injection/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileWaterInjection(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water-injection/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileFuelFlaringAndLossesOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeOverrideDto> {
        const res: Components.Schemas.TimeSeriesVolumeOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/fuel-flaring-and-losses-override/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileFuelFlaringAndLossesOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeOverrideDto> {
        const res: Components.Schemas.TimeSeriesVolumeOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/fuel-flaring-and-losses-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileNetSalesGasOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeOverrideDto> {
        const res: Components.Schemas.TimeSeriesVolumeOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/net-sales-gas-override/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileNetSalesGasOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeOverrideDto> {
        const res: Components.Schemas.TimeSeriesVolumeOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/net-sales-gas-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileCo2EmissionsOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesMassOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesMassOverrideDto> {
        const res: Components.Schemas.TimeSeriesMassOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/co2-emissions-override/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileCo2EmissionsOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesMassOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesMassOverrideDto> {
        const res: Components.Schemas.TimeSeriesMassOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/co2-emissions-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileImportedElectricityOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesEnergyDto,
    ): Promise<Components.Schemas.TimeSeriesEnergyOverrideDto> {
        const res: Components.Schemas.TimeSeriesEnergyOverrideDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/imported-electricity-override/`,
            { body: dto },
        )
        return res
    }

    public async updateProductionProfileImportedElectricityOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesEnergyOverrideDto,
    ): Promise<Components.Schemas.TimeSeriesEnergyOverrideDto> {
        const res: Components.Schemas.TimeSeriesEnergyOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/imported-electricity-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createDeferredOilProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/deferred-oil-production/`,
            { body: dto },
        )
        return res
    }

    public async updateDeferredOilProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/deferred-oil-production/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createDeferredGasProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.post(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/deferred-gas-production/`,
            { body: dto },
        )
        return res
    }

    public async updateDeferredGasProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        productionProfileId: string,
        dto: Components.Schemas.UpdateTimeSeriesVolumeDto,
    ): Promise<Components.Schemas.TimeSeriesVolumeDto> {
        const res: Components.Schemas.TimeSeriesVolumeDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/deferred-gas-production/${productionProfileId}`,
            { body: dto },
        )
        return res
    }
}

export const GetDrainageStrategyService = async () => new DrainageStrategyService({
    ...config.BaseUrl,
    accessToken: await getToken(loginAccessTokenKey)!,
})
