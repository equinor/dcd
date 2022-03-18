import { DrillingScheduleDto } from "./DrillingScheduleDto"
import { WellProjectCostProfileDto } from "./WellProjectCostProfileDto"

export class Wellproject implements Components.Schemas.WellProjectDto {
    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: WellProjectCostProfileDto | undefined
    drillingSchedule?: DrillingScheduleDto | undefined
    producerCount?: number | undefined
    gasInjectorCount?: number | undefined
    waterInjectorCount?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    rigMobDemob?: number | undefined
    annualWellInterventionCost?: number | undefined
    pluggingAndAbandonment?: number | undefined

    constructor(data: Components.Schemas.WellProjectDto) {
        this.id = data.id
        this.name = data.name ?? null
        this.projectId = data.projectId ?? ""
        this.costProfile = WellProjectCostProfileDto.fromJSON(data.costProfile)
        this.drillingSchedule = DrillingScheduleDto.fromJSON(data.drillingSchedule)
        this.producerCount = data.producerCount ?? 0
        this.gasInjectorCount = data.gasInjectorCount ?? 0
        this.waterInjectorCount = data.waterInjectorCount ?? 0
        this.artificialLift = data.artificialLift ?? 0
        this.rigMobDemob = data.rigMobDemob ?? 0
        this.annualWellInterventionCost = data.annualWellInterventionCost ?? 0
        this.pluggingAndAbandonment = data.pluggingAndAbandonment ?? 0
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): Wellproject {
        return new Wellproject(data)
    }
}
