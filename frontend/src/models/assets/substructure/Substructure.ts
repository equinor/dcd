import { SubstructureCostProfile } from "./SubstructureCostProfile"

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

    static Copy(data: Substructure) {
        const substructureCopy = new Substructure(data)
        return {
            ...substructureCopy,
            substructureCostProfile: data.substructureCostProfile,
        }
    }

    static fromJSON(data: Components.Schemas.SubstructureDto): Substructure {
        return new Substructure(data)
    }
}
