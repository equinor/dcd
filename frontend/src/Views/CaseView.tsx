import React from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'
import { Tabs, Typography } from '@equinor/eds-core-react'
import { projects } from '../Components/SideMenu/SideMenu'

const { List, Tab, Panels, Panel } = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
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
                <Typography variant="h2" style={{ marginBottom: '2rem' }}>
                    {project.name} - {caseItem.title}
                </Typography>
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
                        <Panel>Overview</Panel>
                        <Panel>Input data</Panel>
                        <Panel>Technical input</Panel>
                        <Panel>Satelite production</Panel>
                        <Panel>Pipelines umbilicals</Panel>
                        <Panel>Flexible riser</Panel>
                    </Panels>
                </Tabs>
            </CaseViewDiv>
        )
    } else {
        return <></>
    }
}

export default CaseView
