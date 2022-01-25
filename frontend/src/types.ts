export type Project = {
    name: string
    id: string
    cases: Case[]
    createdDate: number
}

export type Case = {
    description?: string
    drainageStrategy: DrainageStrategy
    id: string
    name: string
    project?: any
}

export type DrainageStrategy = {
    case?: any
    id: string
    nglYield: number
    productionProfileGas: ProductionProfile
    productionProfileOil: ProductionProfile
}

export type ProductionProfile = {
    id: string
    drainageStrategy?: string
    yearValues: {
        id: string|number
        year: number
        value: number
    }[]
}
