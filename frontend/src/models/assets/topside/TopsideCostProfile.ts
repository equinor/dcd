export class TopsideCostProfile implements Components.Schemas.TopsideCostProfileDto {
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.TopsideCostProfileDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
        this.epaVersion = data?.epaVersion ?? null
        this.currency = data?.currency
        this.sum = data?.sum
    }

    static fromJSON(data?: Components.Schemas.TopsideCostProfileDto): TopsideCostProfile {
        return new TopsideCostProfile(data)
    }
}
