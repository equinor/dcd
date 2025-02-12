import { Source } from "../enums"

export default class SharePointImport
implements Components.Schemas.SharePointImportDto {
    id?: string | undefined
    selected?: boolean | undefined
    sharePointFileId?: string | null
    sharePointFileName?: string | null
    sharePointFileUrl?: string | null
    sharePointSiteUrl?: string | undefined

    constructor(
        projectCase: Components.Schemas.CaseOverviewDto,
        data: Components.Schemas.SharePointImportDto | undefined,
    ) {
        this.id = projectCase.caseId!
        this.selected = false
        this.sharePointFileName = data?.sharePointFileName ?? ""
        this.sharePointFileId = data?.sharePointFileId ?? ""
        this.sharePointFileUrl = data?.sharePointFileUrl ?? ""
        this.sharePointSiteUrl = data?.sharePointSiteUrl ?? ""
    }

    static mapSource = (source: Components.Schemas.Source | undefined) => (source === Source.ConceptApp ? "ConceptApp" : "PROSP")

    static toDto = (
        value: SharePointImport,
    ): Components.Schemas.SharePointImportDto => {
        const dto: Components.Schemas.SharePointImportDto = { ...value }
        return dto
    }
}
