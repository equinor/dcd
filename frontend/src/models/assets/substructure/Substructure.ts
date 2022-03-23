import { SubstructureCostProfile } from "./SubstructureCostProfile"

export class Substructure implements Components.Schemas.SubstructureDto {
    id?: string
    name?: string
    projectId?: string
    substructureCostProfile?: SubstructureCostProfile | undefined
    dryweight?: number
    maturity?: Components.Schemas.Maturity

    constructor(data?: Components.Schemas.SubstructureDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.substructureCostProfile = SubstructureCostProfile.fromJSON(data.costProfile)
            this.dryweight = data.dryWeight
            this.maturity = data.maturity
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
            this.substructureCostProfile = new SubstructureCostProfile()
        }
    }

    static Copy(data: Substructure) {
        const substructureCopy = new Substructure(data)
        return {
            ...substructureCopy,
            substructureCostProfile: data.substructureCostProfile,
        }
    }

    static ToDto(data: Substructure): Components.Schemas.SubstructureDto {
        const substructureCopy = new Substructure(data)
        return {
            ...substructureCopy,
            costProfile: data.substructureCostProfile,
        }
    }

    static fromJSON(data: Components.Schemas.SubstructureDto): Substructure {
        return new Substructure(data)
    }
}
