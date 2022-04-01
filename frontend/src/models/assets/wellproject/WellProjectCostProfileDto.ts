export class WellProjectCostProfileDto implements Components.Schemas.WellProjectCostProfileDto {
    id?: string
    startYear: number | undefined
    values?: number []
    epaVersion?: string
    currency?: Components.Schemas.Currency
    sum?: number

    constructor(data?: Components.Schemas.WellProjectCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.epaVersion = data?.epaVersion ?? ""
            this.currency = data?.currency
            this.sum = data?.sum
        }
    }

    static fromJSON(data?: Components.Schemas.WellProjectCostProfileDto): WellProjectCostProfileDto | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new WellProjectCostProfileDto(data)
    }
}
