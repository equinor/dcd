export class Case {
    capex?: number
    createdAt?: Date | null
    description?: string
    lastDecisionGate?: Date | null
    id?: string
    updatedAt?: Date | null
    name?: string
    isRef: boolean
    drainageStrategyLink?: string
    explorationLink?: string
    substructureLink?: string
    surfLink?: string
    topsideLink?: string
    transportLink?: string
    wellProjectLink?: string

    constructor(data: Components.Schemas.CaseDto) {
        this.capex = data.capex
        this.createdAt = data.createTime ? new Date(data.createTime) : null
        this.description = data.description ?? ""
        this.lastDecisionGate = data.dG4Date ? new Date(data.dG4Date) : null
        this.id = data.id
        this.updatedAt = data.modifyTime ? new Date(data.modifyTime) : null
        this.name = data.name ?? ""
        this.isRef = data.referenceCase ?? false
        this.drainageStrategyLink = data.drainageStrategyLink ?? ""
        this.explorationLink = data.explorationLink ?? ""
        this.substructureLink = data.substructureLink ?? ""
        this.surfLink = data.surfLink ?? ""
        this.topsideLink = data.topsideLink ?? ""
        this.transportLink = data.transportLink ?? ""
        this.wellProjectLink = data.wellProjectLink ?? ""
    }

    static fromJSON(data: Components.Schemas.CaseDto): Case {
        return new Case(data)
    }
}
