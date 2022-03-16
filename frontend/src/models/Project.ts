import { Case } from "./Case"
import { DrainageStrategy } from "./assets/drainagestrategy/DrainageStrategy"
import { Exploration } from "./assets/exploration/Exploration"
import { ProjectCategory } from "./ProjectCategory"
import { ProjectPhase } from "./ProjectPhase"
import { Substructure } from "./assets/substructure/Substructure"
import { Surf } from "./assets/surf/Surf"
import { Topside } from "./assets/topside/Topside"
import { Transport } from "./assets/transport/Transport"
import { Wellproject } from "./assets/wellproject/Wellproject"

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
    substructures: Substructure[]
    surfs: Surf[]
    topsides: Topside[]
    transports: Transport[]
    wellProjects: Wellproject[]

    constructor(data: Components.Schemas.ProjectDto) {
        this.cases = data.cases?.map(Case.fromJSON) ?? []
        this.category = data.projectCategory ? new ProjectCategory(data.projectCategory) : null
        this.country = data.country ?? null
        this.createdAt = data.createDate ? new Date(data.createDate) : null
        this.description = data.description ?? null
        this.drainageStrategies = data.drainageStrategies?.map(DrainageStrategy.fromJSON) ?? []
        this.explorations = data.explorations?.map(Exploration.fromJSON) ?? []
        this.id = data.projectId ?? ""
        this.name = data.name ?? ""
        this.phase = data.projectPhase ? new ProjectPhase(data.projectPhase) : null
        this.substructures = data.substructures?.map(Substructure.fromJSON) ?? []
        this.surfs = data.surfs?.map(Surf.fromJSON) ?? []
        this.topsides = data.topsides?.map(Topside.fromJSON) ?? []
        this.transports = data.transports?.map(Transport.fromJSON) ?? []
        this.wellProjects = data.wellProjects?.map(Wellproject.fromJSON) ?? []
    }

    static fromJSON(data: Components.Schemas.ProjectDto): Project {
        return new Project(data)
    }

    static readonly recentProjectsKey = "recentProjects"

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

    static retrieveRecentProjects() {
        const recentProjectJSON = localStorage.getItem(Project.recentProjectsKey)
        const recentProjects: Project[] = Project.deserialize(recentProjectJSON ?? "[]")
        return recentProjects
    }

    static storeRecentProject(recentProject: Project) {
        let currentRecentProjects = Project.retrieveRecentProjects()
        // find possible duplicate, remove it
        const projectAlreadyNotedIndex = currentRecentProjects.findIndex(
            (recordedProject) => recordedProject.id === recentProject.id,
        )
        if (projectAlreadyNotedIndex >= 0) {
            currentRecentProjects = currentRecentProjects
                .slice(0, projectAlreadyNotedIndex)
                .concat(
                    currentRecentProjects.slice(projectAlreadyNotedIndex + 1),
                )
        }
        currentRecentProjects.unshift(recentProject)
        const recentProjects = currentRecentProjects.slice(0, 4)
        localStorage.setItem(Project.recentProjectsKey, JSON.stringify(recentProjects))
    }
}
