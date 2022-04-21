export class SurfCessationCostProfile implements Components.Schemas.SurfCessationCostProfileDto {
    id?: string
    startYear: number
    values: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.SurfCessationCostProfileDto) {
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

    static fromJSON(data?: Components.Schemas.SurfCessationCostProfileDto): SurfCessationCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new SurfCessationCostProfile(data)
    }
}
