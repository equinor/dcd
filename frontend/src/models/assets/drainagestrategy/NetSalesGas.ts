export class NetSalesGas implements Components.Schemas.NetSalesGasDto {
    id?: string
    startYear: number
    values: number []
    sum?: number

    constructor(data?: Components.Schemas.NetSalesGasDto) {
        if (data !== null && data !== undefined) {
            this.id = data?.id
            this.startYear = data.startYear ?? 0
            this.values = data.values ?? []
            this.sum = data.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.startYear = 0
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
