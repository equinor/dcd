import styled from "styled-components"
import { SideBar, Button, Divider } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useAppContext } from "../../../Context/AppContext"
import ProjectDetails from "./Components/ProjectDetails"
import CasesDetails from "./Components/CasesDetails"
import CurrentCaseEditHistory from "./Components/CurrentCaseEditHistory"

const { Toggle, Content, Footer } = SideBar

const Wrapper = styled.div`
        position: relative;
`

const Sticky = styled.div`
    position: sticky;
    top: 0;
    background-color: white;
    height: calc(100vh - 64px);
`

export const Header = styled.div`
    margin: 15px 10px;
`

export const Timeline = styled(Grid)`
    padding-left: 10px;
    max-height: 200px;
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

export const TimelineElement = styled(Button)`
    text-align: left;
    height: 28px;
    padding: 10px 5px 10px 10px;  
    
`

const Sidebar = () => {
    const { project } = useProjectContext()
    const { sidebarOpen, setSidebarOpen } = useAppContext()

    if (!project) { return null }

    return (
        <Wrapper>
            <Sticky>
                <SideBar open={sidebarOpen} onToggle={(toggle) => setSidebarOpen(toggle)}>
                    <Content>
                        <ProjectDetails />
                        <Divider />
                        <CasesDetails />
                        <Divider />
                        <CurrentCaseEditHistory />
                        <Divider />
                    </Content>
                    <Footer>
                        <Toggle />
                    </Footer>
                </SideBar>
            </Sticky>
        </Wrapper>
    )
}

export default Sidebar
