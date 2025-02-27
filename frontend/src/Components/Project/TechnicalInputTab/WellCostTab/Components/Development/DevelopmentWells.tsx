import { isExplorationWell, developmentWellOptions } from "@/Utils/common"
import Wells from "../Shared/Wells"
import { WellCategory } from "@/Models/enums"

const DevelopmentWells = () => (
    <Wells
        title="Development Well Costs"
        addButtonText="Add new development/drilling well type"
        defaultWellCategory={WellCategory.Oil_Producer}
        wellOptions={developmentWellOptions}
        filterWells={(well) => !isExplorationWell(well)}
    />
)

export default DevelopmentWells
