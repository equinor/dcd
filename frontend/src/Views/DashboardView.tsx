<<<<<<< HEAD
import { search } from '@equinor/eds-icons'
import { Icon, NativeSelect, Typography } from '@equinor/eds-core-react'
import { tokens } from '@equinor/eds-tokens'
import { ChangeEvent, useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import styled from 'styled-components'
import { useTranslation } from "react-i18next";

import RecentProjects from '../Components/RecentProjects'
import { ProjectPath, RetrieveLastVisitForProject } from '../Utils/common'
import { ProjectService } from '../Services/ProjectService'
import { Project } from '../models/Project'
=======
import { ChangeEvent, useEffect, useState } from "react"
import { Icon, NativeSelect, Typography } from "@equinor/eds-core-react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"

import { Project } from "../models/Project"

import RecentProjects from "../Components/RecentProjects"

import { GetProjectService } from "../Services/ProjectService"

import { ProjectPath, RetrieveLastVisitForProject } from "../Utils/common"
>>>>>>> main

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

const ProjectDropdown = styled(NativeSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const FindProjectText = styled(Typography)`
    width: 25rem;
    margin-left: 2rem;
    margin-bottom: 1rem;
`

<<<<<<< HEAD

const DashboardView = () => {
    const { t } = useTranslation()
=======
function DashboardView() {
>>>>>>> main
    const navigate = useNavigate()

    const ProjectService = GetProjectService()

    const [projects, setProjects] = useState<any[]>()
    const [recentProjects, setRecentProjects] = useState<Components.Schemas.ProjectDto[] | any>()

    const getRecentProjects = (incomingProjects: Project[]) => {
        const recentProjectsWithTimeStamp = incomingProjects
            .map((project) => [project, RetrieveLastVisitForProject(project.id!)])
            .filter(([_, timeStamp]) => timeStamp !== null)
            .map(([project, timeStamp]) => [project, parseInt(timeStamp! as string, 10)])
            .sort((oneTimeStampedProject, otherTimeStampedProject) => {
                const oneTimeStamp = oneTimeStampedProject[1] as number
                const otherTimeStamp = otherTimeStampedProject[1] as number
                return otherTimeStamp - oneTimeStamp
            })

        return recentProjectsWithTimeStamp.map((timeStampedProject) => timeStampedProject[0])
    }

    useEffect(() => {
        (async () => {
            try {
                const res = await ProjectService.getProjects()
                console.log("[DashboardView]", res)
                setProjects(res)
                const recPro = getRecentProjects(res)
                setRecentProjects(recPro)
            } catch (error) {
                console.error(error)
            }
        })()
    }, [])

    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project = projects?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
        if (project) {
            navigate(ProjectPath(project.id))
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    if (!projects) return null

    return (
        <Wrapper>
            <FindProjectText variant="h2">{t('DashboardView.FindProject')}</FindProjectText>
            <ProjectSelect>
                <Icon data={search} color={grey} />
                <ProjectDropdown
                    id="select-project"
                    label=""
                    defaultValue="empty"
                    placeholder="Search projects"
                    onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
                >
                    {/* eslint-disable-next-line jsx-a11y/control-has-associated-label */}
                    <option value="empty" disabled />
                    {projects.map((project) => <option value={project.id!} key={project.id}>{project.name!}</option>)}
                </ProjectDropdown>
            </ProjectSelect>
            <RecentProjects projects={recentProjects} />
        </Wrapper>
    )
}

export default DashboardView
