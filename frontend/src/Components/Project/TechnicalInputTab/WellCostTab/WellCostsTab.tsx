import { Typography } from "@equinor/eds-core-react"

import { Section } from "./Components/Shared/SharedWellStyles"
import ExplorationCosts from "./Components/Costs/ExplorationCosts"
import DevelopmentCosts from "./Components/Costs/DevelopmentCosts"
import Wells from "./Components/Shared/Wells"
import { WellCategory } from "@/Models/enums"
import { developmentWellOptions, explorationWellOptions, isExplorationWell } from "@/Utils/common"

const WellCostsTab = () => (
    <div>
        <Typography variant="body_long">
            This input is used to calculate each case&apos;s well costs based on their drilling schedules.
        </Typography>
        <Section>
            <Wells
                title="Exploration Well Costs"
                addButtonText="Add new exploration well type"
                defaultWellCategory={WellCategory.Exploration_Well}
                wellOptions={explorationWellOptions}
                filterWells={isExplorationWell}
            />
            <ExplorationCosts />
        </Section>
        <Section>
            <Wells
                title="Development Well Costs"
                addButtonText="Add new development/drilling well type"
                defaultWellCategory={WellCategory.Oil_Producer}
                wellOptions={developmentWellOptions}
                filterWells={(well) => !isExplorationWell(well)}
            />
            <DevelopmentCosts />
        </Section>
    </div>
)

export default WellCostsTab
