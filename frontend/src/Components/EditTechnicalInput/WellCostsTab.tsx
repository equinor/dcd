import { Dispatch, SetStateAction } from "react"
import { Typography } from "@equinor/eds-core-react"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"
import Grid from "@mui/material/Grid"

interface Props {
    developmentOperationalWellCosts: Components.Schemas.DevelopmentOperationalWellCostsDto | undefined
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.DevelopmentOperationalWellCostsDto | undefined>>

    explorationOperationalWellCosts: Components.Schemas.ExplorationOperationalWellCostsDto | undefined
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<Components.Schemas.ExplorationOperationalWellCostsDto | undefined>>

    wellProjectWells: Components.Schemas.WellDto[]
    setWellProjectWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>

    explorationWells: Components.Schemas.WellDto[]
    setExplorationWells: Dispatch<SetStateAction<Components.Schemas.WellDto[]>>

    setDeletedWells: Dispatch<SetStateAction<string[]>>
}

const WellCostsTab = ({
    developmentOperationalWellCosts, setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts, setExplorationOperationalWellCosts,
    wellProjectWells, setWellProjectWells,
    explorationWells, setExplorationWells,
    setDeletedWells,
}: Props) => (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography variant="body_long">
                    This input is used to calculate each case&apos;s well costs based on their drilling schedules.
                </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
                <WellListEditTechnicalInput
                    explorationWells
                    setWells={setExplorationWells}
                    wells={explorationWells}
                    setDeletedWells={setDeletedWells}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <OperationalWellCosts
                    title="Exploration costs"
                    explorationOperationalWellCosts={explorationOperationalWellCosts}
                    setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <WellListEditTechnicalInput
                    explorationWells={false}
                    setWells={setWellProjectWells}
                    wells={wellProjectWells}
                    setDeletedWells={setDeletedWells}
                />
            </Grid>
            <Grid item xs={12} md={6}>
                <OperationalWellCosts
                    title="Development costs"
                    developmentOperationalWellCosts={developmentOperationalWellCosts}
                    setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
                />
            </Grid>
        </Grid>
)

export default WellCostsTab
