import { ITimeSeries } from "../../ITimeSeries"

export class FuelFlaringAndLossesCostProfile implements Components.Schemas.FuelFlaringAndLossesDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number[]
    sum?: number

    constructor(data?: Components.Schemas.FuelFlaringAndLossesDto) {
        if (data !== null && data !== undefined) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.NetSalesGasDto): FuelFlaringAndLossesCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new FuelFlaringAndLossesCostProfile(data)
    }
}
