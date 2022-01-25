import { Tabs, Typography } from '@equinor/eds-core-react'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'

import { Project } from '../types'
import { useService } from '../Services'
import DrainageStrategyView from './DrainageStrategyView'
import ExplorationView from './ExplorationView'
import OverviewView from './OverviewView'

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
    const [project, setProject] = useState<Project>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()

    const ProjectService = useService('ProjectService')

    useEffect(() => {
        if (ProjectService) {
            (async () => {
                try {
                    setProject(await ProjectService.getProjectByID(params.projectId!))
                } catch (error) {
                    console.error(`[CaseView] Error while fetching projet ${params.projectId}`, error)
                }
            })()
        }
    })

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    const caseItem: any = {}

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader variant="h2">
                {project.name} - {caseItem.title}
            </CaseHeader>
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <List>
                    <Tab>Overview</Tab>
                    <Tab>Input data</Tab>
                    <Tab>Technical input</Tab>
                    <Tab>Satelite production</Tab>
                    <Tab>Pipelines umbilicals</Tab>
                    <Tab>Flexible riser</Tab>
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
}

export default CaseView
