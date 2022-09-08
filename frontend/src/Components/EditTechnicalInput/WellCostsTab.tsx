import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { DevelopmentOperationalWellCosts } from "../../models/DevelopmentOperationalWellCosts"
import { ExplorationOperationalWellCosts } from "../../models/ExplorationOperationalWellCosts"
import { Project } from "../../models/Project"
// import DevelopmentOperationalWellCost from "./DevelopmentOperationalWellCost"
// import DevelopmentOperationalWellCosts from "./DevelopmentOperationalWellCosts"
import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
    developmentOperationalWellCosts: DevelopmentOperationalWellCosts
    setDevelopmentOperationalWellCosts: Dispatch<SetStateAction<DevelopmentOperationalWellCosts | undefined>>

    explorationOperationalWellCosts: ExplorationOperationalWellCosts
    setExplorationOperationalWellCosts: Dispatch<SetStateAction<ExplorationOperationalWellCosts | undefined>>

}

const WellCostsTab = ({
    project,
    setProject,
    developmentOperationalWellCosts,
    setDevelopmentOperationalWellCosts,
    explorationOperationalWellCosts,
    setExplorationOperationalWellCosts,
}: Props) => (

    <div color="yellow">
        <TopWrapper color="danger">
            <Typography variant="h4">Operational Well Costs</Typography>
        </TopWrapper>
        <OperationalWellCosts
            title="Exploration costs"
            developmentOperationalWellCosts={developmentOperationalWellCosts}
            setDevelopmentOperationalWellCosts={setDevelopmentOperationalWellCosts}
        />
        <OperationalWellCosts
            title="Development costs"
            explorationOperationalWellCosts={explorationOperationalWellCosts}
            setExplorationOperationalWellCosts={setExplorationOperationalWellCosts}
        />
        {/* <DevelopmentOperationalWellCosts project={project} setProject={setProject} title="Exploration costs" /> */}
        <TopWrapper color="danger">
            <Typography variant="h4">Well Types</Typography>
        </TopWrapper>
        <WellListEditTechnicalInput project={project} setProject={setProject} explorationWells />
        <WellListEditTechnicalInput project={project} setProject={setProject} explorationWells={false} />

    </div>

)

export default WellCostsTab
