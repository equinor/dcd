import { NetSalesGasCostProfile } from "./NetSalesGasCostProfile"
import { Co2EmissionsCostProfile } from "./Co2EmissionsCostProfile"
import { FuelFlaringAndLossesCostProfile } from "./FuelFlaringAndLossesCostProfile"
import { ProductionProfileGasCostProfile } from "./ProductionProfileGasCostProfile"
import { ProductionProfileOilCostProfile } from "./ProductionProfileOilCostProfile"
import { ProductionProfileWaterCostProfile } from "./ProductionProfileWaterCostProfile"
import { ProductionProfileWaterInjectionCostProfile } from "./ProductionProfileWaterInjectionCostProfile"
import { IAsset } from "../IAsset"

export class DrainageStrategy implements Components.Schemas.DrainageStrategyDto, IAsset {
    id?: string
    projectId?: string
    name?: string | undefined
    description?: string | null
    waterInjectorCount?: number
    nglYield?: number
    producerCount?: number
    gasInjectorCount?: number
    artificialLift?: Components.Schemas.ArtificialLift
    netSalesGas?: NetSalesGasCostProfile | undefined
    co2Emissions?: Co2EmissionsCostProfile | undefined
    fuelFlaringAndLosses?: FuelFlaringAndLossesCostProfile | undefined
    productionProfileGas?: ProductionProfileGasCostProfile | undefined
    productionProfileOil?: ProductionProfileOilCostProfile | undefined
    productionProfileWater?: ProductionProfileWaterCostProfile | undefined
    productionProfileWaterInjection?: ProductionProfileWaterInjectionCostProfile | undefined

    constructor(data?: Components.Schemas.DrainageStrategyDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId
            this.name = data.name ?? ""
            this.description = data.description
            this.waterInjectorCount = data.waterInjectorCount
            this.nglYield = data.nglYield
            this.gasInjectorCount = data.gasInjectorCount
            this.producerCount = data.producerCount
            this.artificialLift = data.artificialLift
            this.netSalesGas = NetSalesGasCostProfile.fromJson(data.netSalesGas)
            this.co2Emissions = Co2EmissionsCostProfile.fromJson(data.co2Emissions)
            this.fuelFlaringAndLosses = FuelFlaringAndLossesCostProfile.fromJson(data.fuelFlaringAndLosses)
            this.productionProfileGas = ProductionProfileGasCostProfile.fromJson(data.productionProfileGas)
            this.productionProfileOil = ProductionProfileOilCostProfile.fromJson(data.productionProfileOil)
            this.productionProfileWater = ProductionProfileWaterCostProfile.fromJson(data.productionProfileWater)
            this.productionProfileWaterInjection = ProductionProfileWaterInjectionCostProfile
                .fromJson(data.productionProfileWaterInjection)
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
            this.description = ""
        }
    }

    static fromJSON(data: Components.Schemas.DrainageStrategyDto): DrainageStrategy {
        return new DrainageStrategy(data)
    }
}
