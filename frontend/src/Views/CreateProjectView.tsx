import {
    Button,
    Icon,
    NativeSelect,
    TextField,
    Typography,
} from "@equinor/eds-core-react"
import React, { ChangeEvent, useEffect, useState } from "react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"

import { Modal } from "../Components/Modal"

import { ProjectCategory } from "../models/ProjectCategory"
import { ProjectPhase } from "../models/ProjectPhase"

import { GetCommonLibraryService } from "../Services/CommonLibraryService"
import { GetProjectService } from "../Services/ProjectService"

const ProjectSelect = styled.div`
    margin-top: 1.5rem;
    display: flex;
    align-items: center;
`

const StyledTextField = styled(TextField)`
    margin-top: 1rem;
`

const Margin = styled.div`
    margin-top: 1rem;
`

const Description = styled.div`
    display: flex;
    align-items: center;
    padding-top: 6px;
    margin-bot: 1rem;
    max-width: 90%;
`

const Label = styled.div`
    margin-top: 1rem;
    font-family: Equinor;
    font-size: 0.750rem;
    font-weight: 500;
    line-height: 1.333em;
    text-align: left;
    margin-left: 8px;
    margin-right: 8px;
    color: var(--eds_text__static_icons__tertiary,rgba(111,111,111,1));
`

const ProjectDropdown = styled(NativeSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const TextArea = styled.textarea`
    width: 70%;
    height: 7rem;
    font-size: 17px;
    max-width:50rem;
    min-width:90%;
    max-height: 30rem;
    min-height: 7rem;
`

const grey = tokens.colors.ui.background__scrim.rgba

type Props = {
    isOpen: boolean;
    closeModal: Function;
    shards: any[];
    passedInProject: Components.Schemas.CommonLibraryProjectDto;
}

const CreateProjectView = ({
    passedInProject, isOpen, closeModal, shards,
}: Props) => {
    const navigate = useNavigate()
    const [projects, setProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>()
    const [selectedProject, setSelectedProject] = useState<Components.Schemas.CommonLibraryProjectDto>(passedInProject)
    const [inputName, setName] = useState<string>()
    const [inputDescription, setDescription] = useState<string>()
    const [commonLibFetchError, setCommonLibFetchError] = useState<boolean>()
    const CommonLibraryService = GetCommonLibraryService()

    // When user is inside the view, and chooses a new project.
    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project = projects?.find((p) => p.id === event.currentTarget.selectedOptions[0].value)
        setSelectedProject(project!)
    }

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
            const createdProject = await GetProjectService().createProject(project)
            navigate(`/project/${createdProject.projectId}`)
        }
        setSelectedProject(undefined!)
        closeModal()
    }

    const handleOkClick = () => {
        closeCreateProjectView(true)
    }

    const handleCancelClick = () => {
        setSelectedProject(undefined!)
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
                setSelectedProject(passedInProject)
                setCommonLibFetchError(false)
                const res = await CommonLibraryService.getProjects()
                setProjects(res)
            } catch (error) {
                setCommonLibFetchError(true)
                console.error("[CreateProjectView] Error while fetching common library projects.", error)
            }
        })()
    }, [passedInProject])

    if (commonLibFetchError) {
        return (
            <Modal isOpen={isOpen} title="Oops!" shards={shards}>
                <Typography>
                    Something went wrong while retrieving projects from Common Library.
                    {" "}
                    Unable to create a new DCD project right now.
                </Typography>
                <Button onClick={handleCancelClick}>Close</Button>
            </Modal>
        )
    }

    return (
        <Modal isOpen={isOpen} title="Add Project from CommonLib" shards={shards}>
            <div>
                <ProjectSelect>
                    <Icon data={search} color={grey} />
                    <ProjectDropdown
                        id="select-project"
                        label="CommonLib projects"
                        onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
                        defaultValue=""
                    >
                        {/* eslint-disable-next-line jsx-a11y/control-has-associated-label */}
                        <option value={selectedProject?.name ?? ""}>{selectedProject?.name ?? ""}</option>
                        {projects?.map((project) => (
                            <option value={project.id!} key={project.id}>{project.name!}</option>
                        ))}
                    </ProjectDropdown>
                </ProjectSelect>
                <div>
                    <StyledTextField
                        label="Name"
                        id="textfield-name"
                        value={selectedProject?.name ?? "hei"}
                        defaultValue={selectedProject?.name ?? "halla"}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => (
                            updateNameHandler(event)
                        )}
                        readOnly
                    />
                </div>
                <Label>
                    <label htmlFor="textfield-description">Description</label>
                </Label>
                <Description>
                    <TextArea
                        id="textfield-description"
                        defaultValue={selectedProject?.description ?? ""}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => (
                            updateDescriptionHandler(event)
                        )}
                    />
                </Description>
                <div>
                    <StyledTextField
                        label="Category"
                        id="category"
                        placeholder={new ProjectCategory(selectedProject?.projectCategory!).toString()}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <div>
                    <StyledTextField
                        label="Phase"
                        id="phase"
                        placeholder={new ProjectPhase(selectedProject?.projectPhase!).toString()}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <div>
                    <StyledTextField
                        label="Country"
                        id="country"
                        value={selectedProject?.country ?? ""}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <Margin>
                    <Button
                        onClick={handleOkClick}
                        disabled={!selectedProject}
                    >
                        Add Project
                    </Button>
                    <Button onClick={handleCancelClick} variant="outlined">Cancel</Button>
                </Margin>
            </div>
        </Modal>
    )
}

export default CreateProjectView
