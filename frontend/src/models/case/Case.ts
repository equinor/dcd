import { DefaultDate } from "../../Utils/common"
import { EMPTY_GUID } from "../../Utils/constants"
import { CaseCessationCostProfile } from "./CaseCessationCostProfile"

export class Case implements Components.Schemas.CaseDto {
    capex?: number
    capexYear?: Components.Schemas.CapexYear
    createdAt?: Date | null
    description?: string
    DGADate: Date // date-time
    DGBDate: Date // date-time
    DGCDate: Date // date-time
    APXDate: Date // date-time
    APZDate: Date // date-time
    DG0Date: Date // date-time
    DG1Date: Date // date-time
    DG2Date: Date // date-time
    DG3Date: Date // date-time
    DG4Date: Date // date-time
    id: string
    projectId?: string
    updatedAt?: Date | null
    name?: string
    referenceCase: boolean
    drainageStrategyLink?: string
    explorationLink?: string
    substructureLink?: string
    surfLink?: string
    topsideLink?: string
    transportLink?: string
    wellProjectLink?: string
    artificialLift: Components.Schemas.ArtificialLift
    producerCount?: number
    gasInjectorCount?: number
    waterInjectorCount?: number
    facilitiesAvailability?: number
    capexFactorFeasibilityStudies: number // double
    capexFactorFEEDStudies: number // double
    npv: number // double
    breakEven: number // double
    productionStrategyOverview: Components.Schemas.ProductionStrategyOverview
    cessationCost?: CaseCessationCostProfile
    sharepointFileId?: string | null
    sharepointFileName?: string | null
    sharepointFileUrl?: string | null

    constructor(data: Components.Schemas.CaseDto) {
        this.capex = data.capex
        this.capexYear = data.capexYear
        this.createdAt = data.createTime ? new Date(data.createTime) : null
        this.description = data.description ?? ""
        this.DGADate = data.dgaDate ? new Date(data.dgaDate) : DefaultDate()
        this.DGBDate = data.dgbDate ? new Date(data.dgbDate) : DefaultDate()
        this.DGCDate = data.dgcDate ? new Date(data.dgcDate) : DefaultDate()
        this.APXDate = data.apxDate ? new Date(data.apxDate) : DefaultDate()
        this.APZDate = data.apzDate ? new Date(data.apzDate) : DefaultDate()
        this.DG0Date = data.dG0Date ? new Date(data.dG0Date) : DefaultDate()
        this.DG1Date = data.dG1Date ? new Date(data.dG1Date) : DefaultDate()
        this.DG2Date = data.dG2Date ? new Date(data.dG2Date) : DefaultDate()
        this.DG3Date = data.dG3Date ? new Date(data.dG3Date) : DefaultDate()
        this.DG4Date = data.dG4Date ? new Date(data.dG4Date) : DefaultDate()
        this.id = data.id ?? EMPTY_GUID
        this.projectId = data.projectId
        this.updatedAt = data.modifyTime ? new Date(data.modifyTime) : null
        this.name = data.name ?? ""
        this.referenceCase = data.referenceCase ?? false
        this.drainageStrategyLink = data.drainageStrategyLink ?? ""
        this.explorationLink = data.explorationLink ?? ""
        this.substructureLink = data.substructureLink ?? ""
        this.surfLink = data.surfLink ?? ""
        this.topsideLink = data.topsideLink ?? ""
        this.transportLink = data.transportLink ?? ""
        this.wellProjectLink = data.wellProjectLink ?? ""
        this.artificialLift = data.artificialLift ?? 0
        this.producerCount = data.producerCount
        this.gasInjectorCount = data.gasInjectorCount
        this.waterInjectorCount = data.waterInjectorCount
        this.facilitiesAvailability = data.facilitiesAvailability
        this.capexFactorFeasibilityStudies = data.capexFactorFeasibilityStudies ?? 0
        this.capexFactorFEEDStudies = data.capexFactorFEEDStudies ?? 0
        this.npv = data.npv ?? 0
        this.breakEven = data.breakEven ?? 0
        this.productionStrategyOverview = data.productionStrategyOverview ?? 0
        this.cessationCost = CaseCessationCostProfile.fromJSON(data.cessationCost)
        this.sharepointFileId = data.sharepointFileId ?? ""
        this.sharepointFileName = data.sharepointFileName ?? ""
    }

    static Copy(data: Case) {
        const caseCopy: Case = new Case(data)
        return {
            ...caseCopy,
            DG0Date: data.DG0Date,
            DG1Date: data.DG1Date,
            DG2Date: data.DG2Date,
            DG3Date: data.DG3Date,
            DG4Date: data.DG4Date,
        }
    }

    static fromJSON(data: Components.Schemas.CaseDto): Case {
        return new Case(data)
    }
}
