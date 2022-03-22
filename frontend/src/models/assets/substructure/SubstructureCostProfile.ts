export class SubstructureCostProfile implements Components.Schemas.SubstructureCostProfileDto {
    id?: string | undefined
    startYear?: number
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency
    sum?: number

    constructor(data: Components.Schemas.SubstructureCostProfileDto) {
        this.id = data.id
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data?: Components.Schemas.SubstructureCostProfileDto): SubstructureCostProfile | undefined {
        if (data !== undefined) {
            return new SubstructureCostProfile(data)
        }
        return undefined
    }
}
