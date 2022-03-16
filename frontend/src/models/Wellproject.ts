import { Schema } from "inspector"

export class Wellproject implements Components.Schemas.WellProjectDto{

    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: WellProjectCostProfileDto | undefined
    drillingSchedule?: Components.Schemas.DrillingScheduleDto | undefined
    producerCount?: number | undefined
    gasInjectorCount?: number | undefined
    waterInjectorCount?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    rigMobDemob?: number | undefined
    annualWellInterventionCost?: number | undefined
    pluggingAndAbandonment?: number | undefined
    
    constructor(data: Components.Schemas.WellProjectDto) {
        this.id = data.id
        this.name = data.name ?? null
        this.projectId = data.projectId ?? ""
        this.costProfile = data.costProfile
        this.drillingSchedule = data.drillingSchedule
        this.producerCount = data.producerCount ?? 0
        this.gasInjectorCount = data.gasInjectorCount ?? 0
        this.waterInjectorCount = data.waterInjectorCount ?? 0
        this.artificialLift = data.artificialLift ?? 0
        this.rigMobDemob = data.rigMobDemob ?? 0
        this.annualWellInterventionCost = data.annualWellInterventionCost ?? 0
        this.pluggingAndAbandonment = data.pluggingAndAbandonment ?? 0
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): Wellproject {
        return new Wellproject(data)
    }

}

export class WellProjectCostProfileDto implements Components.Schemas.WellProjectCostProfileDto { 
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data: Components.Schemas.WellProjectCostProfileDto) {
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data: Components.Schemas.WellProjectCostProfileDto): WellProjectCostProfileDto {
        return new WellProjectCostProfileDto(data)
    }

}

export class DrillingScheduleDto implements Components.Schemas.DrillingScheduleDto{
    startYear?: number | undefined
    values?: number [] | null

    constructor(data: Components.Schemas.DrillingScheduleDto) {
        this.startYear = data.startYear ?? undefined
        this.values = data.values ?? []
    }

    static fromJSON(data: Components.Schemas.DrillingScheduleDto): DrillingScheduleDto {
        return new DrillingScheduleDto(data)
    }

}