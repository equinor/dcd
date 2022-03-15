export class Surf {

    id: string | null
    name: string | null
    projectId: string | null
    costProfile: SurfCostProfile | null
    maturity: Maturity | null
    infieldPipelineSystemLength: number | null
    umbilicalSystemLength: number | null
    artificialLift: ArtificialLift | null
    riserCount: number | null 
    templateCount: number | null
    productionFlowline: ProductionFlowline | null

    constructor(data: Components.Schemas.SurfDto) {
        this.id = data.id ?? ""
        this.name = data.name ?? ""
        this.projectId = data.projectId ?? null
        this.costProfile = data.costProfile ?? null
        this.maturity = data.maturity ?? null
        this.infieldPipelineSystemLength = data.infieldPipelineSystemLength ?? null
        this.umbilicalSystemLength = data.umbilicalSystemLength ?? null
        this.artificialLift = data.artificialLift ?? null
        this.riserCount = data.riserCount ?? null
        this.templateCount = data.templateCount ?? null
        this.productionFlowline = data.productionFlowline ?? null
    }



}

export interface SurfCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Currency /* int32 */;
    sum?: number; // double
}

export class SurfCostProfile {
    startYear?: number | null
    values?: number [] | null
    epaVersion?: string | null
    currency?: Currency | null
    sum?: number | null

    constructor(data: SurfCostProfileConstructor) {
        this.startYear = data.startYear ?? null
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency ?? null
        this.sum = data.sum ?? null
    }

    static fromJSON(data: SurfCostProfileConstructor): SurfCostProfile {
        return new SurfCostProfile(data)
    }

}
export type Currency = 0 | 1; // int32
export type ProductionFlowline = 999; // int32
export type ArtificialLift = 0 | 1 | 2 | 3; // int32
export type Maturity = 0 | 1 | 2 | 3; // int32