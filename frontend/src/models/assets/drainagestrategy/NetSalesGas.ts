import { ITimeSeries } from "../../ITimeSeries"

export class NetSalesGas implements Components.Schemas.NetSalesGasDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.NetSalesGasDto) {
        if (data !== null && data !== undefined) {
            this.id = data?.id
            this.startYear = data.startYear ?? 0
            this.name = "Net sales gas"
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Net sales gas"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.NetSalesGasDto): NetSalesGas | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new NetSalesGas(data)
    }
}
