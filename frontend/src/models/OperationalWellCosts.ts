import { EMPTY_GUID } from "../Utils/constants"

export class OperationalWellCosts implements Components.Schemas.OperationalWellCostsDto {
    id?: string // uuid
    rigUpgrading?: number // double
    rigMobDemob?: number // double
    projectDrillingCosts?: number // double
    annualWellInterventionCostPerWell?: number // double
    pluggingAndAbandonment?: number // double

    constructor(data?: Components.Schemas.OperationalWellCostsDto) {
        if (data !== undefined) {
            this.id = data.id
            this.rigUpgrading = data.rigUpgrading
            this.rigMobDemob = data.rigMobDemob
            this.projectDrillingCosts = data.projectDrillingCosts
            this.annualWellInterventionCostPerWell = data.annualWellInterventionCostPerWell
            this.pluggingAndAbandonment = data.pluggingAndAbandonment
        } else {
            this.id = EMPTY_GUID
        }
    }

    static fromJSON(data: Components.Schemas.OperationalWellCostsDto | undefined): OperationalWellCosts {
        return new OperationalWellCosts(data)
    }
}
