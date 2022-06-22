import { EMPTY_GUID } from "../../../Utils/constants"
import { IAsset } from "../IAsset"
import { ExplorationCostProfile } from "./ExplorationCostProfile"
import { ExplorationDrillingSchedule } from "./ExplorationDrillingSchedule"
import { GAndGAdminCost } from "./GAndAdminCost"

export class Exploration implements Components.Schemas.ExplorationDto, IAsset {
    id?: string | undefined
    projectId?: string | undefined
    name?: string | undefined
    wellType?: Components.Schemas.WellType | undefined
    costProfile?: ExplorationCostProfile | undefined
    drillingSchedule?: ExplorationDrillingSchedule | undefined
    gAndGAdminCost?: GAndGAdminCost | undefined
    rigMobDemob?: number | undefined
    currency?: Components.Schemas.Currency
    explorationWellType?: Components.Schemas.ExplorationWellType | undefined

    constructor(data?: Components.Schemas.ExplorationDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId
            this.name = data.name ?? ""
            this.wellType = data.wellType
            this.costProfile = ExplorationCostProfile.fromJSON(data.costProfile)
            this.drillingSchedule = ExplorationDrillingSchedule.fromJSON(data.drillingSchedule)
            this.gAndGAdminCost = GAndGAdminCost.fromJSON(data.gAndGAdminCost)
            this.rigMobDemob = data.rigMobDemob
            this.currency = data.currency ?? 1
            this.explorationWellType = data.explorationWellType
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }
}
