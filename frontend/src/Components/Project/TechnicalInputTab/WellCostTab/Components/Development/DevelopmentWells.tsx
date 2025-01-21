import { isExplorationWell } from "@/Utils/common"
import Wells from "../Shared/Wells"

const DevelopmentWells = () => {
    const wellOptions = [
        { key: "0", value: 0, label: "Oil producer" },
        { key: "1", value: 1, label: "Gas producer" },
        { key: "2", value: 2, label: "Water injector" },
        { key: "3", value: 3, label: "Gas injector" },
    ]

    return (
        <Wells
            title="Development Well Costs"
            addButtonText="Add new development/drilling well type"
            defaultWellCategory={0}
            wellOptions={wellOptions}
            filterWells={(well) => !isExplorationWell(well)}
        />
    )
}

export default DevelopmentWells
