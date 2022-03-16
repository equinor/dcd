export class Wellproject {

    id: string | null
    name: string | null
    projectId: string | null
    costProfile: WellProjectCostProfileDto | null
    drillingSchedule: DrillingScheduleDto | null
    producerCount: number | null
    gasInjectorCount: number | null
    waterInjectorCount: number | null
    artificialLift: number | null
    rigMobDemob: number | null
    annualWellInterventionCost: number | null
    pluggingAndAbandonment: number | null
    
    constructor(data: Components.Schemas.WellProjectDto) {
        this.id = data.id ?? null
        this.name = data.name ?? null
        this.projectId = data.projectId ?? null
        this.costProfile = data.costProfile ?? null
        this.drillingSchedule = data.drillingSchedule ?? null
        this.producerCount = data.producerCount ?? null
        this.gasInjectorCount = data.gasInjectorCount ?? null
        this.waterInjectorCount = data.waterInjectorCount ?? null
        this.artificialLift = data.artificialLift ?? null
        this.rigMobDemob = data.rigMobDemob ?? null
        this.annualWellInterventionCost = data.annualWellInterventionCost ?? null
        this.pluggingAndAbandonment = data.pluggingAndAbandonment ?? null
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): Wellproject {
        return new Wellproject(data)
    }

}



//Wellproject
export interface WellProjectCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}

export class WellProjectCostProfileDto { 
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null

    constructor(data: WellProjectCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: WellProjectCostProfileConstructor): WellProjectCostProfileDto {
        return new WellProjectCostProfileDto(data)
    }

}

export interface DrillingScheduleCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* int32 */[] | null;
}

export class DrillingScheduleDto {
    startYear?: number | null
    values?: number [] | null

    constructor(data: DrillingScheduleCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
    }

    static fromJSON(data: DrillingScheduleCostProfileConstructor): DrillingScheduleDto {
        return new DrillingScheduleDto(data)
    }

}

export type WellType = 0 | 1; // int32
export type Currency = 0 | 1; // int32
