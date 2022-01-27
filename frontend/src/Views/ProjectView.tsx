import { Typography } from '@equinor/eds-core-react'
import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import styled from 'styled-components'

import CasesTable from '../Components/CasesTable/CasesTable'
import BarChart from '../Components/BarChart'
import { Project } from '../types'
import { useProjectService } from '../Services/ProjectService'

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
    const [project, setProject] = useState<Project>()

    const ProjectService = useProjectService()

    useEffect(() => {
        if (ProjectService) {
            (async () => {
                try {
                    const res = await ProjectService.getProjectByID(params.projectId!)
                    setProject(res)
                } catch (error) {
                    console.error(`[ProjectView] Error while fetching project ${params.projectId}`, error)
                }
            })()
        }
    }, [])

    if (!project) return null

    const dataX: string[] = []
    let dataProdProfileGas: number[] = []
    let dataProdProfileOil: number[] = []

    project.cases.forEach((c) => {
        dataProdProfileGas = c.drainageStrategy?.productionProfileGas?.yearValues?.map((v) => v.value)
        dataProdProfileOil = c.drainageStrategy?.productionProfileOil?.yearValues?.map((v) => v.value)
        dataX.push(c.name)
    })

    return (
        <Wrapper>
            <OverviewHeader variant="h2">
                {project.name} - Overview
            </OverviewHeader>
            <Charts>
                <BarChart data={{ x: dataX, y: dataProdProfileGas }} title="Production profile gas" />
                <BarChart data={{ x: dataX, y: dataProdProfileOil }} title="Production profile oil" />
            </Charts>
            <CasesHeader variant="h3">
                Cases
            </CasesHeader>
            <CasesTable key={project.id} projectId={project.id} cases={project.cases} />
        </Wrapper>
    )
}

export default ProjectView
