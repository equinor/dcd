import React, { useState } from "react"
import { Grid } from "@mui/material"
import {
    Tooltip,
    Typography,
    Icon,
} from "@equinor/eds-core-react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import {
    dashboard,
    settings,
    compare,
} from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { useAppContext } from "@/Context/AppContext"
import {
    TimelineElement,
    Timeline,
    Header,
    StyledDivider,
} from "../Sidebar"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppNavigation } from "@/Hooks/useNavigate"

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
`

const CenterIcon = styled.div`
    padding-right: 20px;
`

const ProjectDetails: React.FC = () => {
    const { currentContext } = useModuleCurrentContext()
    const { sidebarOpen } = useAppContext()
    const { setActiveTabProject } = useProjectContext()
    const { revisionId } = useParams()
    const { navigateToProjectTab } = useAppNavigation()
    const [titleClicks, setTitleClicks] = useState(0)

    const handleTitleClick = () => {
        const newClickCount = titleClicks + 1
        setTitleClicks(newClickCount)

        if (newClickCount === 5) {
            setShowEditHistory(!showEditHistory)
            setTitleClicks(0)
        }
    }

    const handleNavigateToProjectTab = (index: number) => {
        setActiveTabProject(index)
        navigateToProjectTab(index, revisionId)
    }

    return (
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
                            onClick={() => handleNavigateToProjectTab(0)}
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
                            onClick={() => handleNavigateToProjectTab(1)}
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
                            onClick={() => handleNavigateToProjectTab(2)}
                        >
                            {sidebarOpen
                                ? "Technical input"
                                : <Tooltip title="Technical input" placement="right"><Icon data={settings} /></Tooltip>}
                        </TimelineElement>
                    </Grid>
                    <Grid item>
                        <TimelineElement
                            variant="ghost"
                            className="GhostButton"
                            onClick={() => handleNavigateToProjectTab(5)}
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
    )
}

export default ProjectDetails
