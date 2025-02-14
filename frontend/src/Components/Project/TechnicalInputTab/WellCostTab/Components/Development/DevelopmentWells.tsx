import { isExplorationWell } from "@/Utils/common"
import Wells from "../Shared/Wells"
import { WellCategory } from "@/Models/enums"

const DevelopmentWells = () => {
    const wellOptions = [
        { key: "0", value: WellCategory.Oil_Producer, label: "Oil producer" },
        { key: "1", value: WellCategory.Gas_Producer, label: "Gas producer" },
        { key: "2", value: WellCategory.Water_Injector, label: "Water injector" },
        { key: "3", value: WellCategory.Gas_Injector, label: "Gas injector" },
    ]

    return (
        <Wells
            title="Development Well Costs"
            addButtonText="Add new development/drilling well type"
            defaultWellCategory={WellCategory.Oil_Producer}
            wellOptions={wellOptions}
            filterWells={(well) => !isExplorationWell(well)}
        />
    )
}

export default DevelopmentWells
