import { DrillingSchedule } from "./assets/wellproject/DrillingSchedule"

export class WellProjectWell implements Components.Schemas.WellProjectWellDto {
    drillingSchedule?: DrillingSchedule
    wellProjectId?: string
    wellId?: string

    constructor(data?: Components.Schemas.WellProjectWellDto) {
        if (data !== undefined) {
            this.drillingSchedule = DrillingSchedule.fromJSON(data.drillingSchedule)
            this.wellProjectId = data.wellProjectId
            this.wellId = data.wellId
        }
    }

    static fromJSON(data: Components.Schemas.WellProjectWellDto): WellProjectWell {
        return new WellProjectWell(data)
    }
}
