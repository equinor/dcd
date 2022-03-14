import { Case } from "./Case"
import { DrainageStrategy } from "./DrainageStrategy"
import { ProjectCategory } from "./ProjectCategory"
import { ProjectPhase } from "./ProjectPhase"

export class Project {
    cases: Case[]
    category: ProjectCategory | null
    country: string | null
    createdAt: Date | null
    description: string | null
    drainageStrategies: DrainageStrategy[]
    explorations: any[]
    id: string
    name: string
    phase: ProjectPhase | null
    substructures: any[]
    surfs: any[]
    topsides: any[]
    transports: any[]
    wellProjects: any[]

    constructor(data: Components.Schemas.ProjectDto) {
        this.cases = data.cases?.map(Case.fromJSON) ?? []
        this.category = data.projectCategory ? new ProjectCategory(data.projectCategory) : null
        this.country = data.country ?? null
        this.createdAt = data.createDate ? new Date(data.createDate) : null
        this.description = data.description ?? null
        this.drainageStrategies = data.drainageStrategies?.map(DrainageStrategy.fromJSON) ?? []
        this.explorations = data.explorations ?? []
        this.id = data.projectId ?? ""
        this.name = data.name ?? ""
        this.phase = data.projectPhase ? new ProjectPhase(data.projectPhase) : null
        this.substructures = data.substructures ?? []
        this.surfs = data.surfs ?? []
        this.topsides = data.topsides ?? []
        this.transports = data.transports ?? []
        this.wellProjects = data.wellProjects ?? []
    }

    static fromJSON(data: Components.Schemas.ProjectDto): Project {
        return new Project(data)
    }

    public static deserialize(data: string): Project[] {
        return JSON.parse(data, Project.parseComplexFields)
    }

    private static parseComplexFields(key: any, value: any): any {
        if (key === "createdAt" && typeof value === "string") {
            return new Date(value)
        }
        if (key === "phase" && typeof value === "object" && value) {
            return ProjectPhase.parseJSON(value)
        }

        return value
    }
}
