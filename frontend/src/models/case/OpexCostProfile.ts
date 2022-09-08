import { ITimeSeries } from "../ITimeSeries"

export class OpexCostProfile implements Components.Schemas.OpexCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.OpexCostProfileDto) {
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

    static fromJSON(data?: Components.Schemas.OpexCostProfileDto): OpexCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new OpexCostProfile(data)
    }
}
