import { EMPTY_GUID } from "../Utils/constants"

export class DevelopmentOperationalWellCosts implements Components.Schemas.DevelopmentOperationalWellCostsDto {
    id?: string // uuid
    rigUpgrading?: number // double
    rigMobDemob?: number // double
    annualWellInterventionCostPerWell?: number // double
    pluggingAndAbandonment?: number // double

    constructor(data?: Components.Schemas.DevelopmentOperationalWellCostsDto) {
        if (data) {
            this.id = data.id
            this.rigUpgrading = data.rigUpgrading
            this.rigMobDemob = data.rigMobDemob
            this.annualWellInterventionCostPerWell = data.annualWellInterventionCostPerWell
            this.pluggingAndAbandonment = data.pluggingAndAbandonment
        } else {
            this.id = EMPTY_GUID
        }
    }

    static fromJSON(data: Components.Schemas.DevelopmentOperationalWellCostsDto | undefined):
        DevelopmentOperationalWellCosts {
        return new DevelopmentOperationalWellCosts(data)
    }
}
