export class Transport implements Components.Schemas.TransportDto{
    id?: string | undefined
    name?: string | null
    projectId?: string | undefined
    costProfile?: Components.Schemas.TransportCostProfileDto | undefined
    maturity?: Components.Schemas.Maturity | undefined
    gasExportPipelineLength?: number | undefined
    oilExportPipelineLength?: number | undefined

    constructor(data: Components.Schemas.TransportDto)  
    {
        this.id = data.id
        this.name = data.name ?? ""
        this.projectId = data.projectId
        this.costProfile = data.costProfile
        this.maturity = data.maturity
        this.gasExportPipelineLength = data.gasExportPipelineLength
        this.oilExportPipelineLength = data.oilExportPipelineLength
    }

    static fromJSON(data: Components.Schemas.SurfDto): Transport {
        return new Transport(data)
    }

}

export class TransportCostProfile implements Components.Schemas.TransportCostProfileDto{
    startYear?: number | undefined
    values?: number [] | null
    epaVersion?: string | null
    currency?: Components.Schemas.Currency | undefined
    sum?: number | undefined

    constructor(data: Components.Schemas.TransportCostProfileDto) {
        this.startYear = data.startYear
        this.values = data.values ?? []
        this.epaVersion = data.epaVersion ?? null
        this.currency = data.currency
        this.sum = data.sum
    }

    static fromJSON(data: Components.Schemas.TransportCostProfileDto): TransportCostProfile {
        return new TransportCostProfile(data)
    }
}