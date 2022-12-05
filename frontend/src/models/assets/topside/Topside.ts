import { TopsideCostProfile } from "./TopsideCostProfile"
import { TopsideCessationCostProfile } from "./TopsideCessationCostProfile"
import { IAsset } from "../IAsset"
import { EMPTY_GUID } from "../../../Utils/constants"

export class Topside implements Components.Schemas.TopsideDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: TopsideCostProfile | undefined
    cessationCostProfile?: TopsideCessationCostProfile | undefined
    dryWeight?: number | undefined
    oilCapacity?: number | undefined
    waterInjectionCapacity?: number // double
    gasCapacity?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift
    maturity?: Components.Schemas.Maturity | undefined
    currency?: Components.Schemas.Currency
    fuelConsumption?: number | undefined
    flaredGas?: number | undefined
    producerCount?: number | undefined
    gasInjectorCount?: number | undefined
    waterInjectorCount?: number | undefined
    cO2ShareOilProfile?: number | undefined
    cO2ShareGasProfile?: number | undefined
    cO2ShareWaterInjectionProfile?: number | undefined
    cO2OnMaxOilProfile?: number | undefined
    cO2OnMaxGasProfile?: number | undefined
    cO2OnMaxWaterInjectionProfile?: number | undefined
    costYear?: number | undefined
    ProspVersion?: Date | null
    LastChangedDate?: Date | null
    source?: Components.Schemas.Source
    approvedBy?: string | null | undefined
    DG3Date?: Date | null
    DG4Date?: Date | null
    facilityOpex?: number | undefined
    peakElectricityImported?: number // double
    hasChanges?: boolean

    constructor(data?: Components.Schemas.TopsideDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = TopsideCostProfile.fromJSON(data.costProfile)
            this.cessationCostProfile = TopsideCessationCostProfile
                .fromJSON(data.cessationCostProfile)
            this.artificialLift = data.artificialLift ?? 0
            this.dryWeight = data.dryWeight
            this.maturity = data.maturity
            this.oilCapacity = data.oilCapacity
            this.gasCapacity = data.gasCapacity
            this.waterInjectionCapacity = data.waterInjectionCapacity
            this.currency = data.currency ?? 1
            this.fuelConsumption = data.fuelConsumption
            this.flaredGas = data.flaredGas
            this.producerCount = data.producerCount
            this.gasInjectorCount = data.gasInjectorCount
            this.waterInjectorCount = data.waterInjectorCount
            this.cO2ShareOilProfile = data.cO2ShareOilProfile
            this.cO2ShareGasProfile = data.cO2ShareGasProfile
            this.cO2ShareWaterInjectionProfile = data.cO2ShareWaterInjectionProfile
            this.cO2OnMaxOilProfile = data.cO2OnMaxOilProfile
            this.cO2OnMaxGasProfile = data.cO2OnMaxGasProfile
            this.cO2OnMaxWaterInjectionProfile = data.cO2OnMaxWaterInjectionProfile
            this.costYear = data.costYear
            this.ProspVersion = data.prospVersion ? new Date(data.prospVersion) : null
            this.LastChangedDate = data.lastChangedDate ? new Date(data.lastChangedDate) : null
            this.source = data.source
            this.approvedBy = data.approvedBy ?? ""
            this.DG3Date = data.dG3Date ? new Date(data.dG3Date) : null
            this.DG4Date = data.dG4Date ? new Date(data.dG4Date) : null
            this.facilityOpex = data.facilityOpex
            this.peakElectricityImported = data.peakElectricityImported
        } else {
            this.id = EMPTY_GUID
            this.name = ""
            this.approvedBy = ""
        }
    }

    static Copy(data: Topside) {
        const topsideCopy: Topside = new Topside(data)
        return {
            ...topsideCopy,
            ProspVersion: data.ProspVersion,
            DG3Date: data.DG3Date,
            DG4Date: data.DG4Date,
        }
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }
}
