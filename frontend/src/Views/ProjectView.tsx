import { useEffect } from "react"
import styled from "styled-components"
import { Tabs } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import { useLocation } from "react-router-dom"
import ProjectOverviewTab from "../Components/Project/ProjectOverviewTab"
import ProjectCompareCasesTab from "../Components/Project/CompareCasesTab/CompareCasesTabOverview"
import ProjectSettingsTab from "../Components/Project/ProjectSettingsTab"
import { useProjectContext } from "../Context/ProjectContext"
import { EnvironmentVariables } from "../environmentVariables"

const env = EnvironmentVariables.ENVIRONMENT

const {
    List, Tab, Panels, Panel,
} = Tabs

const ScrollablePanel = styled(Panel)`
  height: calc(100vh - ${env === "dev" ? "195px" : "230px"});
  padding: 16px;
  overflow: auto;
`

const ProjectView = () => {
    const { activeTabProject, setActiveTabProject } = useProjectContext()
    const location = useLocation()
    const activeTabProjectParam = location?.state?.activeTabProject

    useEffect(() => {
        if (activeTabProjectParam) { setActiveTabProject(activeTabProjectParam) }
    }, [activeTabProjectParam])

    return (
        <Grid container spacing={1} alignSelf="flex-start">
            <Grid item xs={12}>
                <Tabs activeTab={activeTabProject} onChange={setActiveTabProject}>
                    <List>
                        <Tab>Overview </Tab>
                        <Tab>Compare cases</Tab>
                        <Tab>Settings</Tab>
                    </List>
                    <Panels>
                        <ScrollablePanel>
                            <ProjectOverviewTab />
                        </ScrollablePanel>
                        <ScrollablePanel>
                            <ProjectCompareCasesTab />
                        </ScrollablePanel>
                        <ScrollablePanel>
                            <ProjectSettingsTab />
                        </ScrollablePanel>
                    </Panels>
                </Tabs>
            </Grid>
        </Grid>
    )
}

export default ProjectView
