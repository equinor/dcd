import { TopsideCostProfile } from "./TopsideCostProfile"

export class Topside implements Components.Schemas.TopsideDto {
    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: TopsideCostProfile | undefined
    dryWeight?: number | undefined
    oilCapacity?: number | undefined
    gasCapacity?: number | undefined
    facilitiesAvailability?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    maturity?: Components.Schemas.Maturity | undefined

    constructor(data: Components.Schemas.TopsideDto) {
        this.id = data.id
        this.name = data.name ?? ""
        this.projectId = data.projectId
        this.costProfile = TopsideCostProfile.fromJSON(data.costProfile)
        this.dryWeight = data.dryWeight
        this.oilCapacity = data.oilCapacity
        this.gasCapacity = data.gasCapacity
        this.facilitiesAvailability = data.facilitiesAvailability
        this.artificialLift = data.artificialLift
        this.maturity = data.maturity
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }
}
