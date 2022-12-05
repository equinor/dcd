import { EMPTY_GUID } from "../../../Utils/constants"
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
    LastChangedDate?: Date | null
    costYear?: number | undefined
    source?: Components.Schemas.Source | undefined
    ProspVersion?: Date | null
    DG3Date?: Date | null
    DG4Date?: Date | null
    hasChanges?: boolean

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
            this.currency = data.currency ?? 1
            this.LastChangedDate = data.lastChangedDate ? new Date(data.lastChangedDate) : null
            this.costYear = data.costYear
            this.source = data.source
            this.ProspVersion = data.prospVersion ? new Date(data.prospVersion) : null
            this.DG3Date = data.dG3Date ? new Date(data.dG3Date) : null
            this.DG4Date = data.dG4Date ? new Date(data.dG4Date) : null
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static Copy(data: Transport) {
        const transportCopy: Transport = new Transport(data)
        return {
            ...transportCopy,
            ProspVersion: data.ProspVersion,
            DG3Date: data.DG3Date,
            DG4Date: data.DG4Date,
        }
    }

    static fromJSON(data: Components.Schemas.SurfDto): Transport {
        return new Transport(data)
    }
}
