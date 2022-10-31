import { EMPTY_GUID } from "../../../Utils/constants"
import { ITimeSeries } from "../../ITimeSeries"

export class SidetrackCostProfile implements Components.Schemas.SidetrackCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    epaVersion?: string
    currency?: Components.Schemas.Currency
    sum?: number
    override?: boolean

    constructor(data?: Components.Schemas.SidetrackCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Cost profile"
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = EMPTY_GUID
            this.startYear = 0
            this.name = "Cost profile"
            this.values = []
            this.epaVersion = ""
        }
    }

    static fromJSON(data?: Components.Schemas.SidetrackCostProfileDto): SidetrackCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new SidetrackCostProfile(data)
    }
}
