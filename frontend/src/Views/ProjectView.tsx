import React from 'react'
import styled from 'styled-components'
import { useParams } from 'react-router-dom'
import { Typography } from '@equinor/eds-core-react'

import { projects } from '../Components/SideMenu/SideMenu'
import CasesTable from '../Components/CasesTable/CasesTable'
import BarChart from '../Components/BarChart'

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const OverviewHeader = styled(Typography)`
    margin-bottom: 2rem;
`

const CasesHeader = styled(Typography)`
    margin-bottom: 1rem;
`

const Charts = styled.div`
    display: flex;
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
            <Wrapper>
                <OverviewHeader variant="h2">{project.name} - Overview</OverviewHeader>
                <Charts>
                    <BarChart data={{ x: dataX, y: dataYCapex }} title="Total Capex per case" />
                    <BarChart data={{ x: dataX, y: dataYDrillex }} title="Total Drillex per case" />
                </Charts>
                <CasesHeader variant="h3">Cases</CasesHeader>
                <CasesTable key={project.id} projectId={project.id} cases={project.cases} />
            </Wrapper>
        )
    } else {
        return <></>
    }
}

export default ProjectView
