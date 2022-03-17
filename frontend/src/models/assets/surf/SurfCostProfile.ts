export class SurfCostProfile implements Components.Schemas.SurfCostProfileDto {
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.SurfCostProfileDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
        this.epaVersion = data?.epaVersion ?? null
        this.currency = data?.currency
        this.sum = data?.sum
    }

    static fromJSON(data?: Components.Schemas.SurfCostProfileDto): SurfCostProfile {
        return new SurfCostProfile(data)
    }
}
