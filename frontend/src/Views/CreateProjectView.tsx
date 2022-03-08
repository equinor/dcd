import {
    NativeSelect,
    TextField,
    Button,
    Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState, VoidFunctionComponent } from "react"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"

import { Modal, ModalActionsContainer } from "../Components/Modal"
import { ProjectCategory } from "../models/ProjectCategory"
import { ProjectPhase } from "../models/ProjectPhase"

import { GetCommonLibraryService } from "../Services/CommonLibraryService"
import { GetProjectService } from "../Services/ProjectService"

const ProjectForm = styled.form`
    > *:not(:last-child) {
        margin-bottom: 1rem;
    }
`

type Props = {
    isOpen: boolean;
    closeModal: Function;
}

const CreateProjectView: VoidFunctionComponent<Props> = ({ isOpen, closeModal }) => {
    const navigate = useNavigate()
    const [projects, setProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>()
    const [selectedProject, setSelectedProject] = useState<Components.Schemas.CommonLibraryProjectDto>()
    const [inputName, setName] = useState<string>()
    const [inputDescription, setDescription] = useState<string>()
    const [commonLibFetchError, setCommonLibFetchError] = useState<boolean>()
    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project = projects?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
        setSelectedProject(project)
    }

    const CommonLibraryService = GetCommonLibraryService()
    const ProjectService = GetProjectService()

    const convertCommonLibProjectToProject = (
        commonLibraryProject: Components.Schemas.CommonLibraryProjectDto,
    ): Components.Schemas.ProjectDto => {
        const project: Components.Schemas.ProjectDto = {
            name: inputName ?? commonLibraryProject?.name,
            commonLibraryId: commonLibraryProject?.id,
            commonLibraryName: commonLibraryProject?.name,
            description: inputDescription ?? commonLibraryProject?.description,
            country: commonLibraryProject?.country ?? "",
            projectPhase: commonLibraryProject?.projectPhase,
            projectCategory: commonLibraryProject?.projectCategory,
            cases: [],
            explorations: [],
            surfs: [],
            substructures: [],
            topsides: [],
            transports: [],
            drainageStrategies: [],
            wellProjects: [],
        }
        return project
    }

    const closeCreateProjectView = async (pressedOkButton: boolean) => {
        let project
        if (pressedOkButton === true) {
            project = convertCommonLibProjectToProject(selectedProject!)
            const createdProject = await ProjectService.createProject(project)
            navigate(`/project/${createdProject.projectId}`)
        }
        setSelectedProject(undefined)
        closeModal()
    }

    const handleOkClick = () => {
        closeCreateProjectView(true)
    }

    const handleModalDismiss = () => {
        closeCreateProjectView(false)
    }

    const updateNameHandler = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setName(event.target.value)
    }

    const updateDescriptionHandler = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setDescription(event.target.value)
    }

    useEffect(() => {
        (async () => {
            try {
                setCommonLibFetchError(false)
                const res = await CommonLibraryService.getProjects()
                setProjects(res)
            } catch (error) {
                setCommonLibFetchError(true)
                console.error("[CreateProjectView] Error while fetching common library projects.", error)
            }
        })()
    }, [])

    if (!isOpen) return null

    if (commonLibFetchError) {
        return (
            <Modal title="Oops!" onDismiss={handleModalDismiss}>
                <Typography>
                    Something went wrong while retrieving projects from Common Library.
                    {" "}
                    Unable to create a new DCD project right now.
                </Typography>
                <Button onClick={handleModalDismiss}>Close</Button>
            </Modal>
        )
    }

    if (!projects) {
        return (
            <Modal title="Oops!" onDismiss={handleModalDismiss}>
                <Typography>
                    Something went wrong while retrieving projects from Common Library.
                    {" "}
                    Unable to create a new DCD project right now.
                </Typography>
                <Button onClick={handleModalDismiss}>Close</Button>
            </Modal>
        )
    }

    return (
        <Modal title="Create a project" onDismiss={handleModalDismiss}>
            <ProjectForm>
                <NativeSelect
                    id="select-project"
                    label="CommonLib project"
                    defaultValue="Select a CommonLib project"
                    onChange={onSelected}
                >
                    {projects.map((project) => (
                        <option value={project.id!} key={project.id}>{project.name!}</option>
                    ))}
                </NativeSelect>
                <TextField
                    label="Name"
                    id="textfield-name"
                    placeholder={selectedProject?.name!}
                    autoComplete="off"
                    onChange={updateNameHandler}
                />
                <TextField
                    label="Description"
                    id="textfield-description"
                    placeholder={selectedProject?.description!}
                    autoComplete="off"
                    onChange={updateDescriptionHandler}
                    multiline
                />
                <TextField
                    label="Category"
                    id="textfield-description"
                    placeholder={new ProjectCategory(selectedProject?.projectCategory!).toString()}
                    autoComplete="off"
                    readOnly
                />
                <TextField
                    label="Phase"
                    id="textfield-description"
                    placeholder={new ProjectPhase(selectedProject?.projectPhase!).toString()}
                    autoComplete="off"
                    readOnly
                />
                <TextField
                    label="Country"
                    id="textfield-description"
                    placeholder={selectedProject?.country!}
                    autoComplete="off"
                    readOnly
                />

                <ModalActionsContainer>
                    <Button onClick={handleOkClick} type="submit">Create Project</Button>
                    <Button onClick={handleModalDismiss} variant="outlined" type="button">Cancel</Button>
                </ModalActionsContainer>
            </ProjectForm>
        </Modal>
    )
}

export default CreateProjectView
