import { ITimeSeries } from "../../ITimeSeries"

export class FuelFlaringAndLossesOverride implements Components.Schemas.FuelFlaringAndLossesOverrideDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number[]
    sum?: number
    override?: boolean

    constructor(data?: Components.Schemas.FuelFlaringAndLossesOverrideDto) {
        if (data !== null && data !== undefined) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Fuel flaring and losses"
            this.values = data.values ?? []
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Fuel flaring and losses"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.FuelFlaringAndLossesOverrideDto):
        FuelFlaringAndLossesOverride | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new FuelFlaringAndLossesOverride(data)
    }
}
