export class ProductionProfileWaterInjectionCostProfile implements Components.Schemas.ProductionProfileWaterInjectionDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileWaterInjectionDto) {
        if (data !== null || undefined) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.sum = data?.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
        }
    }

    static fromJson(data?: Components.Schemas.ProductionProfileWaterInjectionDto): ProductionProfileWaterInjectionCostProfile | undefined {
        if (data !== undefined) {
            return new ProductionProfileWaterInjectionCostProfile(data)
        }
        return undefined
    }

}