import { isExplorationWell, explorationWellOptions } from "@/Utils/common"
import Wells from "../Shared/Wells"
import { WellCategory } from "@/Models/enums"

const ExplorationWells = () => (
    <Wells
        title="Exploration Well Costs"
        addButtonText="Add new exploration well type"
        defaultWellCategory={WellCategory.Exploration_Well}
        wellOptions={explorationWellOptions}
        filterWells={isExplorationWell}
    />
)

export default ExplorationWells
