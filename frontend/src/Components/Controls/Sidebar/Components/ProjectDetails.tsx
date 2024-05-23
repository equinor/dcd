import React from "react"
import { Grid } from "@mui/material"
import {
    Tooltip,
    Typography,
    Icon,
} from "@equinor/eds-core-react"
import { useNavigate } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import {
    dashboard, settings, compare,
} from "@equinor/eds-icons"
import { projectPath } from "../../../../Utils/common"
import { TimelineElement, Timeline } from "../Sidebar"
import { useAppContext } from "../../../../Context/AppContext"
import { useCaseContext } from "../../../../Context/CaseContext"

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
`

const ProjectDetails: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { projectCase } = useCaseContext()
    const { currentContext } = useModuleCurrentContext()
    const navigate = useNavigate()

    return (
        <div>
            {projectCase
                && (
                    <Grid item container justifyContent="center">
                        <Grid item xs={12} container alignItems="center" justifyContent={sidebarOpen ? "space-between" : "center"}>
                            <Grid item sx={{ padding: "8px" }}>
                                <ProjectTitle variant="overline">{sidebarOpen ? currentContext?.title : "Project"}</ProjectTitle>
                            </Grid>
                        </Grid>

                        <Timeline data-timeline={sidebarOpen}>
                            <Grid item>
                                <TimelineElement
                                    variant="ghost"
                                    className="GhostButton"
                                    onClick={() => navigate(projectPath(currentContext?.id!), { state: { activeTabProject: 0 } })}
                                >
                                    {sidebarOpen
                                        ? "Overview"
                                        : <Tooltip title="Overview" placement="right"><Icon data={dashboard} /></Tooltip>}
                                </TimelineElement>
                            </Grid>
                            <Grid item>
                                <TimelineElement
                                    variant="ghost"
                                    className="GhostButton"
                                    onClick={() => navigate(projectPath(currentContext?.id!), { state: { activeTabProject: 1 } })}
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
                                    onClick={() => navigate(projectPath(currentContext?.id!), { state: { activeTabProject: 2 } })}
                                >
                                    {sidebarOpen
                                        ? "Settings"
                                        : <Tooltip title="Settings" placement="right"><Icon data={settings} /></Tooltip>}
                                </TimelineElement>
                            </Grid>
                        </Timeline>

                    </Grid>
                )}
        </div>
    )
}

export default ProjectDetails
