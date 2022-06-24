import React from "react"
import { RouteProps } from "react-router-dom"
import { Tabs } from "@equinor/eds-core-react"
import styled from "styled-components"
import { Project } from "../models/Project"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const OverviewTab = () => (
    <p> Overview Tab</p>
)

const CompareCasesTab = () => (
    <p> Compare Cases Tab</p>
)

const WorkflowTab = () => (
    <p> Workflow Tab</p>
)

interface SettingsProps {
    project: Project
}

const SettingsTab = ({
    project,
}: SettingsProps) => (
    <p>

        {project.name}
    </p>

)

interface Props {
    project: Project
}

const CaseTabs = ({
    project,
}: Props) => {
    const [activeTab, setActiveTab] = React.useState(0)

    return (
        <Tabs activeTab={activeTab} onChange={setActiveTab}>
            <List>
                <Tab>Overview </Tab>
                <Tab>Compare cases</Tab>
                <Tab>Workflow</Tab>
                <Tab>Setings</Tab>
            </List>
            <Panels>
                <StyledTabPanel>
                    <OverviewTab />
                </StyledTabPanel>
                <StyledTabPanel>
                    <CompareCasesTab />
                </StyledTabPanel>
                <StyledTabPanel>
                    <WorkflowTab />
                </StyledTabPanel>
                <StyledTabPanel>
                    <SettingsTab project={project} />
                </StyledTabPanel>
            </Panels>
        </Tabs>
    )
}

export default CaseTabs
