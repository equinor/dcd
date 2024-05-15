import { ImportStatusEnum } from "./ImportStatusEnum"

export default class SharePointImport
implements Components.Schemas.SharePointImportDto {
    id?: string | undefined
    selected?: boolean | undefined
    surfState?: ImportStatusEnum | undefined
    substructureState?: ImportStatusEnum | undefined
    topsideState?: ImportStatusEnum | undefined
    transportState?: ImportStatusEnum | undefined
    sharePointFileId?: string | null
    sharePointFileName?: string | null
    sharePointFileUrl?: string | null
    sharePointSiteUrl?: string | undefined

    constructor(
        projectCase: Components.Schemas.CaseDto,
        project: Components.Schemas.ProjectDto,
        data: Components.Schemas.SharePointImportDto | undefined,
    ) {
        this.id = projectCase.id!
        this.selected = false
        this.surfState = SharePointImport.surfStatus(projectCase, project)
        this.substructureState = SharePointImport.substructureStatus(
            projectCase,
            project,
        )
        this.topsideState = SharePointImport.topsideStatus(projectCase, project)
        this.transportState = SharePointImport.transportStatus(
            projectCase,
            project,
        )
        this.sharePointFileName = data?.sharePointFileName ?? ""
        this.sharePointFileId = data?.sharePointFileId ?? ""
        this.sharePointFileUrl = data?.sharePointFileUrl ?? ""
        this.sharePointSiteUrl = data?.sharePointSiteUrl ?? ""
    }

    static mapSource = (source: Components.Schemas.Source | undefined) => (source === 0 ? "ConceptApp" : "PROSP")

    static surfStatus = (
        projectCase: Components.Schemas.CaseDto,
        project: Components.Schemas.ProjectDto,
    ): ImportStatusEnum => {
        const surfId = projectCase.surfLink
        const surf = project.surfs.find((s) => s.id === surfId)
        if (!surf) {
            return ImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(surf.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ImportStatusEnum.NotSelected
        }

        return ImportStatusEnum.Selected
    }

    static substructureStatus = (
        projectCase: Components.Schemas.CaseDto,
        project: Components.Schemas.ProjectDto,
    ): ImportStatusEnum => {
        const substructureId = projectCase.substructureLink
        const substructure = project.substructures.find(
            (s) => s.id === substructureId,
        )
        if (!substructure) {
            return ImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(substructure.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ImportStatusEnum.NotSelected
        }

        return ImportStatusEnum.Selected
    }

    static topsideStatus = (
        projectCase: Components.Schemas.CaseDto,
        project: Components.Schemas.ProjectDto,
    ): ImportStatusEnum => {
        const topsideId = projectCase.topsideLink
        const topside = project.topsides.find((s) => s.id === topsideId)
        if (!topside) {
            return ImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(topside.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ImportStatusEnum.NotSelected
        }

        return ImportStatusEnum.Selected
    }

    static transportStatus = (
        projectCase: Components.Schemas.CaseDto,
        project: Components.Schemas.ProjectDto,
    ): ImportStatusEnum => {
        const transportId = projectCase.transportLink
        const transport = project.transports.find((s) => s.id === transportId)
        if (!transport) {
            return ImportStatusEnum.NotSelected
        }
        if (
            SharePointImport.mapSource(transport.source) === "ConceptApp"
            && projectCase.sharepointFileName !== ""
        ) {
            return ImportStatusEnum.NotSelected
        }

        return ImportStatusEnum.Selected
    }

    static toDto = (
        value: SharePointImport,
    ): Components.Schemas.SharePointImportDto => {
        const dto: Components.Schemas.SharePointImportDto = { ...value }
        dto.surf = value.surfState === ImportStatusEnum.Selected
        dto.substructure = value.substructureState === ImportStatusEnum.Selected
        dto.topside = value.topsideState === ImportStatusEnum.Selected
        dto.transport = value.transportState === ImportStatusEnum.Selected

        return dto
    }
}
