import { EMPTY_GUID } from "../../../Utils/constants"
import { IAsset } from "../IAsset"
import { DrillingSchedule } from "./DrillingSchedule"
import { WellProjectCostProfile } from "./WellProjectCostProfile"

export class WellProject implements Components.Schemas.WellProjectDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: WellProjectCostProfile | undefined
    drillingSchedule?: DrillingSchedule | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    rigMobDemob?: number | undefined
    annualWellInterventionCost?: number | undefined
    pluggingAndAbandonment?: number | undefined
    currency?: Components.Schemas.Currency

    constructor(data?: Components.Schemas.WellProjectDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId ?? ""
            this.costProfile = WellProjectCostProfile.fromJSON(data.costProfile)
            this.drillingSchedule = DrillingSchedule.fromJSON(data.drillingSchedule)
            this.artificialLift = data.artificialLift ?? 0
            this.rigMobDemob = data.rigMobDemob ?? 0
            this.annualWellInterventionCost = data.annualWellInterventionCost ?? 0
            this.pluggingAndAbandonment = data.pluggingAndAbandonment ?? 0
            this.currency = data.currency ?? 1
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): WellProject {
        return new WellProject(data)
    }
}
