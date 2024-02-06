import { Case } from "./case/Case"
import { ProjectPhase } from "./ProjectPhase"
import { Well } from "./Well"
import { ExplorationOperationalWellCosts } from "./ExplorationOperationalWellCosts"
import { DevelopmentOperationalWellCosts } from "./DevelopmentOperationalWellCosts"

export class Project implements Components.Schemas.ProjectDto {
    cases: Components.Schemas.CaseDto[]
    category?: Components.Schemas.ProjectCategory
    country: string | null
    createdAt: Date | null
    description: string | null
    drainageStrategies: Components.Schemas.DrainageStrategyDto[]
    explorations: Components.Schemas.ExplorationDto[]
    id: string
    name: string
    phase?: Components.Schemas.ProjectPhase
    substructures: Components.Schemas.SubstructureDto[]
    surfs: Components.Schemas.SurfDto[]
    topsides: Components.Schemas.TopsideDto[]
    transports: Components.Schemas.TransportDto[]
    wellProjects: Components.Schemas.WellProjectDto[]
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
        this.drainageStrategies = data.drainageStrategies ?? []
        this.explorations = data.explorations ?? []
        this.id = data.id ?? ""
        this.commonLibraryId = data.commonLibraryId ?? ""
        this.commonLibraryName = data.commonLibraryName ?? ""
        this.referenceCaseId = data.referenceCaseId ?? ""
        this.name = data.name ?? ""
        this.phase = data.projectPhase
        this.substructures = data.substructures ?? []
        this.surfs = data.surfs ?? []
        this.topsides = data.topsides ?? []
        this.transports = data.transports ?? []
        this.wellProjects = data.wellProjects ?? []
        this.currency = data.currency ?? 1
        this.physUnit = data.physUnit ?? 0
        this.wells = data.wells?.map(Well.fromJSON) ?? []
        this.explorationWellCosts = data.explorationOperationalWellCosts
        this.developmentWellCosts = data.developmentOperationalWellCosts
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
