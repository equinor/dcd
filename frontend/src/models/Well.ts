import { EMPTY_GUID } from "../Utils/constants"

export class Well implements Components.Schemas.WellDto {
    id?: string // uuid
    projectId?: string // uuid
    name?: string | null
    wellInterventionCost?: number // double
    plugingAndAbandonmentCost?: number // double
    category?: Components.Schemas.WellTypeCategoryDto /* int32 */
    wellCost?: number // double
    drillingDays?: number // double
    description?: string | null

    constructor(data?: Components.Schemas.WellDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId ?? ""
            this.name = data.name ?? ""
            this.wellInterventionCost = data.wellInterventionCost ?? 0
            this.plugingAndAbandonmentCost = data.plugingAndAbandonmentCost ?? 0
            this.category = data.category
            this.wellCost = data.wellCost
            this.drillingDays = data.drillingDays
            this.description = data.description
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellDto): Well {
        return new Well(data)
    }
}
