import { ChangeEvent, useEffect, useState } from "react"
import {
    Icon, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { useHistory } from "@equinor/fusion"
import { Project } from "../models/Project"

import { GetProjectService } from "../Services/ProjectService"

import { ProjectPath } from "../Utils/common"
import { GetCommonLibraryService } from "../Services/CommonLibraryService"
import { Modal } from "../Components/Modal"
import ProjectModal from "./ProjectModal"

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
    font-weight: bold;
`

const DashboardView = () => {
    const ProjectService = GetProjectService()
    const history = useHistory()

    const [selectedProject, setCurrentProject] = useState<Components.Schemas.CommonLibraryProjectDto | undefined>()
    const [isOpen, setIsOpen] = useState(false)
    const [isFetching, setFetching] = useState<boolean>(false)
    const [projects, setProjects] = useState<Project[]>()
    const [clp, setCommonLibProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>()
    const CommonLibraryService = GetCommonLibraryService()
    const { fusionContextId } = useParams<Record<string, string | undefined>>()

    const closeModal = () => {
        setIsOpen(false)
    }

    useEffect(() => {
        (async () => {
            if (!clp || !projects) {
                try {
                    setFetching(true)
                    await (await CommonLibraryService).getProjects().then((commonlibProjects) => {
                        setCommonLibProjects(commonlibProjects)
                    })
                    await (await ProjectService).getProjects().then((dcdProjects) => {
                        setProjects(dcdProjects)
                    })
                    setFetching(false)
                } catch (error) {
                    console.error("Could not retrieve commonlibprojects or dcdprojects. ", error)
                }
            }
        })()
    }, [])

    const onSelected = async (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project:Project | undefined = projects?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
        if (project) {
           history.push(ProjectPath(fusionContextId!))
        } else {
            const commonlibProject = clp?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
            setCurrentProject(commonlibProject)
            setIsOpen(true)
        }
    }

    const grey: string = tokens.colors.ui.background__scrim.rgba

    if (!projects) return null

    /* Below code maps two lists, commonlib projects (clp) and projects (stored projects) into the same list of options
    Clp is filtering out stored projects */
    return (
        <>
            <Wrapper>
                <FindProjectText variant="h2">Select Project</FindProjectText>
                <ProjectSelect>
                    <Icon data={search} color={grey} />
                    <ProjectDropdown
                        id="select-project"
                        label=""
                        defaultValue="empty"
                        placeholder="Search projects"
                        onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
                    >
                        <option value="empty" disabled> </option>
                        {projects?.map((project) => (
                            <option
                                value={project.id}
                                key={project.id}
                                style={{
                                    fontWeight: 700,
                                    background: clp?.find((p) => p.id === project.commonLibraryId) === undefined
                                        ? "rgba(255,0,0,.5)" : "rgba(51,170,51,.4)",
                                }}
                            >
                                {project.name}
                            </option>
                        ))}
                        {clp?.filter((p) => !projects.find((proj) => p.id === proj.commonLibraryId)).map((project) => (
                            <option
                                value={project.id}
                                key={project.id}
                            >
                                {project.name}
                            </option>
                        ))}
                    </ProjectDropdown>
                </ProjectSelect>
            </Wrapper>
            <ProjectModal
                passedInProject={selectedProject}
                passedInProjects={clp}
                isOpen={isOpen}
                closeModal={closeModal}
            />
        </>
    )
}

export default DashboardView
