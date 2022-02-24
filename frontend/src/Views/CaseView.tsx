import { Tabs, Typography } from '@equinor/eds-core-react'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'

import { Project } from '../models/Project'
import { Case } from '../models/Case'
import DrainageStrategyView from './DrainageStrategyView'
import ExplorationView from './ExplorationView'
import OverviewView from './OverviewView'
import { ProjectService } from '../Services/ProjectService'

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
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await ProjectService.getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find(o => o.id === params.caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader variant="h2">
                {project.name} - {caseItem?.name}
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
