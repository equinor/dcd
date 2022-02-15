import { Typography } from '@equinor/eds-core-react'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'
import Cookies from 'universal-cookie'

import CasesTable from '../Components/CasesTable/CasesTable'
import BarChart from '../Components/BarChart'
import { projectService } from '../Services/ProjectService'
import { GetDrainageStrategy, StoreRecentProject } from '../Utils/common'

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
    const [project, setProject] = useState<Components.Schemas.ProjectDto>()

    useEffect(() => {
        if (projectService) {
            (async () => {
                try {
                    const res = await projectService.getProjectByID(params.projectId!)
                    setProject(res)
                } catch (error) {
                    console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
                }
            })()
        }
    }, [])

    if (!project) return null

    const cookies = new Cookies()
    StoreRecentProject(project.projectId!, cookies)

    const dataX: string[] = []
    let dataProdProfileGas: number[] = []
    let dataProdProfileOil: number[] = []

    project.cases?.forEach(c => {
        const drainageStrategy = GetDrainageStrategy(project, c.drainageStrategyLink);
        dataProdProfileGas = drainageStrategy?.productionProfileGas?.values?.map(v => v)!
        dataProdProfileOil = drainageStrategy?.productionProfileOil?.values?.map(v => v)!
        dataX.push(c.name!)
    })

    return (
        <Wrapper>
            <OverviewHeader variant="h2">{project.name} - Overview</OverviewHeader>
            <Charts>
                <BarChart data={{ x: dataX, y: dataProdProfileGas }} title="Production profile gas" />
                <BarChart data={{ x: dataX, y: dataProdProfileOil }} title="Production profile oil" />
            </Charts>
            <CasesHeader variant="h3">Cases</CasesHeader>
            <CasesTable key={project.projectId} project={project} cases={project.cases!} />
        </Wrapper>
    )
}

export default ProjectView
