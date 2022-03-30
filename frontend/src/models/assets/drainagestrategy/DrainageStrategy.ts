import { AnnualProfile } from "../../AnnualProfile"
import { ArtificialLift } from "../../ArtificialLift"
import { NetSalesGasCostProfile } from "./NetSalesGasCostProfile"
import { Co2EmissionsCostProfile } from "./Co2EmissionsCostProfile"
import { FuelFlaringAndLossesCostProfile } from "./FuelFlaringAndLossesCostProfile"
import { ProductionProfileGasCostProfile } from "./ProductionProfileGasCostProfile" 
import { ProductionProfileOilCostProfile } from "./ProductionProfileOilCostProfile"
import { ProductionProfileWaterCostProfile } from "./ProductionProfileWaterCostProfile"
import { ProductionProfileWaterInjectionCostProfile } from "./ProductionProfileWaterInjectionCostProfile"

export class DrainageStrategy implements Components.Schemas.DrainageStrategyDto{
    id?: string
    projectId?: string
    name?: string
    description?: string | null
    waterInjectorCount?: number
    nglYield?: number 
    producerCount?: number
    gasInjectorCount?: number
    artificialLift?: Components.Schemas.ArtificialLift
    netSalesGas?: NetSalesGasCostProfile
    co2Emissions?: Co2EmissionsCostProfile
    fuelFlaringAndLosses?: FuelFlaringAndLossesCostProfile
    productionProfileGas?: ProductionProfileGasCostProfile
    productionProfileOil?: ProductionProfileOilCostProfile
    productionProfileWater?: ProductionProfileWaterCostProfile
    productionProfileWaterInjection?: ProductionProfileWaterInjectionCostProfile
    
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
            this.artificialLift = data.artificialLift ?? new ArtificialLift(0)
            this.netSalesGas = NetSalesGasCostProfile.fromJson(data.netSalesGas)
            this.co2Emissions = Co2EmissionsCostProfile.fromJson(data.co2Emissions)
            this.fuelFlaringAndLosses = FuelFlaringAndLossesCostProfile.fromJson(data.fuelFlaringAndLosses)
            this.productionProfileGas = ProductionProfileGasCostProfile.fromJson(data.productionProfileGas)
            this.productionProfileOil = ProductionProfileOilCostProfile.fromJson(data.productionProfileOil)
            this.productionProfileWater = ProductionProfileWaterCostProfile.fromJson(data.productionProfileWater)
            this.productionProfileWaterInjection = ProductionProfileWaterInjectionCostProfile.fromJson(data.productionProfileWaterInjection)
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
            this.artificialLift = new ArtificialLift()
            this.co2Emissions = new Co2EmissionsCostProfile()
            this.fuelFlaringAndLosses = new FuelFlaringAndLossesCostProfile()
            this.netSalesGas = new NetSalesGasCostProfile()
            this.productionProfileGas = new ProductionProfileGasCostProfile()
            this.productionProfileOil = new ProductionProfileOilCostProfile()
            this.productionProfileWater = new ProductionProfileWaterCostProfile()
            this.productionProfileWaterInjection = new ProductionProfileWaterInjectionCostProfile()
        }
    }

    static Copy(data: DrainageStrategy) {
        const drainageStrategyCopy: DrainageStrategy = new DrainageStrategy(data)
        return {
            ...drainageStrategyCopy,
            artificialLift: data.artificialLift,
            co2Emissions: data.co2Emissions,
            fuelFlaringAndLosses: data.fuelFlaringAndLosses,
            netSalesGas: data.netSalesGas,
            productionProfileGas: data.productionProfileGas,
            productionProfileOil: data.productionProfileOil,
            productionProfileWater: data.productionProfileWater,
            productionProfileWaterInjection: data.productionProfileWaterInjection,
        }
    }

    static toDto(data: DrainageStrategy): Components.Schemas.DrainageStrategyDto {
        const drainageStrategyCopy = new DrainageStrategy(data)
        return {
            ...drainageStrategyCopy,
            artificialLift: data.artificialLift,
            co2Emissions: data.co2Emissions,
            fuelFlaringAndLosses: data.fuelFlaringAndLosses,
            netSalesGas: data.netSalesGas,
            productionProfileGas: data.productionProfileGas,
            productionProfileOil: data.productionProfileOil,
            productionProfileWater: data.productionProfileWater,
            productionProfileWaterInjection: data.productionProfileWaterInjection,
        }
    }

    static fromJSON(data: Components.Schemas.DrainageStrategyDto): DrainageStrategy {
        return new DrainageStrategy(data)
    }

}
