export class Substructure implements Components.Schemas.SubstructureDto {

    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    substructureCostProfile?: SubstructureCostProfile | undefined
    dryweight?: number | null
    maturity?: Components.Schemas.Maturity | undefined

    constructor(data: Components.Schemas.SubstructureDto) {
        this.id = data.id
        this.name = data.name ?? ""
        this.projectId = data.projectId
        this.substructureCostProfile = SubstructureCostProfile.fromJSON(data.costProfile)
        this.dryweight = data.dryWeight ?? null
        this.maturity = data.maturity
    }

    static fromJSON(data: Components.Schemas.SubstructureDto): Substructure {
        return new Substructure(data)
    }

}

export class SubstructureCostProfile implements Components.Schemas.SubstructureCostProfileDto{
    startYear: number | undefined
    values: number [] | null
    epaVersion: string | null
    currency: Components.Schemas.Currency | undefined
    sum: number | undefined

    constructor(data: Components.Schemas.SubstructureCostProfileDto) {
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data?: Components.Schemas.SubstructureCostProfileDto): SubstructureCostProfile {
        return new SubstructureCostProfile(data!)
    }
}