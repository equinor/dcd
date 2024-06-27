import styled from "styled-components"
import { SideBar, Button, Divider } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useAppContext } from "../../../Context/AppContext"
import ProjectDetails from "./Components/ProjectDetails"
import CasesDetails from "./Components/CasesDetails"
import CurrentCaseEditHistory from "./Components/CurrentCaseEditHistory"

const { Toggle, Content, Footer } = SideBar

const StyledSidebar = styled(SideBar)`
    position: fixed;
    height: calc(100vh - 60px);
    overflow: hidden;
    display: grid;
    grid-template-rows: auto 80px;
    &[open] {
        grid-template-rows: auto 60px;
    }
`

const StyledSidebarContent = styled(Content)`
    display: grid;
    grid-template-rows: auto 1fr;
    width: 100%;
    overflow: auto;
`
export const Timeline = styled(Grid)`
    position: relative;
    max-height: 200px;
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
            border-left: 2px solid #DCDCDC;
            width: 100%;
            &.GhostItem {
                min-height: 14px;
            }
            &[data-timeline-active="true"],
            &:not(.GhostItem):hover {
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

export const TimelineElement = styled(Button)`
    text-align: left;
    height: 28px;
    &:before {
        display: none;
    }
    & > span {
        display: block;
        line-height: 28px;
        text-overflow: ellipsis;
        white-space: nowrap;
        overflow: hidden;
    }
`

const Sidebar = () => {
    const { project } = useProjectContext()
    const { sidebarOpen, setSidebarOpen } = useAppContext()

    if (!project) { return null }

    return (
        <StyledSidebar open={sidebarOpen} onToggle={(toggle) => setSidebarOpen(toggle)}>
            <StyledSidebarContent>
                <Grid container justifyContent="center">
                    <ProjectDetails />
                    <CasesDetails />
                    <Grid item xs={12}>
                        <Divider />
                    </Grid>
                    {/* uncomment for next release <CurrentCaseEditHistory /> */}
                </Grid>
            </StyledSidebarContent>
            <SidebarFooter>
                <Grid container justifyContent="flex-end" alignItems="flex-end">
                    <Toggle />
                </Grid>
            </SidebarFooter>
        </StyledSidebar>
    )
}

export default Sidebar
