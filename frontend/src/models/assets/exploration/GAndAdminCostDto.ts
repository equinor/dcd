export class GAndGAdminCostDto implements Components.Schemas.GAndGAdminCostDto {
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.GAndGAdminCostDto) {
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.GAndGAdminCostDto): GAndGAdminCostDto {
        return new GAndGAdminCostDto(data)
    }
}
