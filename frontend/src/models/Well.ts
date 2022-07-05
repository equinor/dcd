import { EMPTY_GUID } from "../Utils/constants"

export class Well implements Components.Schemas.WellDto {
    id?: string // uuid
    projectId?: string // uuid
    name?: string | null
    wellInterventionCost?: number // double
    plugingAndAbandonmentCost?: number // double
    wellCategory?: Components.Schemas.WellCategory /* int32 */
    wellCost?: number // double
    drillingDays?: number // double

    constructor(data?: Components.Schemas.WellDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId ?? ""
            this.name = data.name ?? ""
            this.wellInterventionCost = data.wellInterventionCost ?? 0
            this.plugingAndAbandonmentCost = data.plugingAndAbandonmentCost ?? 0
            this.wellCategory = data.wellCategory
            this.wellCost = data.wellCost
            this.drillingDays = data.drillingDays
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellDto): Well {
        return new Well(data)
    }
}
