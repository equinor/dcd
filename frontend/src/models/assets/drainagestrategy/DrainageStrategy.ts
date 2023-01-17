import { NetSalesGas } from "./NetSalesGas"
import { Co2Emissions } from "./Co2Emissions"
import { FuelFlaringAndLosses } from "./FuelFlaringAndLosses"
import { ProductionProfileGas } from "./ProductionProfileGas"
import { ProductionProfileOil } from "./ProductionProfileOil"
import { ProductionProfileWater } from "./ProductionProfileWater"
import { ProductionProfileWaterInjection } from "./ProductionProfileWaterInjection"
import { ProductionProfileNGL } from "./ProductionProfileNGL"
import { IAsset } from "../IAsset"
import { EMPTY_GUID } from "../../../Utils/constants"
import { ImportedElectricity } from "./ImportedElectricity"
import { ImportedElectricityOverride } from "./ImportedElectricityOverride"
import { Co2EmissionsOverride } from "./Co2EmissionsOverride"

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
    gasSolution?: Components.Schemas.GasSolution /* int32 */
    netSalesGas?: NetSalesGas | undefined
    co2Emissions?: Co2Emissions | undefined
    co2EmissionsOverride?: Co2EmissionsOverride | undefined
    fuelFlaringAndLosses?: FuelFlaringAndLosses | undefined
    productionProfileGas?: ProductionProfileGas | undefined
    productionProfileOil?: ProductionProfileOil | undefined
    productionProfileWater?: ProductionProfileWater | undefined
    productionProfileWaterInjection?: ProductionProfileWaterInjection | undefined
    productionProfileNGL?: ProductionProfileNGL | undefined
    importedElectricity?: ImportedElectricity
    importedElectricityOverride?: ImportedElectricityOverride
    hasChanges?: boolean

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
            this.gasSolution = data.gasSolution
            this.netSalesGas = NetSalesGas.fromJson(data.netSalesGas)
            this.co2Emissions = Co2Emissions.fromJson(data.co2Emissions)
            this.co2EmissionsOverride = Co2EmissionsOverride.fromJson(data.co2Emissions)
            this.fuelFlaringAndLosses = FuelFlaringAndLosses.fromJson(data.fuelFlaringAndLosses)
            this.productionProfileGas = ProductionProfileGas.fromJson(data.productionProfileGas)
            this.productionProfileOil = ProductionProfileOil.fromJson(data.productionProfileOil)
            this.productionProfileWater = ProductionProfileWater.fromJson(data.productionProfileWater)
            this.productionProfileNGL = ProductionProfileNGL.fromJson(data.productionProfileNGL)
            this.productionProfileWaterInjection = ProductionProfileWaterInjection
                .fromJson(data.productionProfileWaterInjection)
            this.importedElectricity = ImportedElectricity.fromJson(data.importedElectricity)
            this.importedElectricityOverride = ImportedElectricityOverride.fromJson(data.importedElectricityOverride)
        } else {
            this.id = EMPTY_GUID
            this.name = ""
            this.description = ""
        }
    }

    static fromJSON(data: Components.Schemas.DrainageStrategyDto): DrainageStrategy {
        return new DrainageStrategy(data)
    }
}
