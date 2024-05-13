import styled from "styled-components"
import { useLocation, useNavigate } from "react-router-dom"
import {
    Icon,
    SideBar,
    Button,
    Typography,
    Tooltip,
    Divider,
} from "@equinor/eds-core-react"
import {
    add,
    info_circle,
    dashboard,
    settings,
    compare,
} from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../Context/ProjectContext"
import {
    projectPath,
    casePath,
    productionStrategyOverviewToString,
} from "../Utils/common"
import { useModalContext } from "../Context/ModalContext"
import { useCaseContext } from "../Context/CaseContext"
import { useAppContext } from "../Context/AppContext"
import { EnvironmentVariables } from "../environmentVariables"

const env = EnvironmentVariables.ENVIRONMENT

const { Toggle, Content, Footer } = SideBar

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
`

const StyledSidebar = styled(SideBar)`
    display: flex;
    flex-flow: column;
    flex-grow: 1;
    height: 100%;
    width: 72px;
    min-width: 72px;
    overflow: hidden;
    z-index: 5;
    &[open] {
        width: 200px;
        min-width: 200px;
    }
`

const StyledSidebarContent = styled(Content)`
    display: flex;
    flex-flow: column;
    flex-grow: 1;
    width: 100%;
    overflow: hidden;
`
const Timeline = styled(Grid)`
    position: relative;
    max-height: 100%;
    overflow: auto;
    &[data-timeline="true"] {
        margin: 0 8px;
        width: calc(100% - 16px);
        overflow-x: hidden;
        overflow-y: auto;
        display: flex;
        flex-wrap: nowrap;
        align-items: flex-start;
        flex-direction: column;
        scrollbar-width: none;
        & > * {
            position: relative;
            border-left: 2px solid #dcdcdc;
            width: 100%;
            &.GhostItem {
                min-height: 10px;
            }
            &[data-timeline-active="true"],
            &:not(.GhostItem):hover {
                z-index: 100;
                border-left: 2px solid #007079;
            }
        }
    }
    &[data-timeline="false"] .GhostItem {
        display: none;
    }
`

const SidebarFooter = styled(Footer)`
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: wrap;
`

const TimelineElement = styled(Button)`
    width: 100%;
    text-align: left;
    height: 22px;
    &:before {
        display: none;
    }
    & > span {
        display: block;
        line-height: 22px;
        text-overflow: ellipsis;
        text-wrap: nowrap;
        overflow: hidden;
    }
