import { SubstructureCostProfile } from "./SubstructureCostProfile"
import { SubstructureCessationCostProfile } from "./SubstructureCessationCostProfile"
import { IAsset } from "../IAsset"
import { EMPTY_GUID } from "../../../Utils/constants"

export class Substructure implements Components.Schemas.SubstructureDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: SubstructureCostProfile | undefined
    cessationCostProfile?: SubstructureCessationCostProfile | undefined
    dryweight?: number | undefined
    maturity?: Components.Schemas.Maturity | undefined
    currency?: Components.Schemas.Currency
    approvedBy?: string | null | undefined
    costYear?: number | undefined
    ProspVersion?: Date | null
    source?: Components.Schemas.Source
    LastChangedDate?: Date | null

    constructor(data?: Components.Schemas.SubstructureDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = SubstructureCostProfile.fromJSON(data.costProfile)
            this.cessationCostProfile = SubstructureCessationCostProfile
                .fromJSON(data.cessationCostProfile)
            this.dryweight = data.dryWeight
            this.maturity = data.maturity
            this.currency = data.currency ?? 0
            this.approvedBy = data.approvedBy ?? ""
            this.costYear = data.costYear
            this.ProspVersion = data.prospVersion ? new Date(data.prospVersion) : null
            this.source = data.source
            this.LastChangedDate = data.lastChangedDate ? new Date(data.lastChangedDate) : null
        } else {
            this.id = EMPTY_GUID
            this.name = ""
            this.approvedBy = ""
        }
    }

    static Copy(data: Substructure) {
        const substructureCopy: Substructure = new Substructure(data)
        return {
            ...substructureCopy,
            ProspVersion: data.ProspVersion,
        }
    }

    static fromJSON(data: Components.Schemas.SubstructureDto): Substructure {
        return new Substructure(data)
    }
}
