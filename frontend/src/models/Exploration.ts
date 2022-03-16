export class Exploration {
    id: string | null
    projectId: string | null
    name: string | null
    wellType: WellType | null
    costProfile: ExplorationCostProfileDto | null
    drillingSchedule: ExplorationDrillingScheduleDto | null
    gAndGAdminCost: GAndGAdminCostDto | null
    rigMobDemob: number | null

    constructor(data: Components.Schemas.ExplorationDto) {
        this.id = data.id ?? null
        this.projectId = data.projectId ?? null
        this.name = data.name ?? null
        this.wellType = data.wellType ?? null
        this.costProfile = data.costProfile ?? null
        this.drillingSchedule = data.drillingSchedule ?? null
        this.gAndGAdminCost = data.gAndGAdminCost ?? null
        this.rigMobDemob = data.rigMobDemob ?? null
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }

}

export interface ExplorationCostProfileDtoConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}

export class ExplorationCostProfileDto {
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null 

    constructor(data: ExplorationCostProfileDtoConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: ExplorationCostProfileDtoConstructor): ExplorationCostProfileDto {
        return new ExplorationCostProfileDto(data)
    }
}

//asd

export interface ExplorationDrillingScheduleDtoConstructor {
    startYear?: number; // int32
    values?: number /* int32 */[] | null;
}

export class ExplorationDrillingScheduleDto {
    startYear?: number | null
    values?: number [] | null

    constructor(data: ExplorationDrillingScheduleDto) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
    }

    static fromJSON(data: ExplorationDrillingScheduleDto): ExplorationDrillingScheduleDto {
        return new ExplorationDrillingScheduleDto(data)
    }
}

//

export interface GAndGAdminCostDtoConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}


export class GAndGAdminCostDto {
    startYear?: number | null
    values?: number [] | null

    constructor(data: GAndGAdminCostDtoConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
    }

    static fromJSON(data: GAndGAdminCostDtoConstructor): GAndGAdminCostDto {
        return new GAndGAdminCostDto(data)
    }
}


export type WellType = 0 | 1; // int32
export type Currency = 0 | 1; // int32