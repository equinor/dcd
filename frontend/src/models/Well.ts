import { EMPTY_GUID } from "../Utils/constants"

export class Well implements Components.Schemas.WellDto {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    wellInterventionCost?: number | undefined
    plugingAndAbandonmentCost?: number | undefined
    wellType?: Components.Schemas.WellTypeNew | undefined
    explorationWellType?: Components.Schemas.ExplorationWellType | undefined

    constructor(data?: Components.Schemas.WellDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId ?? ""
            this.wellInterventionCost = data.wellInterventionCost ?? 0
            this.plugingAndAbandonmentCost = data.plugingAndAbandonmentCost ?? 0
            this.wellType = data.wellType ?? undefined
            this.explorationWellType = data.explorationWellType ?? undefined
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellDto): Well {
        return new Well(data)
    }
}
