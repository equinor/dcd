import { Case } from "./case/Case"
import { DrainageStrategy } from "./assets/drainagestrategy/DrainageStrategy"
import { Exploration } from "./assets/exploration/Exploration"
import { ProjectPhase } from "./ProjectPhase"
import { Substructure } from "./assets/substructure/Substructure"
import { Surf } from "./assets/surf/Surf"
import { Topside } from "./assets/topside/Topside"
import { Transport } from "./assets/transport/Transport"
import { WellProject } from "./assets/wellproject/WellProject"
import { Well } from "./Well"
import { ExplorationOperationalWellCosts } from "./ExplorationOperationalWellCosts"
import { DevelopmentOperationalWellCosts } from "./DevelopmentOperationalWellCosts"

export class Project implements Components.Schemas.ProjectDto {
    cases: Case[]
    category?: Components.Schemas.ProjectCategory
    country: string | null
    createdAt: Date | null
    description: string | null
    drainageStrategies: DrainageStrategy[]
    explorations: Exploration[]
    id: string
    name: string
    phase?: Components.Schemas.ProjectPhase
    substructures: Substructure[]
    surfs: Surf[]
    topsides: Topside[]
    transports: Transport[]
    wellProjects: WellProject[]
    commonLibraryId: string
    commonLibraryName: string
    referenceCaseId?: string
    currency: Components.Schemas.Currency
    physUnit: Components.Schemas.PhysUnit
    wells?: Well[] | undefined
    explorationWellCosts?: ExplorationOperationalWellCosts
    developmentWellCosts?: DevelopmentOperationalWellCosts
    sharepointSiteUrl?: string | null
    cO2RemovedFromGas?: number // double
    cO2EmissionFromFuelGas?: number // double
    flaredGasPerProducedVolume?: number // double
    cO2EmissionsFromFlaredGas?: number // double
    cO2Vented?: number // double
    dailyEmissionFromDrillingRig?: number // double
    averageDevelopmentDrillingDays?: number // double

    constructor(data: Components.Schemas.ProjectDto) {
        this.cases = data.cases?.map(Case.fromJSON) ?? []
        this.category = data.projectCategory
        this.country = data.country ?? null
        this.createdAt = data.createDate ? new Date(data.createDate) : null
        this.description = data.description ?? null
        this.drainageStrategies = data.drainageStrategies?.map(DrainageStrategy.fromJSON) ?? []
        this.explorations = data.explorations?.map(Exploration.fromJSON) ?? []
        this.id = data.id ?? ""
        this.commonLibraryId = data.commonLibraryId ?? ""
        this.commonLibraryName = data.commonLibraryName ?? ""
        this.referenceCaseId = data.referenceCaseId ?? ""
        this.name = data.name ?? ""
        this.phase = data.projectPhase
        this.substructures = data.substructures?.map(Substructure.fromJSON) ?? []
        this.surfs = data.surfs?.map(Surf.fromJSON) ?? []
        this.topsides = data.topsides?.map(Topside.fromJSON) ?? []
        this.transports = data.transports?.map(Transport.fromJSON) ?? []
        this.wellProjects = data.wellProjects?.map(WellProject.fromJSON) ?? []
        this.currency = data.currency ?? 1
        this.physUnit = data.physUnit ?? 0
        this.wells = data.wells?.map(Well.fromJSON) ?? []
        this.explorationWellCosts = ExplorationOperationalWellCosts.fromJSON(data.explorationOperationalWellCosts)
        this.developmentWellCosts = DevelopmentOperationalWellCosts.fromJSON(data.developmentOperationalWellCosts)
        this.sharepointSiteUrl = data.sharepointSiteUrl
        this.cO2RemovedFromGas = data.cO2RemovedFromGas ?? 0
        this.cO2EmissionFromFuelGas = data.cO2EmissionFromFuelGas ?? 0
        this.flaredGasPerProducedVolume = data.flaredGasPerProducedVolume ?? 0
        this.cO2EmissionsFromFlaredGas = data.cO2EmissionsFromFlaredGas ?? 0
        this.cO2Vented = data.cO2Vented ?? 0
        this.dailyEmissionFromDrillingRig = data.dailyEmissionFromDrillingRig ?? 0
        this.averageDevelopmentDrillingDays = data.averageDevelopmentDrillingDays ?? 0
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
