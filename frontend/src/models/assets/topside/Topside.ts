import { TopsideCostProfile } from "./TopsideCostProfile"
import { TopsideCessationCostProfile } from "./TopsideCessationCostProfile"
import { IAsset } from "../IAsset"
import { EMPTY_GUID } from "../../../Utils/constants"

export class Topside implements Components.Schemas.TopsideDto, IAsset {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: TopsideCostProfile | undefined
    cessationCostProfile?: TopsideCessationCostProfile | undefined
    dryWeight?: number | undefined
    oilCapacity?: number | undefined
    gasCapacity?: number | undefined
    facilitiesAvailability?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    maturity?: Components.Schemas.Maturity | undefined
    currency?: Components.Schemas.Currency

    constructor(data?: Components.Schemas.TopsideDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = TopsideCostProfile.fromJSON(data.costProfile)
            this.cessationCostProfile = TopsideCessationCostProfile
                .fromJSON(data.cessationCostProfile)
            this.artificialLift = data.artificialLift ?? 0
            this.dryWeight = data.dryWeight
            this.maturity = data.maturity
            this.oilCapacity = data.oilCapacity
            this.gasCapacity = data.gasCapacity
            this.currency = data.currency ?? 0
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }
}
