import React from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'
import { Typography } from '@equinor/eds-core-react'

import { projects } from '../Components/SideMenu/SideMenu'
import CasesTable from '../Components/CasesTable/CasesTable'
import BarChart from '../Components/BarChart'

const ProjectViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const ProjectView = () => {
    let params = useParams()
    const project = projects.find(menuItem => menuItem.id === params.projectId)

    if (project) {
        const dataX: string[] = []
        const dataYCapex: number[] = []
        const dataYDrillex: number[] = []

        project.cases.forEach((caseItem, index) => {
            dataYCapex.push(caseItem.capex)
            dataYDrillex.push(caseItem.drillex)
            dataX.push(caseItem.title)
        })

        return (
            <ProjectViewDiv>
                <Typography variant="h2" style={{ marginBottom: '2rem' }}>
                    {project.name} - Overview
                </Typography>
                <div style={{ display: 'flex' }}>
                    <BarChart data={{ x: dataX, y: dataYCapex }} title="Total Capex per case" />
                    <BarChart data={{ x: dataX, y: dataYDrillex }} title="Total Drillex per case" />
                </div>
                <Typography variant="h3" style={{ marginBottom: '1rem' }}>
                    Cases
                </Typography>
                <CasesTable key={project.id} projectId={project.id} cases={project.cases} />
            </ProjectViewDiv>
        )
    } else {
        return <></>
    }
}

export default ProjectView
