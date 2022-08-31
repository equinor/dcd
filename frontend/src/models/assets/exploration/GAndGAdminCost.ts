import { ITimeSeries } from "../../ITimeSeries"

export class GAndGAdminCost implements Components.Schemas.GAndGAdminCostDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []

    constructor(data?: Components.Schemas.GAndGAdminCostDto) {
        this.id = data?.id
        this.startYear = data?.startYear ?? 0
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.GAndGAdminCostDto): GAndGAdminCost | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new GAndGAdminCost(data)
    }
}
