import { Case } from "../../models/case/Case"
import { Project } from "../../models/Project"
import { ImportStatusEnum } from "./ImportStatusEnum"

export default class SharePointImport implements Components.Schemas.SharePointImportDto {
    id?: string | undefined
    selected?: boolean | undefined
    surfState?: ImportStatusEnum | undefined
    substructureState?: ImportStatusEnum | undefined
    topsideState?: ImportStatusEnum | undefined
    transportState?: ImportStatusEnum | undefined
    sharePointFileName?: string | undefined
    sharePointFileId?: string | undefined
    sharePointSiteUrl?: string | undefined

    constructor(caseItem: Case, project: Project, data:Components.Schemas.SharePointImportDto | undefined) {
        this.id = caseItem.id!
        this.selected = false
        this.surfState = SharePointImport.surfStatus(caseItem, project)
        this.substructureState = SharePointImport.substructureStatus(caseItem, project)
        this.topsideState = SharePointImport.topsideStatus(caseItem, project)
        this.transportState = SharePointImport.transportStatus(caseItem, project)
        this.sharePointFileName = caseItem.sharepointFileName ?? ""
        this.sharePointFileId = caseItem.sharepointFileId ?? ""
        this.sharePointSiteUrl = data?.sharePointSiteUrl ?? ""
    }

    static mapSource = (source: Components.Schemas.Source | undefined) => (source === 0 ? "ConceptApp" : "PROSP")

    static surfStatus = (caseItem: Case, project: Project): ImportStatusEnum => {
        const surfId = caseItem.surfLink
        const surf = project.surfs.find((s) => s.id === surfId)
        if (!surf) { return ImportStatusEnum.NotSelected }
        if (SharePointImport.mapSource(surf.source) === "PROSP") { return ImportStatusEnum.PROSP }

        return ImportStatusEnum.NotSelected
    }

    static substructureStatus = (caseItem: Case, project: Project): ImportStatusEnum => {
        const surfId = caseItem.substructureLink
        const surf = project.substructures.find((s) => s.id === surfId)
        if (!surf) { return ImportStatusEnum.NotSelected }
        if (SharePointImport.mapSource(surf.source) === "PROSP") { return ImportStatusEnum.PROSP }

        return ImportStatusEnum.NotSelected
    }

    static topsideStatus = (caseItem: Case, project: Project): ImportStatusEnum => {
        const surfId = caseItem.topsideLink
        const surf = project.topsides.find((s) => s.id === surfId)
        if (!surf) { return ImportStatusEnum.NotSelected }
        if (SharePointImport.mapSource(surf.source) === "PROSP") { return ImportStatusEnum.PROSP }

        return ImportStatusEnum.NotSelected
    }

    static transportStatus = (caseItem: Case, project: Project): ImportStatusEnum => {
        const surfId = caseItem.transportLink
        const surf = project.transports.find((s) => s.id === surfId)
        if (!surf) { return ImportStatusEnum.NotSelected }
        if (SharePointImport.mapSource(surf.source) === "PROSP") { return ImportStatusEnum.PROSP }

        return ImportStatusEnum.NotSelected
    }

    static toDto = (value: SharePointImport): Components.Schemas.SharePointImportDto => {
        const dto: Components.Schemas.SharePointImportDto = { ...value }
        dto.surf = value.surfState === ImportStatusEnum.Selected
        dto.substructure = value.substructureState === ImportStatusEnum.Selected
        dto.topside = value.topsideState === ImportStatusEnum.Selected
        dto.transport = value.transportState === ImportStatusEnum.Selected

        return dto
    }
}
