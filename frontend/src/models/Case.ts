export type CaseConstructor = {
    capex: number
    createTime: string
    description: string
    dG4Date: string
    drainageStrategyLink: string
    explorationLink: string
    id: string
    modifyTime: string
    name: string
    projectId: string
    referenceCase: boolean
    substructureLink: string
    surfLink: string
    topsideLink: string
    transportLink: string
    wellProjectLink: string
}

export class Case {
    capex: number
    createdAt: Date
    description: string
    lastDecisionGate: Date
    id: string
    modifiedAt: Date
    name: string
    isRef: boolean
    links: Record<string, string>

    constructor(data: CaseConstructor) {
        this.capex = data.capex
        this.createdAt = new Date(data.createTime)
        this.description = data.description
        this.lastDecisionGate = new Date(data.dG4Date)
        this.id = data.id
        this.modifiedAt = new Date(data.modifyTime)
        this.name = data.name
        this.isRef = data.referenceCase
        this.links = Object.entries(data)
            .filter(([key]) => key.toLowerCase().endsWith('link'))
            .reduce((tmp, [key, value]) => {
                if (key in tmp) return tmp
                return { ...tmp, [key]: value }
            }, {})
    }

    static fromJSON(data: CaseConstructor): Case {
        return new Case(data)
    }
}
