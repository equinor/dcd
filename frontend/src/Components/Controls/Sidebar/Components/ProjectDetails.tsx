import React from "react"
import { Grid } from "@mui/material"
import {
    Tooltip,
    Typography,
    Icon,
} from "@equinor/eds-core-react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import {
    dashboard,
    settings,
    compare,
} from "@equinor/eds-icons"

import { projectPath } from "@/Utils/common"
import { useAppContext } from "@/Context/AppContext"
import {
    TimelineElement,
    Timeline,
    Header,
    StyledDivider,
} from "../Sidebar"
import { useProjectContext } from "@/Context/ProjectContext"

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
`

const CenterIcon = styled.div`
    padding-right: 20px;
`

const ProjectDetails: React.FC = () => {
    const { sidebarOpen, showEditHistory } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { caseId } = useParams()
    const { setActiveTabProject } = useProjectContext()
    const navigate = useNavigate()
    const { isRevision, projectId } = useProjectContext()
    const { revisionId } = useParams()

    const navigateToProjectTab = (index: number) => {
        setActiveTabProject(index)
        if (isRevision && revisionId) {
            navigate(`${projectPath(projectId)}/revision/${revisionId}`)
        } else {
            navigate(projectPath(currentContext?.id!))
        }
    }
    return caseId
        ? (
            <>
                <Grid item container justifyContent={sidebarOpen ? "start" : "center"} alignItems="center">
                    <Grid item xs={12} container>
                        <Header>
                            <ProjectTitle variant="overline">{sidebarOpen ? currentContext?.title : "Project"}</ProjectTitle>
                        </Header>
                    </Grid>

                    <Timeline data-timeline={sidebarOpen}>
                        <Grid item>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => navigateToProjectTab(0)}
                            >
                                {sidebarOpen
                                    ? "Overview"
                                    : <CenterIcon><Tooltip title="Overview" placement="right"><Icon data={dashboard} /></Tooltip></CenterIcon>}
                            </TimelineElement>
                        </Grid>
                        <Grid item>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => navigateToProjectTab(1)}
                            >
                                {sidebarOpen
                                    ? "Compare Cases"
                                    : <Tooltip title="Compare Cases" placement="right"><Icon data={compare} /></Tooltip>}
                            </TimelineElement>
                        </Grid>
                        <Grid item>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => navigateToProjectTab(2)}
                            >
                                {sidebarOpen
                                    ? "Technical input"
                                    : <Tooltip title="Technical input" placement="right"><Icon data={settings} /></Tooltip>}
                            </TimelineElement>
                        </Grid>
                        {/* <Grid item>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => navigate(projectPath(currentContext?.id!), { state: { activeTabProject: 3 } })}
                            >
                                {sidebarOpen
                                    ? "Case edit history"
                                    : <Tooltip title="Case edit history" placement="right"><Icon data={settings} /></Tooltip>}
                            </TimelineElement>
                        </Grid> */}
                        <Grid item>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => navigateToProjectTab(5)}
                            >
                                {sidebarOpen
                                    ? "Settings"
                                    : <Tooltip title="Settings" placement="right"><Icon data={settings} /></Tooltip>}
                            </TimelineElement>
                        </Grid>
                    </Timeline>
                </Grid>
                <StyledDivider />
            </>
        ) : null
}

export default ProjectDetails
