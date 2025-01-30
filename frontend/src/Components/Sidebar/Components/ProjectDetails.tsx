import React, { useState } from "react"
import { Stack } from "@mui/material"
import {
    Tooltip,
    Typography,
    Icon,
} from "@equinor/eds-core-react"
import { useParams, useLocation } from "react-router-dom"
import styled from "styled-components"
import {
    dashboard,
    settings,
    compare,
    tune,
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

export const ProjectTimeline = styled(Stack)`
    overflow: auto;
    &[data-timeline="true"] {
        overflow-y: auto;
        display: flex;
        flex-wrap: nowrap;
        flex-direction: column;
        scrollbar-width: none;
        & > * {
            border-left: 2px solid #DCDCDC;

            &[data-timeline-active="true"]{
                border-left: 2px solid #007079;
            }
        }
    }
`

const CenterIcon = styled.div`
    padding-right: 20px;
`

const ProjectDetails: React.FC = () => {
    const { currentContext } = useModuleCurrentContext()
    const { sidebarOpen, setShowEditHistory, showEditHistory } = useAppContext()
    const { setActiveTabProject } = useProjectContext()
    const { revisionId } = useParams()
    const { navigateToProjectTab } = useAppNavigation()
    const location = useLocation()
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
            <Stack
                direction="row"
                justifyContent={sidebarOpen ? "flex-start" : "center"}
            >
                <Stack>
                    <Header>
                        <ProjectTitle
                            variant="overline"
                            onClick={handleTitleClick}
                        >
                            {sidebarOpen ? currentContext?.title : "Project"}
                        </ProjectTitle>
                    </Header>
                    <Timeline data-timeline={sidebarOpen}>
                        <ProjectTimeline data-timeline-active={location.pathname.includes("Overview")}>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => handleNavigateToProjectTab(0)}
                            >
                                {sidebarOpen
                                    ? "Overview"
                                    : <CenterIcon><Tooltip title="Overview" placement="right"><Icon data={dashboard} /></Tooltip></CenterIcon>}
                            </TimelineElement>
                        </ProjectTimeline>
                        <ProjectTimeline data-timeline-active={location.pathname.includes("Compare%20cases")}>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => handleNavigateToProjectTab(1)}
                            >
                                {sidebarOpen
                                    ? "Compare Cases"
                                    : <Tooltip title="Compare Cases" placement="right"><Icon data={compare} /></Tooltip>}
                            </TimelineElement>
                        </ProjectTimeline>
                        <ProjectTimeline data-timeline-active={location.pathname.includes("Technical%20Input")}>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => handleNavigateToProjectTab(2)}
                            >
                                {sidebarOpen
                                    ? "Technical input"
                                    : <Tooltip title="Technical input" placement="right"><Icon data={tune} /></Tooltip>}
                            </TimelineElement>
                        </ProjectTimeline>
                        <ProjectTimeline data-timeline-active={location.pathname.includes("Settings")}>
                            <TimelineElement
                                variant="ghost"
                                className="GhostButton"
                                onClick={() => handleNavigateToProjectTab(5)}
                            >
                                {sidebarOpen
                                    ? "Settings"
                                    : <Tooltip title="Settings" placement="right"><Icon data={settings} /></Tooltip>}
                            </TimelineElement>
                        </ProjectTimeline>
                    </Timeline>
                </Stack>
            </Stack>
            <StyledDivider />
        </>
    )
}

export default ProjectDetails