`

const Sidebar = () => {
    const { project } = useProjectContext()
    const { projectCase, setProjectCase } = useCaseContext()
    const { addNewCase } = useModalContext()
    const { currentContext } = useModuleCurrentContext()
    const { sidebarOpen, setSidebarOpen } = useAppContext()
    const navigate = useNavigate()
    const location = useLocation()
    if (!project) { return null }

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) {
            return null
        }
        const caseResult = project.cases.find((o) => o.id === caseId)
        setProjectCase(caseResult)
        navigate(casePath(currentContext.id, caseId))
        return null
    }

    return (
        <StyledSidebar
            open={sidebarOpen}
            onToggle={(toggle) => setSidebarOpen(toggle)}
        >
            <StyledSidebarContent>
                <Grid container justifyContent="center">
                    {projectCase && (
                        <Grid item container justifyContent="center">
                            <Grid
                                item
                                xs={12}
                                container
                                alignItems="center"
                                justifyContent={
                                    sidebarOpen ? "space-between" : "center"
                                }
                            >
                                <Grid item sx={{ padding: "8px" }}>
                                    <ProjectTitle variant="overline">
                                        {sidebarOpen
                                            ? currentContext?.title
                                            : "Project"}
                                    </ProjectTitle>
                                </Grid>
                            </Grid>

                            <Timeline data-timeline={sidebarOpen}>
                                <Grid item className="GhostItem" />
                                <Grid item>
                                    <TimelineElement
                                        variant="ghost"
                                        className="GhostButton"
                                        onClick={() => navigate(
                                            projectPath(
                                                    currentContext?.id!,
                                            ),
                                            {
                                                state: {
                                                    activeTabProject: 0,
                                                },
                                            },
                                        )}
                                    >
                                        {sidebarOpen ? (
                                            "Overview"
                                        ) : (
                                            <Tooltip
                                                title="Overview"
                                                placement="right"
                                            >
                                                <Icon data={dashboard} />
                                            </Tooltip>
                                        )}
                                    </TimelineElement>
                                </Grid>
                                <Grid item>
                                    <TimelineElement
                                        variant="ghost"
                                        className="GhostButton"
                                        onClick={() => navigate(
                                            projectPath(
                                                    currentContext?.id!,
                                            ),
                                            {
                                                state: {
                                                    activeTabProject: 1,
                                                },
                                            },
                                        )}
                                    >
                                        {sidebarOpen ? (
                                            "Compare Cases"
                                        ) : (
                                            <Tooltip
                                                title="Compare Cases"
                                                placement="right"
                                            >
                                                <Icon data={compare} />
                                            </Tooltip>
                                        )}
                                    </TimelineElement>
                                </Grid>
                                <Grid item>
                                    <TimelineElement
                                        variant="ghost"
                                        className="GhostButton"
                                        onClick={() => navigate(
                                            projectPath(
                                                    currentContext?.id!,
                                            ),
                                            {
                                                state: {
                                                    activeTabProject: 2,
                                                },
                                            },
                                        )}
                                    >
                                        {sidebarOpen ? (
                                            "Settings"
                                        ) : (
                                            <Tooltip
                                                title="Settings"
                                                placement="right"
                                            >
                                                <Icon data={settings} />
                                            </Tooltip>
                                        )}
                                    </TimelineElement>
                                </Grid>
                                <Grid item className="GhostItem" />
                            </Timeline>
                            <Grid item xs={12}>
                                <Divider />
                            </Grid>
                        </Grid>
                    )}
                    <Grid
                        item
                        xs={12}
                        container
                        alignItems="center"
                        justifyContent={
                            sidebarOpen ? "space-between" : "center"
                        }
                    >
                        <Grid
                            item
                            flex={sidebarOpen ? 1 : undefined}
                            sx={{ padding: "8px" }}
                        >
                            <Typography variant="overline">Cases</Typography>
                        </Grid>
                        {(sidebarOpen && project?.cases.length > 0) && (
                            <Grid item>
                                <Tooltip title="Add new case">
                                    <Button
                                        variant="ghost_icon"
                                        className="GhostButton"
                                        onClick={() => addNewCase()}
                                    >
                                        <Icon data={add} />
                                    </Button>
                                </Tooltip>
                            </Grid>
                        )}
                    </Grid>
                </Grid>
                {project?.cases.length > 0
                    ? (
                        <Timeline
                            data-timeline
                            container
                            justifyContent="flex-start"
                            alignItems="flex-start"
                            direction="column"
                        >
                            <Grid item className="GhostItem" />
                            {project?.cases
                                .sort(
                                    (a, b) => new Date(a.createTime).getDate()
                                - new Date(b.createTime).getDate(),
                                )
                                .map((subItem, index) => (
                                    <Grid
                                        item
                                        container
                                        key={`menu - item - ${index + 1} `}
                                        justifyContent="center"
                                        data-timeline-active={location.pathname.includes(
                                            subItem.id,
                                        )}
                                    >
                                        <Tooltip
                                            title={`${
                                                subItem.name ? subItem.name : "Untitled"
                                            } - Strategy: ${productionStrategyOverviewToString(
                                                subItem.productionStrategyOverview,
                                            )}`}
                                            placement="right"
                                        >
                                            <TimelineElement
                                                variant="ghost"
                                                className="GhostButton"
                                                onClick={() => selectCase(subItem.id)}
                                            >
                                                {!sidebarOpen && `#${index + 1}`}
                                                {sidebarOpen
                                            && subItem.name
                                            && subItem.name}
                                                {sidebarOpen
                                            && (subItem.name === ""
                                                || subItem.name === undefined)
                                            && "Untitled"}
                                            </TimelineElement>
                                        </Tooltip>
                                    </Grid>
                                ))}
                            <Grid item className="GhostItem" />
                        </Timeline>
                    )
                    : (
                        <Button
                            variant="ghost"
                            onClick={() => addNewCase()}
                        >
                            <Icon data={add} />
                            <Typography>Add new case</Typography>
                        </Button>
                    )}
            </StyledSidebarContent>
            <SidebarFooter>
                <Grid
                    container
                    justifyContent={sidebarOpen ? "space-evenly" : "center"}
                    alignItems="center"
                >
                    <Tooltip
                        title={!sidebarOpen ? "Send feedback" : ""}
                        placement="right"
                    >
                        <Typography
                            as="a"
                            href="https://forms.office.com/Pages/ResponsePage.aspx?id=NaKkOuK21UiRlX_PBbRZsCjGTHQnxJxIkcdHZ_YqW4BUMTQyTVNLOEY0VUtSUjIwN1QxUVJIRjBaNC4u"
                            target="_blank"
                            rel="noopener noreferrer"
                        >
                            {sidebarOpen ? (
                                "Send feedback"
                            ) : (
                                <Icon data={info_circle} />
                            )}
                        </Typography>
                    </Tooltip>
                    <Toggle />
                </Grid>

            </SidebarFooter>
        </StyledSidebar>
    )
}

export default Sidebar
