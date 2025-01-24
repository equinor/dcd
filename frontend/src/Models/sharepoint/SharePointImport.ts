import { ProspImportStatusEnum } from "@/Models/ProspImportStatusEnum"

export default class SharePointImport
implements Components.Schemas.SharePointImportDto {
    id?: string | undefined
    selected?: boolean | undefined
    surfState?: ProspImportStatusEnum | undefined
    substructureState?: ProspImportStatusEnum | undefined
    topsideState?: ProspImportStatusEnum | undefined
    transportState?: ProspImportStatusEnum | undefined
    onshorePowerSupplyState?: ProspImportStatusEnum | undefined
    sharePointFileId?: string | null
    sharePointFileName?: string | null
    sharePointFileUrl?: string | null
    sharePointSiteUrl?: string | undefined

    constructor(
        projectCase: Components.Schemas.CaseOverviewDto,
        project: Components.Schemas.ProjectDataDto,
        data: Components.Schemas.SharePointImportDto | undefined,
    ) {
        this.id = projectCase.caseId!
        this.selected = false
        this.surfState = SharePointImport.surfStatus(projectCase, project.commonProjectAndRevisionData)
        this.substructureState = SharePointImport.substructureStatus(
            projectCase,
            project.commonProjectAndRevisionData,
        )
        this.topsideState = SharePointImport.topsideStatus(projectCase, project.commonProjectAndRevisionData)
        this.transportState = SharePointImport.transportStatus(
            projectCase,
            project.commonProjectAndRevisionData,
        )
        this.sharePointFileName = data?.sharePointFileName ?? ""
        this.sharePointFileId = data?.sharePointFileId ?? ""
        this.sharePointFileUrl = data?.sharePointFileUrl ?? ""
        this.sharePointSiteUrl = data?.sharePointSiteUrl ?? ""
    }

    static mapSource = (source: Components.Schemas.Source | undefined) => (source === 0 ? "ConceptApp" : "PROSP")

    static surfStatus = (
        projectCase: Components.Schemas.CaseOverviewDto,
        project: Components.Schemas.CommonProjectAndRevisionDto,
    ): ProspImportStatusEnum => {
        const surfId = projectCase.surfLink
        const surf = project.surfs?.find((s) => s.id === surfId)
        if (!surf) {
            return ProspImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(surf.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ProspImportStatusEnum.NotSelected
        }

        return ProspImportStatusEnum.Selected
    }

    static substructureStatus = (
        projectCase: Components.Schemas.CaseOverviewDto,
        project: Components.Schemas.CommonProjectAndRevisionDto,
    ): ProspImportStatusEnum => {
        const substructureId = projectCase.substructureLink
        const substructure = project.substructures?.find(
            (s) => s.id === substructureId,
        )
        if (!substructure) {
            return ProspImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(substructure.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ProspImportStatusEnum.NotSelected
        }

        return ProspImportStatusEnum.Selected
    }

    static topsideStatus = (
        projectCase: Components.Schemas.CaseOverviewDto,
        project: Components.Schemas.CommonProjectAndRevisionDto,
    ): ProspImportStatusEnum => {
        const topsideId = projectCase.topsideLink
        const topside = project.topsides?.find((s) => s.id === topsideId)
        if (!topside) {
            return ProspImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(topside.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ProspImportStatusEnum.NotSelected
        }

        return ProspImportStatusEnum.Selected
    }

    static transportStatus = (
        projectCase: Components.Schemas.CaseOverviewDto,
        project: Components.Schemas.CommonProjectAndRevisionDto,
    ): ProspImportStatusEnum => {
        const transportId = projectCase.transportLink
        const transport = project.transports?.find((s) => s.id === transportId)
        if (!transport) {
            return ProspImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(transport.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ProspImportStatusEnum.NotSelected
        }

        return ProspImportStatusEnum.Selected
    }

    static toDto = (
        value: SharePointImport,
    ): Components.Schemas.SharePointImportDto => {
        const dto: Components.Schemas.SharePointImportDto = { ...value }
        dto.surf = value.surfState === ProspImportStatusEnum.Selected
        dto.substructure = value.substructureState === ProspImportStatusEnum.Selected
        dto.topside = value.topsideState === ProspImportStatusEnum.Selected
        dto.transport = value.transportState === ProspImportStatusEnum.Selected
        return dto
    }
}
