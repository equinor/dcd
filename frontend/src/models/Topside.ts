export class Topside {

    id: string | null
    name: string | null
    projectId: string | null
    costProfile: TopsideCostProfile | null
    dryWeight: number | null
    oilCapacity: number | null
    gasCapacity: number | null
    facilitiesAvailability: number | null
    artificialLift: ArtificialLift | null
    maturity: Maturity | null

    constructor(data: Components.Schemas.TopsideDto) {
        this.id = data.id ?? null
        this.name = data.name ?? null
        this.projectId = data.projectId ?? null
        this.costProfile = data.costProfile ?? null
        this.dryWeight = data.dryWeight ?? null
        this.oilCapacity = data.oilCapacity ?? null
        this.gasCapacity = data.gasCapacity ?? null
        this.facilitiesAvailability = data.facilitiesAvailability ?? null
        this.artificialLift = data.artificialLift ?? null
        this.maturity = data.maturity ?? null
    }

    static fromJSON(data: Components.Schemas.TopsideDto): Topside {
        return new Topside(data)
    }

}

interface TopsideCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}

export class TopsideCostProfile {
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null 

    constructor(data: TopsideCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: TopsideCostProfileConstructor): TopsideCostProfile {
        return new TopsideCostProfile(data)
    }
}

export type ArtificialLift = 0 | 1 | 2 | 3; // int32
export type Maturity = 0 | 1 | 2 | 3; // int32
export type Currency = 0 | 1; // int32