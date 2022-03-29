import { TopsideCostProfile } from "./TopsideCostProfile"

export class Topside implements Components.Schemas.TopsideDto {
    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    topsideCostProfile?: TopsideCostProfile | undefined
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
            this.topsideCostProfile = TopsideCostProfile.fromJSON(data.costProfile)
            this.dryWeight = data.dryWeight
            this.maturity = data.maturity
        } else {
            this.id = "00000000-0000-0000-0000-000000000000"
            this.name = ""
            this.topsideCostProfile = new TopsideCostProfile()
            this.topsideCostProfile.epaVersion = ""
        }
    }

    static Copy(data: Topside) {
        const topsideCopy = new Topside(data)
        return {
            ...topsideCopy,
            topsideCostProfile: data.topsideCostProfile,
        }
    }

    static ToDto(data: Topside): Components.Schemas.TopsideDto {
        const topsideCopy = new Topside(data)
        return {
            ...topsideCopy,
            costProfile: data.topsideCostProfile,
        }
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }
}
