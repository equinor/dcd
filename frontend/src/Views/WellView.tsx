import React, { useEffect, useState } from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"
import { useParams } from "react-router"
import { Project } from "../models/Project"
import { unwrapProjectId } from "../Utils/common"
import { GetProjectService } from "../Services/ProjectService"
// import { Well } from "../models/Well"
// import { GetWellService } from "../Services/WellService"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

function WellView() {
    const [project, setProject] = useState<Project>()
    // const [wells, setWells] = useState<Well[] | undefined>()
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const projectResult: Project = await GetProjectService().getProjectByID(projectId)
                setProject(projectResult)
                // const allWells: well[] | undefined = GetWellService().getWells
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    return (
        <Wrapper>
            <Typography variant="h3">Implementation of Wellview in progress</Typography>
            <Typography variant="h3">{project?.id}</Typography>
        </Wrapper>
    )
}

export default WellView
