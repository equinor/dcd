export class ExplorationCostProfile implements Components.Schemas.ExplorationCostProfileDto {
    id?: string
    startYear: number
    values: number []
    epaVersion?: string
    currency?: Components.Schemas.Currency
    sum?: number

    constructor(data?: Components.Schemas.ExplorationCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJSON(data?: Components.Schemas.ExplorationCostProfileDto): ExplorationCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ExplorationCostProfile(data)
    }
}
