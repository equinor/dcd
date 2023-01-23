import { EMPTY_GUID } from "../../../Utils/constants"
import { ITimeSeries } from "../../ITimeSeries"

export class WaterInjectorCostProfile implements Components.Schemas.WaterInjectorCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data?: Components.Schemas.WaterInjectorCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Cost profile"
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
        } else {
            this.id = EMPTY_GUID
            this.startYear = 0
            this.name = "Cost profile"
            this.values = []
            this.epaVersion = ""
        }
    }

    static fromJSON(data?: Components.Schemas.WaterInjectorCostProfileDto): WaterInjectorCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new WaterInjectorCostProfile(data)
    }
}
