import Grid2 from "@mui/material/Grid2"
import { useEffect } from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"

import ProjectChangeLog from "@/Components/ChangeLog/ProjectChangeLog"
import AccessManagementTab from "@/Components/ProjectTabs/AccessManagementTab"
import ProjectCompareCasesTab from "@/Components/ProjectTabs/CompareCasesTab/CompareCasesTabOverview"
import ProjectOverviewTab from "@/Components/ProjectTabs/ProjectOverviewTab"
import ProjectSettingsTab from "@/Components/ProjectTabs/ProjectSettingsTab"
import TechnicalInputTab from "@/Components/ProjectTabs/TechnicalInputTab/TechnicalInputTab"
import { useAppNavigation } from "@/Hooks/useNavigate"
import { useProjectContext } from "@/Store/ProjectContext"
import { projectTabNames } from "@/Utils/Config/constants"

const Wrapper = styled(Grid2)`
    padding: 0 16px 16px;
`

const ProjectView = () => {
    const { activeTabProject, setActiveTabProject } = useProjectContext()
    const { tab, revisionId } = useParams()
    const { navigateToProjectTab } = useAppNavigation()

    // syncs the active tab with the url
    useEffect(() => {
        if (tab) {
            const tabIndex = projectTabNames.indexOf(tab)

            if (activeTabProject !== tabIndex) {
                setActiveTabProject(tabIndex)
            }
        } else {
            // If no tab is specified, navigate to default tab using replace
            navigateToProjectTab(0, revisionId)
        }
    }, [tab, revisionId])

    return (
        <Wrapper size={{ xs: 12 }}>
            <div role="tabpanel" hidden={activeTabProject !== 0}>
                <ProjectOverviewTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 1}>
                <ProjectCompareCasesTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 2}>
                <TechnicalInputTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 3}>
                <AccessManagementTab />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 4}>
                <ProjectChangeLog />
            </div>
            <div role="tabpanel" hidden={activeTabProject !== 5}>
                <ProjectSettingsTab />
            </div>
        </Wrapper>
    )
}

export default ProjectView
