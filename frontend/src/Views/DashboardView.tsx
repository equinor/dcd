import { ChangeEvent, useEffect, useState } from "react"
import {
    Icon, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../models/Project"

import { GetProjectService } from "../Services/ProjectService"

import { ProjectPath } from "../Utils/common"
import { GetCommonLibraryService } from "../Services/CommonLibraryService"
import { Modal } from "../Components/Modal"
import CreateProjectView from "./CreateProjectView"

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

type Props = {
    shards: any[];
}

const DashboardView = ({ shards }: Props) => {
    const navigate = useNavigate()

    const ProjectService = GetProjectService()

    const [selectedProject, setCurrentProject] = useState<Components.Schemas.CommonLibraryProjectDto | undefined>()
    const [isOpen, setIsOpen] = useState(false)
    const [isFetching, setFetching] = useState<boolean>(false)
    const [projects, setProjects] = useState<Project[]>()
    const [clp, setCommonLibProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>()
    const CommonLibraryService = GetCommonLibraryService()

    const closeModal = () => {
        setIsOpen(false)
    }

    useEffect(() => {
        (async () => {
            if (!clp) {
                try {
                    setFetching(true)
                    const commonLib = await CommonLibraryService.getProjects()
                    const res = await ProjectService.getProjects()
                    console.log("[DashboardView]", res)
                    setProjects(res)
                    setCommonLibProjects(commonLib)
                    setFetching(false)
                } catch (error) {
                    console.error(error)
                }
            }
        })()
    }, [])

    const onSelected = async (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project = projects?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
        if (project) {
            navigate(ProjectPath(project.id))
        } else {
            const commonlibProject = clp?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
            setCurrentProject(commonlibProject)
            setIsOpen(true)
        }
    }

    const grey = tokens.colors.ui.background__scrim.rgba

    if (isFetching) {
        return (
            <Modal isOpen={isFetching} title="Getting data" shards={shards}>
                <Typography>Retrieving projects from Common Library.</Typography>
            </Modal>
        )
    }

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
                                value={project.id!}
                                key={project.id}
                                style={{
                                    fontWeight: 700,
                                    background: clp?.find((p) => p.id === project.commonLibId) === undefined
                                        ? "rgba(255,0,0,.5)" : "rgba(51,170,51,.4)",
                                }}
                            >
                                {project.name!}
                            </option>
                        ))}
                        {clp?.filter((p) => !projects.find((proj) => p.id === proj.commonLibId)).map((project) => (
                            <option
                                value={project.id!}
                                key={project.id!}
                            >
                                {project.name!}
                            </option>
                        ))}
                    </ProjectDropdown>
                </ProjectSelect>
            </Wrapper>
            <CreateProjectView
                passedInProject={selectedProject!}
                isOpen={isOpen}
                shards={shards}
                closeModal={closeModal}
            />
        </>
    )
}

export default DashboardView
