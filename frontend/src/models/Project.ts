import { Case } from "./Case"
import { DrainageStrategy } from "./DrainageStrategy"
import { ProjectCategory } from "./ProjectCategory"
import { ProjectPhase } from "./ProjectPhase"

export type ProjectConstructor = {
    cases: any[]
    country: string
    createdDate: string
    description: string
    drainageStrategies: any[]
    explorations: any[]
    projectId: string
    name: string
    projectCategory: number
    projectPhase: number
    substructures: any[]
    surfs: any[]
    topsides: any[]
    transports: any[]
    wellProjects: any[]
}

export class Project {
    cases: Case[]
    category: ProjectCategory
    country: string
    createdAt: Date
    description: string
    drainageStrategies: DrainageStrategy[]
    explorations: any[]
    id: string
    name: string
    phase: ProjectPhase
    substructures: any[]
    surfs: any[]
    topsides: any[]
    transports: any[]
    wellProjects: any[]

    constructor(data: ProjectConstructor) {
        this.cases = data.cases
        this.category = new ProjectCategory(data.projectCategory)
        this.country = data.country
        this.createdAt = new Date(data.createdDate)
        this.description = data.description
        this.drainageStrategies = data.drainageStrategies.map(DrainageStrategy.fromJSON)
        this.explorations = data.explorations
        this.id = data.projectId
        this.name = data.name
        this.phase = new ProjectPhase(data.projectPhase)
        this.substructures = data.substructures
        this.surfs = data.surfs
        this.topsides = data.topsides
        this.transports = data.transports
        this.wellProjects = data.wellProjects
    }

    static fromJSON(data: ProjectConstructor): Project {
        return new Project(data)
    }
}
