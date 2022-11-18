import { EMPTY_GUID } from "../../../Utils/constants"
import { WellProjectWell } from "../../WellProjectWell"
import { IAsset } from "../IAsset"
import { GasInjectorCostProfile } from "./GasInjectorCostProfile"
import { GasProducerCostProfile } from "./GasProducerCostProfile"
import { OilProducerCostProfile } from "./OilProducerCostProfile"
import { WaterInjectorCostProfile } from "./WaterInjectorCostProfile"

export class WellProject implements Components.Schemas.WellProjectDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    oilProducerCostProfile?: OilProducerCostProfile
    gasProducerCostProfile?: GasProducerCostProfile
    waterInjectorCostProfile?: WaterInjectorCostProfile
    gasInjectorCostProfile?: GasInjectorCostProfile
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    currency?: Components.Schemas.Currency
    wellProjectWells?: WellProjectWell[] | null

    constructor(data?: Components.Schemas.WellProjectDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId ?? ""
            this.oilProducerCostProfile = OilProducerCostProfile.fromJSON(data.oilProducerCostProfile)
            this.gasProducerCostProfile = GasProducerCostProfile.fromJSON(data.gasProducerCostProfile)
            this.waterInjectorCostProfile = WaterInjectorCostProfile.fromJSON(data.waterInjectorCostProfile)
            this.gasInjectorCostProfile = GasInjectorCostProfile.fromJSON(data.gasInjectorCostProfile)
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
