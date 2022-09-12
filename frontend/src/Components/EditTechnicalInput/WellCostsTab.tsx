import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { Project } from "../../models/Project"
// import OperationalWellCosts from "./OperationalWellCosts"
import WellListEditTechnicalInput from "./WellListEditTechnicalInput"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
}

const WellCostsTab = ({
    project,
    setProject,
}: Props) => (

    <div color="yellow">
        <TopWrapper color="danger">
            <Typography variant="h4">Operational Well Costs</Typography>
        </TopWrapper>
        {/* <OperationalWellCosts project={project} setProject={setProject} title="Exploration  costs" /> */}
        <TopWrapper color="danger">
            <Typography variant="h4">Well Types</Typography>
        </TopWrapper>
        <WellListEditTechnicalInput project={project} setProject={setProject} explorationWells />
        <WellListEditTechnicalInput project={project} setProject={setProject} explorationWells={false} />

    </div>

)

export default WellCostsTab
