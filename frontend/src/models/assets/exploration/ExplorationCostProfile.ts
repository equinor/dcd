import { EMPTY_GUID } from "../../../Utils/constants"
import { ITimeSeries } from "../../ITimeSeries"

export class ExplorationCostProfile implements Components.Schemas.ExplorationCostProfileDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    epaVersion?: string
    currency?: Components.Schemas.Currency
    sum?: number
    override?: boolean

    constructor(data?: Components.Schemas.ExplorationCostProfileDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = EMPTY_GUID
            this.startYear = 0
            this.values = []
        }
    }

    static fromJSON(data?: Components.Schemas.ExplorationCostProfileDto): ExplorationCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ExplorationCostProfile(data)
    }
}
