import { add, delete_to_trash, edit } from "@equinor/eds-icons"
import { Button, EdsProvider, Icon, Tooltip, Typography } from '@equinor/eds-core-react'
import { useEffect, useMemo, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'

import BarChart from '../Components/BarChart'

import { Project } from '../models/Project'
import { ProjectService } from '../Services/ProjectService'

import { StoreRecentProject } from '../Utils/common'

const Wrapper = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Header = styled.header`
    display: flex;
    align-items: center;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

const ChartsContainer = styled.div`
    display: flex;
`

const ProjectView = () => {
    let params = useParams()
    const [project, setProject] = useState<Project>()

    useEffect(() => {
        (async () => {
            try {
                const res = await ProjectService.getProjectByID(params.projectId!)
                console.log('[ProjectView]', res)
                setProject(res)
            } catch (error) {
                console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [])

    const chartData = useMemo(() => {
        return project ? {
            x: project?.cases.map((c) => c.name),
            y: project?.cases.map((c) => c.capex),
        } : { x: [], y: [] }
    }, [project])

    if (!project) return null

    StoreRecentProject(project.id!)

    return (
        <Wrapper>
            <Header>
                <Typography variant="h2">{project.name}</Typography>

                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title={`Edit ${project.name}`}>
                            <Button variant="ghost_icon" aria-label={`Edit ${project.name}`}>
                                <Icon data={edit} />
                            </Button>
                        </Tooltip>
                        <Tooltip title="Add a case">
                            <Button variant="ghost_icon" aria-label="Add a case">
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                        <Tooltip title={`Delete ${project.name}`}>
                            <Button variant="ghost_icon" color="danger" aria-label={`Delete ${project.name}`}>
                                <Icon data={delete_to_trash} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </Header>

            <ChartsContainer>
                <BarChart data={chartData!} title="Capex / case" />
            </ChartsContainer>
        </Wrapper>
    )
}

export default ProjectView
