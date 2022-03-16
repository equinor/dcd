export class Surf implements Components.Schemas.SurfDto {

    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: Components.Schemas.SurfCostProfileDto | undefined
    maturity?: Components.Schemas.Maturity | undefined
    infieldPipelineSystemLength?: number | undefined
    umbilicalSystemLength?: number | undefined
    artificialLift?: Components.Schemas.ArtificialLift | undefined
    riserCount?: number | undefined
    templateCount?: number | undefined
    productionFlowline?: Components.Schemas.ProductionFlowline | undefined

    constructor(data: Components.Schemas.SurfDto) {
        this.id = data.id ?? ""
        this.name = data.name ?? ""
        this.projectId = data.projectId
        this.costProfile = data.costProfile
        this.maturity = data.maturity
        this.infieldPipelineSystemLength = data.infieldPipelineSystemLength
        this.umbilicalSystemLength = data.umbilicalSystemLength
        this.artificialLift = data.artificialLift 
        this.riserCount = data.riserCount 
        this.templateCount = data.templateCount
        this.productionFlowline = data.productionFlowline
    }

    static fromJSON(data: Components.Schemas.SurfDto): Surf {
        return new Surf(data)
    }

}

export interface SurfCostProfileConstructor {
    startYear?: number; // int32
    values?: number /* double */[] | null;
    epaVersion?: string | null;
    currency?: Components.Schemas.Currency /* int32 */;
    sum?: number; // double
}

export class SurfCostProfile implements Components.Schemas.SurfCostProfileDto{
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data: Components.Schemas.SurfCostProfileDto) {
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data: Components.Schemas.SurfCostProfileDto): SurfCostProfile {
        return new SurfCostProfile(data)
    }

}