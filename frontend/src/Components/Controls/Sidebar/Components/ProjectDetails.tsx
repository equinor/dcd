import React from "react"
import { Grid } from "@mui/material"
import {
    Tooltip,
    Typography,
    Icon,
    Divider,
} from "@equinor/eds-core-react"
import { useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import {
    dashboard, settings, compare,
} from "@equinor/eds-icons"
import { projectPath } from "../../../../Utils/common"
import { TimelineElement, Timeline } from "../Sidebar"
import { useAppContext } from "../../../../Context/AppContext"

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
`

const ProjectDetails: React.FC = () => {
    const { sidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const { caseId } = useParams()

    const navigate = useNavigate()

    return (
        <div>
            {caseId
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
                                        ? "Case edit history"
                                        : <Tooltip title="Case edit history" placement="right"><Icon data={settings} /></Tooltip>}
                                </TimelineElement>
                            </Grid>
                            <Grid item>
                                <TimelineElement
                                    variant="ghost"
                                    className="GhostButton"
                                    onClick={() => navigate(projectPath(currentContext?.id!), { state: { activeTabProject: 3 } })}
                                >
                                    {sidebarOpen
                                        ? "Settings"
                                        : <Tooltip title="Settings" placement="right"><Icon data={settings} /></Tooltip>}
                                </TimelineElement>
                            </Grid>
                        </Timeline>
                        <Grid item xs={12}>
                            <Divider />
                        </Grid>
                    </Grid>
                )}
        </div>
    )
}

export default ProjectDetails
