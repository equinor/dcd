import { EMPTY_GUID } from "../../../Utils/constants"
import { ITimeSeries } from "../../ITimeSeries"

export class SeismicAcquisitionAndProcessing implements
Components.Schemas.SeismicAcquisitionAndProcessingDto, ITimeSeries {
    id?: string
    startYear?: number
    name?: string
    values?: number []
    epaVersion?: string
    currency?: Components.Schemas.Currency
    sum?: number

    constructor(data?: Components.Schemas.SeismicAcquisitionAndProcessingDto) {
        if (data !== undefined && data !== null) {
            this.id = data.id
            this.startYear = data.startYear ?? 0
            this.name = "Seismic acquisition and processing"
            this.values = data.values ?? []
            this.epaVersion = data.epaVersion ?? ""
            this.currency = data.currency
            this.sum = data.sum
        } else {
            this.id = EMPTY_GUID
            this.startYear = 0
            this.name = "Seismic acquisition and processing"
            this.values = []
            this.epaVersion = ""
        }
    }

    static fromJSON(data?: Components.Schemas.SeismicAcquisitionAndProcessingDto):
    SeismicAcquisitionAndProcessing | undefined {
        if (data === undefined || data === null) {
            return undefined
        }
        return new SeismicAcquisitionAndProcessing(data)
    }
}
