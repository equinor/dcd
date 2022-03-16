export class Transport {
    id: String | null
    name: String | null
    projectId: String | null
    costProfile: TransportCostProfile | null
    maturity:  Maturity | null
    gasExportPipelineLength: number | null
    oilExportPipelineLength: number | null

    constructor(data: Components.Schemas.TransportDto)  
    {
        this.id = data.id ?? null
        this.name = data.name ?? null
        this.projectId = data.projectId ?? null
        this.costProfile = data.costProfile ?? null
        this.maturity = data.maturity ?? null
        this.gasExportPipelineLength = data.gasExportPipelineLength ?? null
        this.oilExportPipelineLength = data.oilExportPipelineLength ?? null
    }

    static fromJSON(data: Components.Schemas.SurfDto): Transport {
        return new Transport(data)
    }

}

export interface TransportCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}

export class TransportCostProfile {
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null

    constructor(data: TransportCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: TransportCostProfileConstructor): TransportCostProfile {
        return new TransportCostProfile(data)
    }
}


export type Maturity = 0 | 1 | 2 | 3; // int32
export type Currency = 0 | 1; // int32