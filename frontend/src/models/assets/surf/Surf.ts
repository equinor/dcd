import { SurfCostProfile } from "./SurfCostProfile"

export class Surf implements Components.Schemas.SurfDto {
    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: SurfCostProfile | undefined
    maturity?: Components.Schemas.Maturity | undefined
    infieldPipelineSystemLength?: number | undefined
    umbilicalSystemLength?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    riserCount?: number | undefined
    templateCount?: number | undefined
    productionFlowline?: Components.Schemas.ProductionFlowline | undefined

    constructor(data: Components.Schemas.SurfDto) {
        this.id = data.id ?? ""
        this.name = data.name ?? ""
        this.projectId = data.projectId
        this.costProfile = SurfCostProfile.fromJSON(data.costProfile)
        this.maturity = data.maturity
        this.infieldPipelineSystemLength = data.infieldPipelineSystemLength
        this.umbilicalSystemLength = data.umbilicalSystemLength
        this.artificialLift = data.artificialLift
        this.riserCount = data.riserCount
        this.templateCount = data.templateCount
        this.productionFlowline = data.productionFlowline
    }

    static fromJSON(data: Components.Schemas.SurfDto): Surf {
        return new Surf(data)
    }
}
