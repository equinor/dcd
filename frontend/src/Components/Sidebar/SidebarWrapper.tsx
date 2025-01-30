import { useEffect, useState } from "react"
import styled from "styled-components"
import { SideBar, Button, Divider } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid2"

import { useAppContext } from "@/Context/AppContext"
import { useDataFetch } from "@/Hooks/useDataFetch"
import ProjectDetails from "./Components/Project/ProjectDetails"
import CasesDetails from "./Components/ActiveCases/CasesDetails"
import CurrentCaseEditHistory from "./Components/EditHistory/CurrentCaseEditHistory"
import ArchivedCasesDetails from "./Components/ArchivedCases/ArchivedCasesDetails"
import { sharedTimelineStyles } from "./sharedStyles"

const { Toggle, Footer } = SideBar

const Wrapper = styled.div`
    position: relative;
    border-right: 1px solid #DCDCDC;
`

const Sticky = styled.div`
    position: sticky;
    top: 0;
    background-color: white;
    height: calc(100vh - 102px);
    overflow: auto;
`

const StyledSideBar = styled(SideBar)`
    height: 100%;
    border-right: 0px;
    display: flex;
    flex-direction: column;
`

export const Header = styled.div`
    margin: 15px 10px;
`

export const Timeline = styled(Grid)`
    max-height: 200px;
    ${sharedTimelineStyles}
`

export const StyledDivider = styled(Divider)`
    height: 1px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 10px;
    width: 95%;
`

export const TimelineElement = styled(Button)`
    height: 28px;
    padding: 10px 0px 10px 5px;
`

const Sidebar = () => {
    const { sidebarOpen, setSidebarOpen } = useAppContext()
    const revisionAndProjectData = useDataFetch()

    const [archivedCases, setArchivedCases] = useState<Components.Schemas.CaseOverviewDto[]>([])

    useEffect(() => {
        if (!revisionAndProjectData) { return }
        setArchivedCases(revisionAndProjectData.commonProjectAndRevisionData.cases.filter((c) => c.archived))
    }, [revisionAndProjectData])

    if (!revisionAndProjectData) { return null }

    return (
        <Wrapper>
            <Sticky>
                <StyledSideBar open={sidebarOpen} onToggle={(toggle) => setSidebarOpen(toggle)}>
                    <ProjectDetails />
                    <CasesDetails />
                    <StyledDivider />
                    {archivedCases.length > 0 && (
                        <>
                            <ArchivedCasesDetails />
                            <StyledDivider />
                        </>
                    )}
                    <CurrentCaseEditHistory />
                    <Footer>
                        <Toggle />
                    </Footer>
                </StyledSideBar>
            </Sticky>
        </Wrapper>
    )
}

export default Sidebar
