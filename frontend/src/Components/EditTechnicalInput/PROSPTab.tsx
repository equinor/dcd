import { Typography } from "@material-ui/core"
import React, { Dispatch, SetStateAction } from "react"
import styled from "styled-components"
import { Project } from "../../models/Project"
import PROSPCaseList from "./PROSPCaseList"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    project: Project
}

const PROSPTab = ({
    project,
    setProject,
}: Props) => (

    <div color="yellow">
        <TopWrapper color="danger">
            <Typography variant="h4">PROSP</Typography>
        </TopWrapper>
        <PROSPCaseList project={project} setProject={setProject} />

    </div>

)

export default PROSPTab
