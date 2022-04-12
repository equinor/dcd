import { SurfCostProfile } from "./SurfCostProfile"
import { SurfCessationCostProfile } from "./SurfCessationCostProfile"

export class Surf implements Components.Schemas.SurfDto {
    id?: string | undefined
    name: string | undefined
    projectId?: string | undefined
    costProfile?: SurfCostProfile | undefined
    surfCessationCostProfileDto: SurfCessationCostProfile | undefined
    maturity?: Components.Schemas.Maturity | undefined
    infieldPipelineSystemLength?: number | undefined
    umbilicalSystemLength?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    riserCount?: number | undefined
    templateCount?: number | undefined
    productionFlowline?: Components.Schemas.ProductionFlowline | undefined

    constructor(data?: Components.Schemas.SurfDto) {
        if (data !== undefined) {
            this.id = data.id ?? ""
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.surfCessationCostProfileDto = SurfCessationCostProfile.fromJSON(data.surfCessationCostProfileDto)
            this.costProfile = SurfCostProfile.fromJSON(data.costProfile)
            this.maturity = data.maturity
            this.infieldPipelineSystemLength = data.infieldPipelineSystemLength
            this.umbilicalSystemLength = data.umbilicalSystemLength
            this.artificialLift = data.artificialLift
            this.riserCount = data.riserCount
            this.templateCount = data.templateCount
            this.productionFlowline = data.productionFlowline
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Surf {
        return new Surf(data)
    }
}
