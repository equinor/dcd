import { search } from '@equinor/eds-icons'
import { Icon, SingleSelect, Typography } from '@equinor/eds-core-react'
import { tokens } from '@equinor/eds-tokens'
import { UseComboboxStateChange } from 'downshift'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import styled from 'styled-components'

import RecentProjects from '../Components/RecentProjects'
import { ProjectPath, RetrieveLastVisitForProject } from '../Utils/common'
import { ProjectService } from '../Services/ProjectService'

const Wrapper = styled.div`
    margin: 2rem;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    margin-top: 4rem;
`

const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(SingleSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const FindProjectText = styled(Typography)`
    width: 25rem;
    margin-left: 2rem;
    margin-bottom: 1rem;
`


const DashboardView = () => {
    const navigate = useNavigate()

    const [projects, setProjects] = useState<any[]>()
    const [recentProjects, setRecentProjects] = useState<Components.Schemas.ProjectDto[] | any>()

    const getRecentProjects = (projects: Components.Schemas.ProjectDto[]) => {
        const recentProjectsWithTimeStamp = projects
            .map((project) => [project, RetrieveLastVisitForProject(project.projectId!)])
            .filter(([_, timeStamp]) => timeStamp !== null )
            .map(([project, timeStamp]) => [project, parseInt(timeStamp! as string)])
            .sort((oneTimeStampedProject, otherTimeStampedProject) => {
                const oneTimeStamp = oneTimeStampedProject[1] as number
                const otherTimeStamp = otherTimeStampedProject[1] as number
                return otherTimeStamp - oneTimeStamp
            })

        return recentProjectsWithTimeStamp.map(timeStampedProject => timeStampedProject[0])
    }


    useEffect(() => {
        (async () => {
            try {
                const res = await ProjectService.getProjects()
                console.log('[DashboardView]', res)
                setProjects(res)
                const recPro = getRecentProjects(res)
                setRecentProjects(recPro)
            } catch (error) {
                console.error(error)
            }
        })()
    }, [])

    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects?.find(p => p.name === selectedValue)
        if (project) {
            navigate(ProjectPath(project.projectId))
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    if (!projects) return null

    return (
        <Wrapper>
            <FindProjectText variant="h2">Find a project</FindProjectText>
            <ProjectSelect>
                <Icon data={search} color={grey}></Icon>
                <ProjectDropdown
                    label={''}
                    placeholder={'Search projects'}
                    items={projects.map(p => p.name)}
                    handleSelectedItemChange={(changes: UseComboboxStateChange<string>) => onSelected(changes.selectedItem)}
                />
            </ProjectSelect>
            <RecentProjects projects={recentProjects} />
        </Wrapper>
    )
}

export default DashboardView
