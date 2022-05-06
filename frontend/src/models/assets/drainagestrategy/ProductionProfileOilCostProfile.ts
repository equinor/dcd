import { ITimeSeries } from "../../ITimeSeries"

export class ProductionProfileOilCostProfile implements Components.Schemas.ProductionProfileOilDto, ITimeSeries {
    id?: string
    startYear?: number
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileOilDto) {
        if (data !== undefined && data !== null) {
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

    static fromJson(data?: Components.Schemas.ProductionProfileOilDto): ProductionProfileOilCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileOilCostProfile(data)
    }
}
