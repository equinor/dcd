export class ExplorationCostProfile implements Components.Schemas.ExplorationCostProfileDto {
    id?: string | undefined
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.ExplorationCostProfileDto) {
        this.id = data?.id
        this.startYear = data?.startYear
        this.values = data?.values ?? []
        this.epaVersion = data?.epaVersion ?? null
        this.currency = data?.currency
        this.sum = data?.sum
    }

    static fromJSON(data?: Components.Schemas.ExplorationCostProfileDto): ExplorationCostProfile {
        return new ExplorationCostProfile(data)
    }
}
