export class Topside implements Components.Schemas.TopsideDto{

    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: Components.Schemas.TopsideCostProfileDto | undefined
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
        this.costProfile = data.costProfile 
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

export class TopsideCostProfile implements Components.Schemas.TopsideCostProfileDto{
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data: Components.Schemas.TopsideCostProfileDto) {
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data: Components.Schemas.TopsideCostProfileDto): TopsideCostProfile {
        return new TopsideCostProfile(data)
    }
}