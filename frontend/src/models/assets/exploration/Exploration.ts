import { EMPTY_GUID } from "../../../Utils/constants"
import { Well } from "../../Well"
import { IAsset } from "../IAsset"
import { ExplorationCostProfile } from "./ExplorationCostProfile"
import { ExplorationDrillingSchedule } from "./ExplorationDrillingSchedule"
import { GAndGAdminCost } from "./GAndAdminCost"

export class Exploration implements Components.Schemas.ExplorationDto, IAsset {
    id?: string | undefined
    projectId?: string | undefined
    name?: string | undefined
    costProfile?: ExplorationCostProfile | undefined
    drillingSchedule?: ExplorationDrillingSchedule | undefined
    gAndGAdminCost?: GAndGAdminCost | undefined
    rigMobDemob?: number | undefined
    currency?: Components.Schemas.Currency

    constructor(data?: Components.Schemas.ExplorationDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId
            this.name = data.name ?? ""
            this.costProfile = ExplorationCostProfile.fromJSON(data.costProfile)
            this.drillingSchedule = ExplorationDrillingSchedule.fromJSON(data.drillingSchedule)
            this.gAndGAdminCost = GAndGAdminCost.fromJSON(data.gAndGAdminCost)
            this.rigMobDemob = data.rigMobDemob
            this.currency = data.currency ?? 1
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }
}
