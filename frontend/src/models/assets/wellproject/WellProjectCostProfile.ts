import { ITimeSeries } from "../../ITimeSeries"

export class WellProjectCostProfile implements Components.Schemas.WellProjectCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined
    override?: boolean

    constructor(data?: Components.Schemas.WellProjectCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
            this.epaVersion = ""
        }
    }

    static fromJSON(data?: Components.Schemas.WellProjectCostProfileDto): WellProjectCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new WellProjectCostProfile(data)
    }
}
