export class Co2EmissionsCostProfile implements Components.Schemas.Co2EmissionsDto {
    id?: string
    startYear?: number | undefined
    values?: number [] | null
    sum?: number

    constructor(data?: Components.Schemas.Co2EmissionsDto) {
        if (data !== undefined && data !== undefined) {
            this.id = data?.id
            this.startYear = data?.startYear
            this.values = data?.values ?? []
            this.sum = data?.sum
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
        }
    }

    static fromJson(data?: Components.Schemas.NetSalesGasDto): Co2EmissionsCostProfile | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new Co2EmissionsCostProfile(data)
    }
}
