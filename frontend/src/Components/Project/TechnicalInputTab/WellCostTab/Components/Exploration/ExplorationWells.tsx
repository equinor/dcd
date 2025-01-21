import { isExplorationWell } from "@/Utils/common"
import Wells from "../Shared/Wells"

const ExplorationWells = () => {
    const wellOptions = [
        { key: "4", value: 4, label: "Exploration well" },
        { key: "5", value: 5, label: "Appraisal well" },
        { key: "6", value: 6, label: "Sidetrack" },
    ]

    return (
        <Wells
            title="Exploration Well Costs"
            addButtonText="Add new exploration well type"
            defaultWellCategory={4}
            wellOptions={wellOptions}
            filterWells={isExplorationWell}
        />
    )
}

export default ExplorationWells
