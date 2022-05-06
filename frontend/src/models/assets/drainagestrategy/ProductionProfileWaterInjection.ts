export class ProductionProfileWaterInjection implements
Components.Schemas.ProductionProfileWaterInjectionDto {
    id?: string
    startYear: number
    values: number []
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileWaterInjectionDto) {
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

    static fromJson(data?: Components.Schemas.ProductionProfileWaterInjectionDto):
    ProductionProfileWaterInjection | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileWaterInjection(data)
    }
}
