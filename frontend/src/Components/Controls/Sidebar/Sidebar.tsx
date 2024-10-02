import { useEffect, useState } from "react"
import styled from "styled-components"
import { SideBar, Button, Divider } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useQuery } from "@tanstack/react-query"

import { useAppContext } from "@/Context/AppContext"
import { projectQueryFn } from "@/Services/QueryFunctions"
import ProjectDetails from "./Components/ProjectDetails"
import CasesDetails from "./Components/CasesDetails"
import CurrentCaseEditHistory from "./Components/CurrentCaseEditHistory"
import ArchivedCasesDetails from "./Components/ArchivedCasesDetails"
import { sharedTimelineStyles } from "./sharedStyles"

const { Toggle, Footer } = SideBar

const Wrapper = styled.div`
    position: relative;
`

const Sticky = styled.div`
    position: sticky;
    top: 0;
    background-color: white;
    height: calc(100vh - 68px);
    overflow: auto;
    border-right: 1px solid #DCDCDC;
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
    margin-left: 1px;
    margin-right: 1px;
    height: 1px;
`

export const TimelineElement = styled(Button)`
    text-align: left;
    height: 28px;
    padding: 10px 0px 10px 5px;
`

const Sidebar = () => {
    const { sidebarOpen, setSidebarOpen } = useAppContext()
    const { currentContext } = useModuleCurrentContext()
    const externalId = currentContext?.externalId

    const { data: projectData } = useQuery({
        queryKey: ["projectApiData", externalId],
        queryFn: () => projectQueryFn(externalId),
        enabled: !!externalId,
    })

    const [archivedCases, setArchivedCases] = useState<Components.Schemas.CaseWithProfilesDto[]>([])

    useEffect(() => {
        if (!projectData) { return }
        setArchivedCases(projectData.cases.filter((c) => c.archived))
    }, [projectData])

    if (!projectData) { return null }

    return (
        <Wrapper>
            <Sticky>
                <StyledSideBar open={sidebarOpen} onToggle={(toggle) => setSidebarOpen(toggle)}>
                    <ProjectDetails />
                    <StyledDivider />
                    <CasesDetails />
                    <StyledDivider />
                    <>
                        {archivedCases.length > 0 && (
                            <>
                                <ArchivedCasesDetails />
                                <StyledDivider />
                            </>
                        )}
                    </>
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
