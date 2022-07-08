import { DrillingSchedule } from "./assets/wellproject/DrillingSchedule"

export class WellProjectWell implements Components.Schemas.WellProjectWellDto {
    count?: number
    drillingSchedule?: DrillingSchedule
    wellProjectId?: string
    wellId?: string

    constructor(data?: Components.Schemas.WellProjectWellDto) {
        if (data !== undefined) {
            this.count = data.count
            this.drillingSchedule = DrillingSchedule.fromJSON(data.drillingSchedule)
            this.wellProjectId = data.wellProjectId
            this.wellId = data.wellId
        }
    }

    static fromJSON(data: Components.Schemas.WellProjectWellDto): WellProjectWell {
        return new WellProjectWell(data)
    }
}
