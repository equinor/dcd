import { ITimeSeries } from "../../ITimeSeries"

export class TopsideCessationCostProfile implements Components.Schemas.TopsideCessationCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
    source?: Components.Schemas.Source | undefined

    constructor(data?: Components.Schemas.TopsideCessationCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
            this.source = data.source
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJSON(data?: Components.Schemas.TopsideCessationCostProfileDto): TopsideCessationCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new TopsideCessationCostProfile(data)
    }
}
