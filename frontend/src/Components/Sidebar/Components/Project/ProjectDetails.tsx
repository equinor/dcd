import React, { useState } from "react"
import { Stack } from "@mui/material"
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
    tune,
    users_circle,
} from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"

import { useAppContext } from "@/Context/AppContext"
import {
    TimelineElement,
    Timeline,
    Header,
    StyledDivider,
} from "@/Components/Sidebar/SidebarWrapper"
import { useProjectContext } from "@/Context/ProjectContext"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { projectTabNames } from "@/Utils/constants"

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

const icons = [
    dashboard,
    compare,
    tune,
    users_circle,
    settings,
]

const ProjectDetails: React.FC = () => {
    const { currentContext } = useModuleCurrentContext()
    const { sidebarOpen, setDeveloperMode, developerMode } = useAppContext()
    const { setActiveTabProject, activeTabProject } = useProjectContext()
    const { revisionId } = useParams()
    const { navigateToProjectTab } = useAppNavigation()
    const [titleClicks, setTitleClicks] = useState(0)

    const handleTitleClick = () => {
        const newClickCount = titleClicks + 1
        setTitleClicks(newClickCount)

        if (newClickCount === 5) {
            setDeveloperMode(!developerMode)
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
                        {projectTabNames.map((tab, index) => (
                            <ProjectTimeline key={tab} data-timeline-active={index === activeTabProject}>
                                <TimelineElement
                                    variant="ghost"
                                    className="GhostButton"
                                    onClick={() => handleNavigateToProjectTab(index)}
                                >
                                    {sidebarOpen
                                        ? tab
                                        : <CenterIcon><Tooltip title={tab} placement="right"><Icon data={icons[index]} /></Tooltip></CenterIcon>}
                                </TimelineElement>
                            </ProjectTimeline>
                        ))}
                    </Timeline>
                </Stack>
            </Stack>
            <StyledDivider />
        </>
    )
}

export default ProjectDetails
