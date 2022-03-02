import { ChangeEvent, useEffect, useState } from "react"
import { Icon, NativeSelect, Typography } from "@equinor/eds-core-react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"

import RecentProjects from "../Components/RecentProjects"

import { GetProjectService } from "../Services/ProjectService"

import { ProjectPath, RetrieveRecentProjects } from "../Utils/common"

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

function DashboardView() {
    const navigate = useNavigate()

    const ProjectService = GetProjectService()

    const [projects, setProjects] = useState<any[]>()

    useEffect(() => {
        (async () => {
            try {
                const res = await ProjectService.getProjects()
                console.log("[DashboardView]", res)
                setProjects(res)
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

    const recentProjects = RetrieveRecentProjects()

    if (!projects) return null

    return (
        <Wrapper>
            <FindProjectText variant="h2">Find a project</FindProjectText>
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
