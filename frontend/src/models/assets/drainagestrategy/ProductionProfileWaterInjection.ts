import { ITimeSeries } from "../../ITimeSeries"

export class ProductionProfileWaterInjection implements
Components.Schemas.ProductionProfileWaterInjectionDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileWaterInjectionDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Production profile water injection"
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
            this.name = "Production profile water injection"
            this.values = []
        }
    }

    static fromJson(data?: Components.Schemas.ProductionProfileWaterInjectionDto):
    ProductionProfileWaterInjection | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileWaterInjection(data)
    }
}
