export class Exploration implements Components.Schemas.ExplorationDto {
    id?: string | undefined
    projectId?: string | undefined
    name?: string | undefined
    wellType?: Components.Schemas.WellType | undefined
    costProfile?: ExplorationCostProfileDto | undefined
    drillingSchedule?: ExplorationDrillingScheduleDto | undefined
    gAndGAdminCost?: GAndGAdminCostDto | undefined
    rigMobDemob?: number | undefined

    constructor(data: Components.Schemas.ExplorationDto) {
        this.id = data.id
        this.projectId = data.projectId
        this.name = data.name ?? ""
        this.wellType = data.wellType
        this.costProfile = ExplorationCostProfileDto.fromJSON(data.costProfile)
        this.drillingSchedule = ExplorationDrillingScheduleDto.fromJSON(data.drillingSchedule)
        this.gAndGAdminCost = GAndGAdminCostDto.fromJSON(data.gAndGAdminCost)
        this.rigMobDemob = data.rigMobDemob
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }

}

export class ExplorationCostProfileDto implements Components.Schemas.ExplorationCostProfileDto {
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined 

    constructor(data?: Components.Schemas.ExplorationCostProfileDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
        this.epaVersion = data?.epaVersion ?? null
        this.currency = data?.currency
        this.sum = data?.sum
    }

    static fromJSON(data?: Components.Schemas.ExplorationCostProfileDto): ExplorationCostProfileDto {
        return new ExplorationCostProfileDto(data)
    }
}

export class ExplorationDrillingScheduleDto implements Components.Schemas.ExplorationDrillingScheduleDto {
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.ExplorationDrillingScheduleDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.ExplorationDrillingScheduleDto): ExplorationDrillingScheduleDto {
        return new ExplorationDrillingScheduleDto(data)
    }
}

export class GAndGAdminCostDto implements Components.Schemas.GAndGAdminCostDto {
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.GAndGAdminCostDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.GAndGAdminCostDto): GAndGAdminCostDto {
        return new GAndGAdminCostDto(data)
    }
}
