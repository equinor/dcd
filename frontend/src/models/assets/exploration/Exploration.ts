import { EMPTY_GUID } from "../../../Utils/constants"
import { ExplorationWell } from "../../ExplorationWell"
import { IAsset } from "../IAsset"
import { ExplorationCostProfile } from "./ExplorationCostProfile"
import { GAndGAdminCost } from "./GAndAdminCost"

export class Exploration implements Components.Schemas.ExplorationDto, IAsset {
    id?: string | undefined
    projectId?: string | undefined
    name?: string | undefined
    costProfile?: ExplorationCostProfile | undefined
    gAndGAdminCost?: GAndGAdminCost | undefined
    rigMobDemob?: number | undefined
    currency?: Components.Schemas.Currency
    explorationWells?: ExplorationWell[] | null

    constructor(data?: Components.Schemas.ExplorationDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId
            this.name = data.name ?? ""
            this.costProfile = ExplorationCostProfile.fromJSON(data.costProfile)
            this.gAndGAdminCost = GAndGAdminCost.fromJSON(data.gAndGAdminCost)
            this.rigMobDemob = data.rigMobDemob
            this.currency = data.currency ?? 1
            this.explorationWells = data.explorationWells?.map((ew) => new ExplorationWell(ew))
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }
}
