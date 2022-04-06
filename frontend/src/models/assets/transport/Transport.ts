import { TransportCostProfile } from "./TransportCostProfile"

export class Transport implements Components.Schemas.TransportDto {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: TransportCostProfile | undefined
    maturity?: Components.Schemas.Maturity | undefined
    gasExportPipelineLength?: number | undefined
    oilExportPipelineLength?: number | undefined

    constructor(data?: Components.Schemas.TransportDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = TransportCostProfile.fromJSON(data.costProfile)
            this.maturity = data.maturity
            this.gasExportPipelineLength = data.gasExportPipelineLength
            this.oilExportPipelineLength = data.oilExportPipelineLength
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Transport {
        return new Transport(data)
    }
}
