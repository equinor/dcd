export class GAndGAdminCostDto implements Components.Schemas.GAndGAdminCostDto {
    id?: string | undefined
    startYear?: number | undefined
    values?: number [] | null

    constructor(data?: Components.Schemas.GAndGAdminCostDto) {
        this.id = data?.id
        this.startYear = data?.startYear
        this.values = data?.values ?? []
    }

    static fromJSON(data?: Components.Schemas.GAndGAdminCostDto): GAndGAdminCostDto {
        return new GAndGAdminCostDto(data)
    }
}
