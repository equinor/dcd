import React from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'
import { Typography } from '@equinor/eds-core-react'

import { projects } from '../Components/SideMenu/SideMenu'
import CasesTable from '../Components/CasesTable/CasesTable'

const ProjectViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const ProjectView = () => {
    let params = useParams()
    const project = projects.find(menuItem => menuItem.id === params.projectId)

    if (project) {
        return (
            <ProjectViewDiv>
                <Typography variant="h2" style={{ marginBottom: '2rem' }}>
                    {project.name} - Overview
                </Typography>
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
