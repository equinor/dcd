export class Case {
    capex?: number
    createdAt?: Date | null
    description?: string
    firstDecisionGate?: Date | null
    secondDecisionGate?: Date | null
    thirdDecisionGate?: Date | null
    lastDecisionGate?: Date | null
    id?: string
    updatedAt?: Date | null
    name?: string
    isRef: boolean
    links?: Record<string, string>

    constructor(data: Components.Schemas.CaseDto) {
        this.capex = data.capex
        this.createdAt = data.createTime ? new Date(data.createTime) : null
        this.description = data.description ?? ""
        this.firstDecisionGate = data.dG1Date ? new Date(data.dG1Date) : null
        this.secondDecisionGate = data.dG2Date ? new Date(data.dG2Date) : null
        this.thirdDecisionGate = data.dG3Date ? new Date(data.dG3Date) : null
        this.lastDecisionGate = data.dG4Date ? new Date(data.dG4Date) : null
        this.id = data.id
        this.updatedAt = data.modifyTime ? new Date(data.modifyTime) : null
        this.name = data.name ?? ""
        this.isRef = data.referenceCase ?? false
        this.links = Object.entries(data)
            .filter(([key]) => key.toLowerCase().endsWith("link"))
            .reduce((tmp, [key, value]) => {
                if (key in tmp) return tmp
                return { ...tmp, [key]: value }
            }, {})
    }

    static fromJSON(data: Components.Schemas.CaseDto): Case {
        return new Case(data)
    }
}
