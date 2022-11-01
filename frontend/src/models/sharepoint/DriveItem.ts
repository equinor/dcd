import IdentitySet = Components.Schemas.IdentitySet;
import SharepointIds = Components.Schemas.SharepointIds;

export class DriveItem implements Components.Schemas.DriveItemDto {
    name?: string | null
    id?: string | null
    sharepointFileUrl?: string | null
    createdDateTime?: string | null
    size?: number | null
    sharepointIds?: SharepointIds
    createdBy?: IdentitySet
    lastModifiedBy?: IdentitySet
    lastModifiedDateTime?: string | null

    constructor(data: Components.Schemas.DriveItemDto) {
        this.name = data.name ?? null
        this.id = data.id ?? null
        this.sharepointFileUrl = data.sharepointFileUrl ?? null
        this.createdDateTime = data.createdDateTime ?? null
        this.size = data.size ?? 0
        this.sharepointIds = data.sharepointIds ?? []
        this.createdBy = data.createdBy ?? []
        this.lastModifiedBy = data.lastModifiedBy ?? []
        this.lastModifiedDateTime = data.lastModifiedDateTime ?? null
    }
}
