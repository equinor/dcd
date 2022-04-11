export class SubstructureCessasionCostProfile implements Components.Schemas.SubstructureCessasionCostProfileDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.SubstructureCessasionCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.epaVersion = data?.epaVersion ?? ""
            this.currency = data?.currency
            this.sum = data?.sum
        }
    }

    static fromJSON(data?: Components.Schemas.SubstructureCessasionCostProfileDto):
        SubstructureCessasionCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new SubstructureCessasionCostProfile(data)
    }
}
