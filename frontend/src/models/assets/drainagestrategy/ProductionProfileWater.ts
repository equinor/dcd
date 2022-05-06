export class ProductionProfileWater implements Components.Schemas.ProductionProfileWaterDto {
    id?: string
    startYear: number
    values: number []
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileWaterDto) {
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

    static fromJson(data?: Components.Schemas.ProductionProfileWaterDto):
    ProductionProfileWater | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileWater(data)
    }
}
