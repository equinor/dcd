import { Case } from "./Case"
import { DrainageStrategy } from "./assets/drainagestrategy/DrainageStrategy"
import { Exploration } from "./assets/exploration/Exploration"
import { ProjectCategory } from "./ProjectCategory"
import { ProjectPhase } from "./ProjectPhase"
import { Substructure } from "./assets/substructure/Substructure"
import { Surf } from "./assets/surf/Surf"
import { Topside } from "./assets/topside/Topside"
import { Transport } from "./assets/transport/Transport"
import { WellProject } from "./assets/wellproject/WellProject"

export class Project implements Components.Schemas.ProjectDto {
    cases: Case[]
    category?: Components.Schemas.ProjectCategory
    country: string | null
    createdAt: Date | null
    description: string | null
    drainageStrategies: DrainageStrategy[]
    explorations: any[]
    id: string
    projectId: string
    name: string
    phase?: Components.Schemas.ProjectPhase
    substructures: Substructure[]
    surfs: Surf[]
    topsides: Topside[]
    transports: Transport[]
    wellProjects: WellProject[]
    commonLibId: string
    commonLibraryName: string
    currency: Components.Schemas.Currency
    physUnit: Components.Schemas.PhysUnit

    constructor(data: Components.Schemas.ProjectDto) {
        this.cases = data.cases?.map(Case.fromJSON) ?? []
        this.category = data.projectCategory
        this.country = data.country ?? null
        this.createdAt = data.createDate ? new Date(data.createDate) : null
        this.description = data.description ?? null
        this.drainageStrategies = data.drainageStrategies?.map(DrainageStrategy.fromJSON) ?? []
        this.explorations = data.explorations?.map(Exploration.fromJSON) ?? []
        this.id = data.projectId ?? ""
        this.projectId = data.projectId ?? ""
        this.commonLibId = data.commonLibraryId ?? ""
        this.commonLibraryName = data.commonLibraryName ?? ""
        this.name = data.name ?? ""
        this.phase = data.projectPhase
        this.substructures = data.substructures?.map(Substructure.fromJSON) ?? []
        this.surfs = data.surfs?.map(Surf.fromJSON) ?? []
        this.topsides = data.topsides?.map(Topside.fromJSON) ?? []
        this.transports = data.transports?.map(Transport.fromJSON) ?? []
        this.wellProjects = data.wellProjects?.map(WellProject.fromJSON) ?? []
        this.currency = data.currency ?? 0
        this.physUnit = data.physUnit ?? 0
    }

    static fromJSON(data: Components.Schemas.ProjectDto): Project {
        return new Project(data)
    }

    static Copy(data: Project): Components.Schemas.ProjectDto {
        const projectCopy = new Project(data)
        return {
            ...projectCopy,
            projectCategory: data.category,
            projectPhase: data.phase,
        }
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
