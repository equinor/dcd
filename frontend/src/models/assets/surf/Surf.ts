import { SurfCostProfile } from "./SurfCostProfile"
import { SurfCessationCostProfile } from "./SurfCessationCostProfile"
import { IAsset } from "../IAsset"
import { EMPTY_GUID } from "../../../Utils/constants"

export class Surf implements Components.Schemas.SurfDto, IAsset {
    id?: string | undefined
    name: string | undefined
    projectId?: string | undefined
    costProfile?: SurfCostProfile | undefined
    cessationCostProfile: SurfCessationCostProfile | undefined
    maturity?: Components.Schemas.Maturity | undefined
    infieldPipelineSystemLength?: number | undefined
    umbilicalSystemLength?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    riserCount?: number | undefined
    templateCount?: number | undefined
    producerCount?: number | undefined
    gasInjectorCount?: number | undefined
    waterInjectorCount?: number | undefined
    productionFlowline?: Components.Schemas.ProductionFlowline | undefined
    currency?: Components.Schemas.Currency

    constructor(data?: Components.Schemas.SurfDto) {
        if (data !== undefined) {
            this.id = data.id ?? ""
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.cessationCostProfile = SurfCessationCostProfile.fromJSON(data.cessationCostProfile)
            this.costProfile = SurfCostProfile.fromJSON(data.costProfile)
            this.maturity = data.maturity
            this.infieldPipelineSystemLength = data.infieldPipelineSystemLength
            this.umbilicalSystemLength = data.umbilicalSystemLength
            this.artificialLift = data.artificialLift
            this.riserCount = data.riserCount
            this.templateCount = data.templateCount
            this.producerCount = data.producerCount
            this.gasInjectorCount = data.gasInjectorCount
            this.waterInjectorCount = data.waterInjectorCount
            this.productionFlowline = data.productionFlowline
            this.currency = data.currency ?? 0
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Surf {
        return new Surf(data)
    }
}
