import { AnnualProfile } from "./AnnualProfile"
import { ArtificialLift } from "./ArtificialLift"

const rowTitles = [
    "Production profile oil",
    "Production profile gas",
    "Production profile water",
    "Production profile water injection",
    "Fuel flaring and losses",
    "Net sales gas",
    "CO2 emissions",
]

export class DrainageStrategy {
    artificialLift: ArtificialLift | null
    co2Emissions: AnnualProfile | null
    description: string | null
    fuelFlaringAndLosses?: AnnualProfile | null
    gasInjectorCount: number | null
    id: string
    name: string
    netSalesGas: AnnualProfile | null
    nglYield: number | null
    producerCount: number | null
    productionProfileGas?: AnnualProfile | null
    productionProfileOil?: AnnualProfile | null
    productionProfileWater?: AnnualProfile | null
    productionProfileWaterInjection?: AnnualProfile | null
    projectId: string | null
    waterInjectorCount: number | null

    constructor(data: Components.Schemas.DrainageStrategyDto) {
        this.artificialLift = data.artificialLift
            ? new ArtificialLift(data.artificialLift)
            : null
        this.co2Emissions = data.co2Emissions
            ? new AnnualProfile(data.co2Emissions)
            : null
        this.description = data.description ?? null
        this.fuelFlaringAndLosses = data.fuelFlaringAndLosses
            ? new AnnualProfile(data.fuelFlaringAndLosses)
            : null
        this.gasInjectorCount = data.gasInjectorCount ?? null
        this.id = data.id ?? ""
        this.name = data.name ?? ""
        this.netSalesGas = data.netSalesGas
            ? new AnnualProfile(data.netSalesGas)
            : null
        this.nglYield = data.nglYield ?? null
        this.producerCount = data.producerCount ?? null
        this.productionProfileGas = data.productionProfileGas
            ? new AnnualProfile(data.productionProfileGas)
            : null
        this.productionProfileOil = data.productionProfileOil
            ? new AnnualProfile(data.productionProfileOil)
            : null
        this.productionProfileWater = data.productionProfileWater
            ? new AnnualProfile(data.productionProfileWater)
            : null
        this.productionProfileWaterInjection = data.productionProfileWaterInjection
            ? new AnnualProfile(data.productionProfileWaterInjection)
            : null
        this.projectId = data.projectId ?? null
        this.waterInjectorCount = data.waterInjectorCount ?? null
    }

    static fromJSON(data: Components.Schemas.DrainageStrategyDto): DrainageStrategy {
        return new DrainageStrategy(data)
    }
}
