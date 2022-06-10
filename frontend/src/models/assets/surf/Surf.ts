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
    LastChangedDate?: Date | null
    costYear?: number | undefined
    source?: Components.Schemas.Source
    ProspVersion?: Date | null
    approvedBy?: string | null | undefined
    DG3Date?: Date | null
    DG4Date?: Date | null

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
            this.currency = data.currency ?? 1
            this.LastChangedDate = data.lastChangedDate ? new Date(data.lastChangedDate) : null
            this.costYear = data.costYear
            this.source = data.source
            this.ProspVersion = data.prospVersion ? new Date(data.prospVersion) : null
            this.approvedBy = data.approvedBy ?? ""
            this.DG3Date = data.dG3Date ? new Date(data.dG3Date) : null
            this.DG4Date = data.dG4Date ? new Date(data.dG4Date) : null
        } else {
            this.id = EMPTY_GUID
            this.name = ""
            this.approvedBy = ""
        }
    }

    static Copy(data: Surf) {
        const surfCopy: Surf = new Surf(data)
        return {
            ...surfCopy,
            ProspVersion: data.ProspVersion,
            DG3Date: data.DG3Date,
            DG4Date: data.DG4Date,
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Surf {
        return new Surf(data)
    }
}
