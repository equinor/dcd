import { IAsset } from "../IAsset"
import { DrillingScheduleDto } from "./DrillingScheduleDto"
import { WellProjectCostProfileDto } from "./WellProjectCostProfileDto"

export class WellProject implements Components.Schemas.WellProjectDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: WellProjectCostProfileDto | undefined
    drillingSchedule?: DrillingScheduleDto | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    rigMobDemob?: number | undefined
    annualWellInterventionCost?: number | undefined
    pluggingAndAbandonment?: number | undefined

    constructor(data?: Components.Schemas.WellProjectDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId ?? ""
            this.costProfile = WellProjectCostProfileDto.fromJSON(data.costProfile)
            this.drillingSchedule = DrillingScheduleDto.fromJSON(data.drillingSchedule)
            this.artificialLift = data.artificialLift ?? 0
            this.rigMobDemob = data.rigMobDemob ?? 0
            this.annualWellInterventionCost = data.annualWellInterventionCost ?? 0
            this.pluggingAndAbandonment = data.pluggingAndAbandonment ?? 0
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): WellProject {
        return new WellProject(data)
    }
}
