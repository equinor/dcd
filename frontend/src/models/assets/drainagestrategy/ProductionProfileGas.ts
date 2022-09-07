import { ITimeSeries } from "../../ITimeSeries"

export class ProductionProfileGas implements Components.Schemas.ProductionProfileGasDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileGasDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Production profile gas"
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Production profile gas"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.ProductionProfileGasDto): ProductionProfileGas | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileGas(data)
    }
}
