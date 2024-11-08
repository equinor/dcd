import { useEffect } from "react"
import Grid from "@mui/material/Grid"
import { useLocation } from "react-router-dom"
import styled from "styled-components"
import ProjectOverviewTab from "../Components/Project/ProjectOverviewTab"
import ProjectCompareCasesTab from "../Components/Project/CompareCasesTab/CompareCasesTabOverview"
import ProjectSettingsTab from "../Components/Project/ProjectSettingsTab"
import EditHistoryOverviewTab from "../Components/Project/EditHistoryOverviewTab"
import { useProjectContext } from "../Context/ProjectContext"
import TechnicalInput from "../Components/Project/CompareCasesTab/Tabs/TechnicalInput"

const Wrapper = styled(Grid)`
    padding: 0 16px;
`

const ProjectView = () => {
    const { activeTabProject, setActiveTabProject } = useProjectContext()
    const location = useLocation()
    const activeTabProjectParam = location?.state?.activeTabProject

    useEffect(() => {
        if (activeTabProjectParam) { setActiveTabProject(activeTabProjectParam) }
    }, [activeTabProjectParam])

    return (
        <Wrapper item xs={12}>
            <div role="tabpanel" hidden={activeTabProject !== 0}>
                <ProjectOverviewTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 1}>
                <ProjectCompareCasesTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 2}>
                <TechnicalInput />
            </div>
            {/* <div role="tabpanel" hidden={activeTabProject !== 3}>
                <EditHistoryOverviewTab />
            </div> */}
            <div role="tabpanel" hidden={activeTabProject !== 3}>
                <ProjectSettingsTab />
            </div>
        </Wrapper>
    )
}

export default ProjectView
