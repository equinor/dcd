import { Section } from "./Components/Shared/SharedWellStyles"
import Wells from "./Components/Shared/Wells"

import { WellCategory } from "@/Models/enums"
import { developmentWellOptions, explorationWellOptions } from "@/Utils/Config/constants"
import { isExplorationWell } from "@/Utils/TableUtils"

const WellCostsTab = () => (
    <div>
        <Section>
            <Wells
                title="Exploration Well Costs"
                addButtonText="Add new exploration well type"
                defaultWellCategory={WellCategory.ExplorationWell}
                wellOptions={explorationWellOptions}
                filterWells={isExplorationWell}
                isExplorationWellTable
            />
        </Section>
        <Section>
            <Wells
                title="Development Well Costs"
                addButtonText="Add new development/drilling well type"
                defaultWellCategory={WellCategory.OilProducer}
                wellOptions={developmentWellOptions}
                filterWells={(well) => !isExplorationWell(well)}
                isExplorationWellTable={false}
            />
        </Section>
    </div>
)

export default WellCostsTab
