import { Typography } from "@equinor/eds-core-react"

import { Section } from "./Components/Shared/SharedWellStyles"
// import ExplorationCosts from "./Components/Exploration/ExplorationCosts"
// import DevelopmentCosts from "./Components/Development/DevelopmentCosts"
import ExplorationWells from "./Components/Exploration/ExplorationWells"
import DevelopmentWells from "./Components/Development/DevelopmentWells"

const WellCostsTab = () => (
    <div>
        <Typography variant="body_long">
            This input is used to calculate each case&apos;s well costs based on their drilling schedules.
        </Typography>
        <Section>
            <ExplorationWells />
            {/* <ExplorationCosts /> */}
        </Section>
        <Section>
            <DevelopmentWells />
            {/* <DevelopmentCosts /> */}
        </Section>
    </div>
)

export default WellCostsTab
