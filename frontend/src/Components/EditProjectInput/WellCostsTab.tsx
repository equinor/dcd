import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { Project } from "../../models/Project"
import WellListEditProject from "./WellListEditProject"

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
            <Typography variant="h4">Production Profiles</Typography>
            <WellListEditProject project={project} setProject={setProject} />
        </TopWrapper>

    </div>

)

export default WellCostsTab
