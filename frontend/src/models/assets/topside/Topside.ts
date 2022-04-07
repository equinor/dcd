import { TopsideCostProfile } from "./TopsideCostProfile"
import { TopsideCessasionCostProfile } from "./TopsideCessasionCostProfile"

export class Topside implements Components.Schemas.TopsideDto {
    id?: string | undefined
    name?: string | undefined
    projectId?: string | undefined
    costProfile?: TopsideCostProfile | undefined
    topsideCessasionCostProfileDto?: TopsideCessasionCostProfile | undefined
    dryWeight?: number | undefined
    oilCapacity?: number | undefined
    gasCapacity?: number | undefined
    facilitiesAvailability?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    maturity?: Components.Schemas.Maturity | undefined

    constructor(data?: Components.Schemas.TopsideDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name ?? ""
            this.projectId = data.projectId
            this.costProfile = TopsideCostProfile.fromJSON(data.costProfile)
            this.topsideCessasionCostProfileDto = TopsideCessasionCostProfile.fromJSON(data.topsideCessasionCostProfileDto)
            this.dryWeight = data.dryWeight
            this.maturity = data.maturity
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }
}
