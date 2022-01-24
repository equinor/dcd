import React from 'react'
import styled from 'styled-components'
import { useParams } from 'react-router-dom'
import { Tabs, Typography } from '@equinor/eds-core-react'

import { projects } from '../Components/SideMenu/SideMenu'
import OverviewView from './OverviewView'
import DrainageStrategyView from './DrainageStrategyView'
import ExplorationView from './ExplorationView'

const { List, Tab, Panels, Panel } = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const CaseHeader = styled(Typography)`
    margin-bottom: 2rem;
`

const CaseView = () => {
    const [activeTab, setActiveTab] = React.useState<number>(0)
    let params = useParams()
    const project = projects.find(project => project.id === params.projectId)
    const caseItem = project && project.cases.find(c => c.id === params.caseId)

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    if (project && caseItem) {
        return (
            <CaseViewDiv>
                <CaseHeader variant="h2">
                    {project.name} - {caseItem.title}
                </CaseHeader>

                <Tabs activeTab={activeTab} onChange={handleTabChange}>
                    <List>
                        <Tab>Overview</Tab>
                        <Tab>Drainage Strategy</Tab>
                        <Tab>Exploration</Tab>
                        <Tab>Well Project</Tab>
                        <Tab>Offshore facilities</Tab>
                    </List>
                    <Panels>
                        <Panel>
                            <OverviewView />
                        </Panel>
                        <Panel>
                            <DrainageStrategyView />
                        </Panel>
                        <Panel>
                            <ExplorationView />
                        </Panel>
                        <Panel>Satelite production</Panel>
                        <Panel>Pipelines umbilicals</Panel>
                    </Panels>
                </Tabs>
            </CaseViewDiv>
        )
    } else {
        return <></>
    }
}

export default CaseView
