export class ProductionProfileWaterCostProfile implements Components.Schemas.ProductionProfileWaterDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileWaterDto) {
        if (data !== undefined) {
            this.id = data.id
            this.startYear = data.startYear
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
        }
    }

    static fromJson(data?: Components.Schemas.ProductionProfileWaterDto): ProductionProfileWaterCostProfile | undefined {
        if (data !== undefined) {
            return new ProductionProfileWaterCostProfile(data)
        }
        return undefined
    }

}