import { ITimeSeries } from "../../ITimeSeries"

export class SurfCessationCostProfile implements Components.Schemas.SurfCessationCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.SurfCessationCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Cessation cost profile"
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Cessation cost profile"
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
