export class Substructure {
 
    id: string
    name: string
    projectId: string | null
    substructureCostProfile: SubstructureCostProfile | null
    dryweight: number | null
    maturity: Maturity | null

    constructor(data: Components.Schemas.SubstructureDto) {
        this.id = data.id ?? ""
        this.name = data.name ?? "" 
        this.projectId = data.projectId ?? null
        this.substructureCostProfile = data.costProfile ?? null
        this.dryweight = data.dryWeight ?? null
        this.maturity = data.maturity ?? null
    }
}

interface SubstructureCostProfileConstructor {
    startYear?: number
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency
    sum?: number
}

export class SubstructureCostProfile {
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null

    constructor(data: SubstructureCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: SubstructureCostProfileConstructor): SubstructureCostProfile {
        return new SubstructureCostProfile(data)
    }
}

export type Currency = 0 | 1; // int32
export type Maturity = 0 | 1 | 2 | 3; // int32