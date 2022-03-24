export class TopsideCostProfile implements Components.Schemas.TopsideCostProfileDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.TopsideCostProfileDto) {
        if (data !== undefined) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.epaVersion = data?.epaVersion ?? null
            this.currency = data?.currency
            this.sum = data?.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.epaVersion = ""
        }
    }

    static fromJSON(data?: Components.Schemas.TopsideCostProfileDto): TopsideCostProfile {
        return new TopsideCostProfile(data)
    }
}
