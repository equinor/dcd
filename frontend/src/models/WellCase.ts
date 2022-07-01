import { EMPTY_GUID } from "../Utils/constants"
import { DrillingSchedule } from "./assets/wellproject/DrillingSchedule"

export class WellCase implements Components.Schemas.WellCaseDto {
    count?: number
    drillingSchedule?: DrillingSchedule
    caseId?: string
    wellId?: string

    constructor(data?: Components.Schemas.WellCaseDto) {
        if (data !== undefined) {
            this.count = data.count
            this.drillingSchedule = DrillingSchedule.fromJSON(data.drillingSchedule)
            this.caseId = data.caseId
            this.wellId = data.wellId
        }
    }

    static fromJSON(data: Components.Schemas.WellCaseDto): WellCase {
        return new WellCase(data)
    }
}
