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
        dto: Components.Schemas.CreateProductionProfileOilDto,
    ): Promise<Components.Schemas.ProductionProfileOilDto> {
        const res: Components.Schemas.ProductionProfileOilDto = await this.post(
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
        dto: Components.Schemas.UpdateProductionProfileOilDto,
    ): Promise<Components.Schemas.ProductionProfileOilDto> {
        const res: Components.Schemas.ProductionProfileOilDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-oil/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileGas(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateProductionProfileGasDto,
    ): Promise<Components.Schemas.ProductionProfileGasDto> {
        const res: Components.Schemas.ProductionProfileGasDto = await this.post(
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
        dto: Components.Schemas.UpdateProductionProfileGasDto,
    ): Promise<Components.Schemas.ProductionProfileGasDto> {
        const res: Components.Schemas.ProductionProfileGasDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-gas/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileWater(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateProductionProfileWaterDto,
    ): Promise<Components.Schemas.ProductionProfileWaterDto> {
        const res: Components.Schemas.ProductionProfileWaterDto = await this.post(
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
        dto: Components.Schemas.UpdateProductionProfileWaterDto,
    ): Promise<Components.Schemas.ProductionProfileWaterDto> {
        const res: Components.Schemas.ProductionProfileWaterDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileWaterInjection(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateProductionProfileWaterInjectionDto,
    ): Promise<Components.Schemas.ProductionProfileWaterInjectionDto> {
        const res: Components.Schemas.ProductionProfileWaterInjectionDto = await this.post(
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
        dto: Components.Schemas.UpdateProductionProfileWaterInjectionDto,
    ): Promise<Components.Schemas.ProductionProfileWaterInjectionDto> {
        const res: Components.Schemas.ProductionProfileWaterInjectionDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/production-profile-water-injection/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileFuelFlaringAndLossesOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateFuelFlaringAndLossesOverrideDto,
    ): Promise<Components.Schemas.FuelFlaringAndLossesOverrideDto> {
        const res: Components.Schemas.FuelFlaringAndLossesOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateFuelFlaringAndLossesOverrideDto,
    ): Promise<Components.Schemas.FuelFlaringAndLossesOverrideDto> {
        const res: Components.Schemas.FuelFlaringAndLossesOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/fuel-flaring-and-losses-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileNetSalesGasOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateNetSalesGasOverrideDto,
    ): Promise<Components.Schemas.NetSalesGasOverrideDto> {
        const res: Components.Schemas.NetSalesGasOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateNetSalesGasOverrideDto,
    ): Promise<Components.Schemas.NetSalesGasOverrideDto> {
        const res: Components.Schemas.NetSalesGasOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/net-sales-gas-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileCo2EmissionsOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateCo2EmissionsOverrideDto,
    ): Promise<Components.Schemas.Co2EmissionsOverrideDto> {
        const res: Components.Schemas.Co2EmissionsOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateCo2EmissionsOverrideDto,
    ): Promise<Components.Schemas.Co2EmissionsOverrideDto> {
        const res: Components.Schemas.Co2EmissionsOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/co2-emissions-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createProductionProfileImportedElectricityOverride(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateImportedElectricityOverrideDto,
    ): Promise<Components.Schemas.ImportedElectricityOverrideDto> {
        const res: Components.Schemas.ImportedElectricityOverrideDto = await this.post(
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
        dto: Components.Schemas.UpdateImportedElectricityOverrideDto,
    ): Promise<Components.Schemas.ImportedElectricityOverrideDto> {
        const res: Components.Schemas.ImportedElectricityOverrideDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/imported-electricity-override/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createDeferredOilProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateDeferredOilProductionDto,
    ): Promise<Components.Schemas.DeferredOilProductionDto> {
        const res: Components.Schemas.DeferredOilProductionDto = await this.post(
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
        dto: Components.Schemas.UpdateDeferredOilProductionDto,
    ): Promise<Components.Schemas.DeferredOilProductionDto> {
        const res: Components.Schemas.DeferredOilProductionDto = await this.put(
            `projects/${projectId}/cases/${caseId}/drainage-strategies/${drainageStrategyId}/deferred-oil-production/${productionProfileId}`,
            { body: dto },
        )
        return res
    }

    public async createDeferredGasProduction(
        projectId: string,
        caseId: string,
        drainageStrategyId: string,
        dto: Components.Schemas.CreateDeferredGasProductionDto,
    ): Promise<Components.Schemas.DeferredGasProductionDto> {
        const res: Components.Schemas.DeferredGasProductionDto = await this.post(
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
        dto: Components.Schemas.UpdateDeferredGasProductionDto,
    ): Promise<Components.Schemas.DeferredGasProductionDto> {
        const res: Components.Schemas.DeferredGasProductionDto = await this.put(
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
