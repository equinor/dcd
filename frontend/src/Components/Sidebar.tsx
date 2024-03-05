import styled from "styled-components"
import { NavLink, useNavigate, useParams } from "react-router-dom"
import { Icon, SideBar, Button, Typography, Tooltip, Divider } from "@equinor/eds-core-react"
import { file, add, go_to } from "@equinor/eds-icons"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useProjectContext } from "../Context/ProjectContext"
import { projectPath } from "../Utils/common"
import { useModalContext } from "../Context/ModalContext"
import Grid from "@mui/material/Grid"
import { useCaseContext } from "../Context/CaseContext"
import { casePath } from "../Utils/common"

const { Content, Footer } = SideBar

const ProjectTitle = styled(Typography)`
    line-break: anywhere;
    padding-top: 1rem;
`

const StyledSidebar = styled(SideBar)`
    position: fixed;
    height: calc(100vh - 60px);
    overflow: hidden;
    display: grid;
    grid-template-rows: auto 2rem;
`

const StyledSidebarContent = styled(Content)`
    display: grid;
    grid-template-rows: auto 1fr;
    width: 100%;
    overflow: hidden;
`
const SidebarCases = styled.div`
    width: 100%;
    max-height: 100%;
    overflow: auto;
`

const Sidebar = () => {
    const { project } = useProjectContext()
    const { projectCase, setProjectCase } = useCaseContext()
    const { addNewCase } = useModalContext()
    const { currentContext } = useModuleCurrentContext()
    const navigate = useNavigate()

    if (!project) return (<></>)

    const selectCase = (caseId: string) => {
        if (!currentContext || !caseId) { return null }
        const caseResult = project.cases.find((o) => o.id === caseId)
        setProjectCase(caseResult)
        navigate(casePath(currentContext.id, caseId))
    }

    return (
        <StyledSidebar open>
            <StyledSidebarContent>
                <Grid container>
                    {projectCase 
                    &&    <>
                            <Grid item container alignItems="center" justifyContent="space-between" display="grid" gridTemplateColumns="0.5rem 1fr auto">
                                <Grid item></Grid>
                                <Grid item>
                                    <ProjectTitle variant="overline">{project?.name}</ProjectTitle>
                                </Grid>
                            </Grid>
                            
                            <Grid item xs={12}>
                                <Button variant="ghost" className="GhostButton" onClick={() => navigate(projectPath(currentContext?.id!), {state: {activeTabProject: 0}})}>
                                    Overview
                                </Button>
                            </Grid>
                            <Grid item xs={12}>
                                <Button variant="ghost" className="GhostButton" onClick={() => navigate(projectPath(currentContext?.id!), {state: {activeTabProject: 1}})}>
                                    Compare Cases
                                </Button>
                            </Grid>
                            <Grid item xs={12}>
                                <Button variant="ghost" className="GhostButton" onClick={() => navigate(projectPath(currentContext?.id!), {state: {activeTabProject: 2}})}>
                                    Settings
                                </Button>
                            </Grid>
                            <Grid item xs={12}>
                                <Divider />
                            </Grid>
                        </>
                    }
                    <Grid item container alignItems="center" justifyContent="space-between" display="grid" gridTemplateColumns="0.5rem 1fr auto">
                        <Grid item></Grid>
                        <Grid item>
                            <Typography variant="overline">Cases</Typography>
                        </Grid>
                        <Grid item>
                            <Tooltip title="Add new case">
                                <Button variant="ghost_icon" className="GhostButton" onClick={() => addNewCase()}><Icon data={add} /></Button>
                            </Tooltip>
                        </Grid>
                    </Grid>
                </Grid>
                <SidebarCases>
                    {
                        project?.cases.sort((a,b) => {
                            return new Date(a.createTime).getDate() - 
                                new Date(b.createTime).getDate()
                        }).map((subItem, index) => (
                            <Grid item xs={12} key={`menu - item - ${index + 1} `}>
                                <Button variant="ghost" className="GhostButton" onClick={() => selectCase(subItem.id)}>
                                    <Icon data={file} />
                                    {subItem.name ? subItem.name : "Untitled"}
                                </Button>
                            </Grid>
                        ))
                    }
                </SidebarCases>
            </StyledSidebarContent>
            <Footer>
                <Grid container justifyContent="center">
                    <Typography
                        as="a"
                        href="https://forms.office.com/Pages/ResponsePage.aspx?id=NaKkOuK21UiRlX_PBbRZsCjGTHQnxJxIkcdHZ_YqW4BUMTQyTVNLOEY0VUtSUjIwN1QxUVJIRjBaNC4u"
                        target="_blank"
                        rel="noopener noreferrer"
                    >
                        Send feedback
                    </Typography>
                </Grid>
            </Footer>
        </StyledSidebar>
    )
}

export default Sidebar
