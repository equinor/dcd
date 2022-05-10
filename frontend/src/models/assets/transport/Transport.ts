import { IAsset } from "../IAsset"
import { TransportCessationCostProfile } from "./TransportCessationCostProfile"
import { TransportCostProfile } from "./TransportCostProfile"

export class Transport implements Components.Schemas.TransportDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: TransportCostProfile | undefined
    cessationCostProfile?: TransportCessationCostProfile | undefined
    maturity?: Components.Schemas.Maturity | undefined
    gasExportPipelineLength?: number | undefined
    oilExportPipelineLength?: number | undefined
    currency?: Components.Schemas.Currency

    constructor(data?: Components.Schemas.TransportDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = TransportCostProfile.fromJSON(data.costProfile)
            this.cessationCostProfile = TransportCessationCostProfile
                .fromJSON(data.cessationCostProfile)
            this.maturity = data.maturity
            this.gasExportPipelineLength = data.gasExportPipelineLength
            this.oilExportPipelineLength = data.oilExportPipelineLength
            this.currency = data.currency ?? 0
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Transport {
        return new Transport(data)
    }
}
