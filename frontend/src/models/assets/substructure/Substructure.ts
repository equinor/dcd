import { SubstructureCostProfile } from "./SubstructureCostProfile"
import { SubstructureCessasionCostProfile } from "./SubstructureCessasionCostProfile"

export class Substructure implements Components.Schemas.SubstructureDto {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: SubstructureCostProfile | undefined
    substructureCessasionCostProfileDto?: SubstructureCessasionCostProfile | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined

    constructor(data?: Components.Schemas.SubstructureDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = SubstructureCostProfile.fromJSON(data.costProfile)
            this.substructureCessasionCostProfileDto = SubstructureCessasionCostProfile.fromJSON(data.substructureCessasionCostProfileDto)
            this.dryweight = data.dryWeight
            this.maturity = data.maturity
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.SubstructureDto): Substructure {
        return new Substructure(data)
    }
}
