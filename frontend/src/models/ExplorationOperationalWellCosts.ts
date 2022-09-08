import { EMPTY_GUID } from "../Utils/constants"

export class ExplorationOperationalWellCosts implements Components.Schemas.ExplorationOperationalWellCostsDto {
    id?: string // uuid
    rigUpgrading?: number // double
    explorationRigMobDemob?: number // double
    explorationProjectDrillingCosts?: number // double
    appraisalRigMobDemob?: number // double
    appraisalProjectDrillingCosts?: number // double

    constructor(data?: Components.Schemas.ExplorationOperationalWellCostsDto) {
        if (data) {
            this.id = data.id
            this.rigUpgrading = data.rigUpgrading
            this.explorationRigMobDemob = data.explorationRigMobDemob
            this.explorationProjectDrillingCosts = data.explorationProjectDrillingCosts
            this.appraisalRigMobDemob = data.appraisalRigMobDemob
            this.appraisalProjectDrillingCosts = data.appraisalProjectDrillingCosts
        } else {
            this.id = EMPTY_GUID
        }
    }

    static fromJSON(data: Components.Schemas.ExplorationOperationalWellCostsDto | undefined):
        ExplorationOperationalWellCosts {
        return new ExplorationOperationalWellCosts(data)
    }
}
