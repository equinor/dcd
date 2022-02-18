import { AnnualProfile } from "./AnnualProfile"
import { ArtificialLift } from "./ArtificialLift"

type DrainageStrategyConstructor = {
    artificialLift: number
    co2Emissions: any
    description: string
    fuelFlaringAndLosses: any
    gasInjectorCount: number
    id: string
    name: string
    netSalesGas: any
    nglYield: number
    producerCount: number
    productionProfileGas?: any
    productionProfileOil?: any
    productionProfileWater?: any
    productionProfileWaterInjection?: any
    projectId: string
    waterInjectorCount: number
}

export class DrainageStrategy {
    artificialLift: ArtificialLift
    co2Emissions?: AnnualProfile | null
    description: string
    fuelFlaringAndLosses?: AnnualProfile | null
    gasInjectorCount: number
    id: string
    name: string
    netSalesGas?: AnnualProfile | null
    nglYield: number
    producerCount: number
    productionProfileGas?: AnnualProfile | null
    productionProfileOil?: AnnualProfile | null
    productionProfileWater?: AnnualProfile | null
    productionProfileWaterInjection?: AnnualProfile | null
    projectId: string
    waterInjectorCount: number

    constructor(data: DrainageStrategyConstructor) {
        this.artificialLift = new ArtificialLift(data.artificialLift)
        this.co2Emissions = data.co2Emissions ? new AnnualProfile(data.co2Emissions) : null
        this.description = data.description
        this.fuelFlaringAndLosses = data.fuelFlaringAndLosses ? new AnnualProfile(data.fuelFlaringAndLosses) : null
        this.gasInjectorCount = data.gasInjectorCount
        this.id = data.id
        this.name = data.name
        this.netSalesGas = data.netSalesGas
        this.nglYield = data.nglYield
        this.producerCount = data.producerCount
        this.productionProfileGas = data.productionProfileGas ? new AnnualProfile(data.productionProfileGas) : null
        this.productionProfileOil = data.productionProfileOil ? new AnnualProfile(data.productionProfileOil) : null
        this.productionProfileWater = data.productionProfileWater ? new AnnualProfile(data.productionProfileWater) : null
        this.productionProfileWaterInjection = data.productionProfileWaterInjection ? new AnnualProfile(data.productionProfileWaterInjection) : null
        this.projectId = data.projectId
        this.waterInjectorCount = data.waterInjectorCount
    }

    static fromJSON(data: DrainageStrategyConstructor): DrainageStrategy {
        return new DrainageStrategy(data)
    }
}
