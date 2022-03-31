export class ProductionProfileGasCostProfile implements Components.Schemas.ProductionProfileGasDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    sum?: number

    constructor(data?: Components.Schemas.ProductionProfileGasDto) {
        if (data !== undefined || data !== null) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.sum = data?.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
        }
    }

    static fromJson(data?: Components.Schemas.ProductionProfileGasDto): ProductionProfileGasCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new ProductionProfileGasCostProfile(data)
    }
}
