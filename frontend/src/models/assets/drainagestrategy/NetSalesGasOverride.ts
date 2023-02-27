import { ITimeSeries } from "../../ITimeSeries"

export class NetSalesGasOverride implements Components.Schemas.NetSalesGasOverrideDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number[]
    sum?: number
    override?: boolean

    constructor(data?: Components.Schemas.NetSalesGasOverrideDto) {
        if (data !== null && data !== undefined) {
            this.id = data?.id
            this.startYear = data.startYear ?? 0
            this.name = "Net sales gas"
            this.values = data.values ?? []
            this.sum = data.sum
            this.override = data.override
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Net sales gas"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.NetSalesGasOverrideDto): NetSalesGasOverride | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new NetSalesGasOverride(data)
    }
}
