import { EMPTY_GUID } from "../../../Utils/constants"
import { ExplorationWell } from "../../ExplorationWell"
import { IAsset } from "../IAsset"
import { CountryOfficeCost } from "./CountryOfficeCost"
import { GAndGAdminCost } from "./GAndGAdminCost"
import { SeismicAcquisitionAndProcessing } from "./SeismicAcquisitionAndProcessing"
import { ExplorationWellCostProfile } from "./ExplorationWellCostProfile"
import { AppraisalWellCostProfile } from "./AppraisalWellCostProfile"
import { SidetrackCostProfile } from "./SidetrackCostProfile"

export class Exploration implements Components.Schemas.ExplorationDto, IAsset {
    id?: string | undefined
    projectId?: string | undefined
    name?: string | undefined
    explorationWellCostProfile?: ExplorationWellCostProfile
    appraisalWellCostProfile?: AppraisalWellCostProfile
    sidetrackCostProfile?: SidetrackCostProfile
    gAndGAdminCost?: GAndGAdminCost | undefined
    seismicAcquisitionAndProcessing?: SeismicAcquisitionAndProcessing | undefined
    countryOfficeCost?: CountryOfficeCost | undefined
    rigMobDemob?: number | undefined
    currency?: Components.Schemas.Currency
    explorationWells?: ExplorationWell[] | null

    constructor(data?: Components.Schemas.ExplorationDto) {
        if (data !== undefined) {
            this.id = data.id
            this.projectId = data.projectId
            this.name = data.name ?? ""
            this.explorationWellCostProfile = ExplorationWellCostProfile.fromJSON(data.explorationWellCostProfile)
            this.appraisalWellCostProfile = AppraisalWellCostProfile.fromJSON(data.appraisalWellCostProfile)
            this.sidetrackCostProfile = SidetrackCostProfile.fromJSON(data.sidetrackCostProfile)
            this.gAndGAdminCost = GAndGAdminCost.fromJSON(data.gAndGAdminCost)
            this.seismicAcquisitionAndProcessing = SeismicAcquisitionAndProcessing
                .fromJSON(data.seismicAcquisitionAndProcessing)
            this.countryOfficeCost = CountryOfficeCost.fromJSON(data.countryOfficeCost)
            this.rigMobDemob = data.rigMobDemob
            this.currency = data.currency ?? 1
            this.explorationWells = data.explorationWells?.map((ew) => new ExplorationWell(ew))
        } else {
            this.id = EMPTY_GUID
            this.name = ""
        }
    }

    static fromJSON(data: Components.Schemas.ExplorationDto): Exploration {
        return new Exploration(data)
    }
}
