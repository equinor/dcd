import { isExplorationWell } from "@/Utils/common"
import Wells from "../Shared/Wells"
import { WellCategory } from "@/Models/enums"

const ExplorationWells = () => {
    const wellOptions = [
        { key: "4", value: WellCategory.Exploration_Well, label: "Exploration well" },
        { key: "5", value: WellCategory.Appraisal_Well, label: "Appraisal well" },
        { key: "6", value: WellCategory.Sidetrack, label: "Sidetrack" },
    ]

    return (
        <Wells
            title="Exploration Well Costs"
            addButtonText="Add new exploration well type"
            defaultWellCategory={WellCategory.Exploration_Well}
            wellOptions={wellOptions}
            filterWells={isExplorationWell}
        />
    )
}

export default ExplorationWells
