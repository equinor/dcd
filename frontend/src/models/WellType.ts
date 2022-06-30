import { EMPTY_GUID } from "../Utils/constants"

export class WellType implements Components.Schemas.WellTypeDto {
    id?: string | undefined
    name?: string | null | undefined
    category?: Components.Schemas.WellTypeCategoryDto | undefined
    description?: string | null | undefined
    drillingDays?: number | undefined
    wellCost?: number | undefined

    constructor(data?: Components.Schemas.WellTypeDto) {
        if (data !== undefined) {
            this.id = data.id
            this.name = data.name
            this.category = data.category
            this.description = data.description
            this.drillingDays = data.drillingDays
            this.wellCost = data.wellCost
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.WellTypeDto): WellType {
        return new WellType(data)
    }
}
