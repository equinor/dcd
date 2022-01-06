export type Project = {
    name: string
    id: string
    cases: Case[]
    createdDate: number
}

export type Case = {
    title: string
    id: string
    capex: number
    drillex: number
    ur: number
}
