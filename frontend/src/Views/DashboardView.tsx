import { search } from '@equinor/eds-icons'
import { Icon, SingleSelect, Typography, Card } from '@equinor/eds-core-react'
import { tokens } from '@equinor/eds-tokens'
import { UseComboboxStateChange } from 'downshift'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import styled from 'styled-components'
import Cookies from 'universal-cookie'

import { projectService } from '../Services/ProjectService'
import RecentProjects from '../Components/RecentProjects'
import { IsRecentProjectCookieKey, ExtractProjectIdFromCookieKey } from '../Utils/common'

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

    const getRecentProjectsFromCookies =
        (projects: Components.Schemas.ProjectDto[]) => {
        const cookies = new Cookies()
        const fetchedCookies = cookies.getAll()
        console.log(fetchedCookies)
        const recentProjectCookies = Object.entries(fetchedCookies)
            .filter(([key, _]) => IsRecentProjectCookieKey(key))
                .sort(([_, oneTimestamp], [__, otherTimestamp]) => {
                    return parseInt(otherTimestamp as string,10) -
                        parseInt(oneTimestamp as string,10)
                })
        console.log(recentProjectCookies)

        const filterRecentProjects = (
            collectedProjects: Components.Schemas.ProjectDto[],
            [cookieKey, _] : [string, any]) => {
                const cookieProjectId = ExtractProjectIdFromCookieKey(cookieKey)
                console.log(cookieProjectId)
                const cookieProject = projects.find(proj => proj.projectId === cookieProjectId)
                if (cookieProject !== undefined) {
                    collectedProjects.push(cookieProject)
                }
                return collectedProjects
            }
        const recentProjects = recentProjectCookies.reduce(filterRecentProjects, [])
        console.log(recentProjects)
        return recentProjects
    }


    useEffect(() => {
        if (projectService) {
            (async () => {
                try {
                    const res = await projectService.getProjects()
                    console.log(res)
                    setProjects(res)
                    const recPro = getRecentProjectsFromCookies(res)
                    setRecentProjects(recPro)
                } catch (error) {
                    console.error(error)
                }
            })()
        }
    }, [])

    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects?.find(p => p.name === selectedValue)
        if (project) {
            navigate(`/project/${project.projectId}`)
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
