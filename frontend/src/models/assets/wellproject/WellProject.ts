import { EMPTY_GUID } from "../../../Utils/constants"
import { WellProjectWell } from "../../WellProjectWell"
import { IAsset } from "../IAsset"
import { GasInjectorCostProfile } from "./GasInjectorCostProfile"
import { GasInjectorCostProfileOverride } from "./GasInjectorCostProfileOverride"
import { GasProducerCostProfile } from "./GasProducerCostProfile"
import { GasProducerCostProfileOverride } from "./GasProducerCostProfileOverride"
import { OilProducerCostProfile } from "./OilProducerCostProfile"
import { OilProducerCostProfileOverride } from "./OilProducerCostProfileOverride"
import { WaterInjectorCostProfile } from "./WaterInjectorCostProfile"
import { WaterInjectorCostProfileOverride } from "./WaterInjectorCostProfileOverride"

export class WellProject implements Components.Schemas.WellProjectDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    oilProducerCostProfile?: OilProducerCostProfile
    oilProducerCostProfileOverride?: OilProducerCostProfileOverride
    gasProducerCostProfile?: GasProducerCostProfile
    gasProducerCostProfileOverride?: GasProducerCostProfileOverride
    waterInjectorCostProfile?: WaterInjectorCostProfile
    waterInjectorCostProfileOverride?: WaterInjectorCostProfileOverride
    gasInjectorCostProfile?: GasInjectorCostProfile
    gasInjectorCostProfileOverride?: GasInjectorCostProfileOverride
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    currency?: Components.Schemas.Currency
    wellProjectWells?: WellProjectWell[] | null
    hasChanges?: boolean

    constructor(data?: Components.Schemas.WellProjectDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId ?? ""
            this.oilProducerCostProfile = OilProducerCostProfile.fromJSON(data.oilProducerCostProfile)
            this.oilProducerCostProfileOverride = OilProducerCostProfileOverride
                .fromJSON(data.oilProducerCostProfileOverride)

            this.gasProducerCostProfile = GasProducerCostProfile.fromJSON(data.gasProducerCostProfile)
            this.gasProducerCostProfileOverride = GasProducerCostProfileOverride
                .fromJSON(data.gasProducerCostProfileOverride)

            this.waterInjectorCostProfile = WaterInjectorCostProfile.fromJSON(data.waterInjectorCostProfile)
            this.waterInjectorCostProfileOverride = WaterInjectorCostProfileOverride
                .fromJSON(data.waterInjectorCostProfileOverride)

            this.gasInjectorCostProfile = GasInjectorCostProfile.fromJSON(data.gasInjectorCostProfile)
            this.gasInjectorCostProfileOverride = GasInjectorCostProfileOverride
                .fromJSON(data.gasInjectorCostProfileOverride)

            this.artificialLift = data.artificialLift ?? 0
            this.currency = data.currency ?? 1
            this.wellProjectWells = data.wellProjectWells?.map((wc) => new WellProjectWell(wc))
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellProjectDto): WellProject {
        return new WellProject(data)
    }
}
