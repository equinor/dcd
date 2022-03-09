import {
    Button,
    Icon,
    NativeSelect,
    TextField,
    Typography,
} from "@equinor/eds-core-react"
import { ChangeEvent, useEffect, useState } from "react"
import { search } from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { useNavigate } from "react-router-dom"
import styled from "styled-components"

import { Modal } from "../Components/Modal"

import { ProjectCategory } from "../models/ProjectCategory"
import { ProjectPhase } from "../models/ProjectPhase"

import { GetCommonLibraryService } from "../Services/CommonLibraryService"
import { GetProjectService } from "../Services/ProjectService"
import { useTranslation } from "react-i18next";


const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(NativeSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const grey = tokens.colors.ui.background__scrim.rgba

type Props = {
    isOpen: boolean;
    closeModal: Function;
    shards: any[];
}

const CreateProjectView = ({ isOpen, closeModal, shards }: Props) => {
    const { t } = useTranslation()
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
        setSelectedProject(undefined)
        closeModal()
    }

    const handleOkClick = () => {
        closeCreateProjectView(true)
    }

    const handleCancelClick = () => {
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

    if (commonLibFetchError) {
        return (
            <Modal isOpen={isOpen} title={t('CreateProjectView.Oops')} shards={shards}>
                <Typography>
                    {t('CreateProjectView.SomethingWrong')}
                    {" "}
                    {t('CreateProjectView.UnableToCreateProject')}
                </Typography>
                <Button onClick={handleCancelClick}>{t('CreateProjectView.Close')}</Button>
            </Modal>
        )
    }

    if (!projects) {
        return (
            <Modal isOpen={isOpen} title={t('CreateProjectView.GettingData')} shards={shards}>
                <Typography>{t('CreateProjectView.RetrievingProjects')}</Typography>
            </Modal>
        )
    }

    return (
        <Modal isOpen={isOpen} title={t('CreateProjectView.CreateProject')} shards={shards}>
            <div>
                <ProjectSelect>
                    <Icon data={search} color={grey} />
                    <ProjectDropdown
                        id="select-project"
                        label={t('CreateProjectView.ComLibProject')}
                        placeholder={t('CreateProjectView.SelectComLibProject')}
                        onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
                    >
                        {/* eslint-disable-next-line jsx-a11y/control-has-associated-label */}
                        <option disabled selected />
                        {projects.map((project) => (
                            <option value={project.id!} key={project.id}>{project.name!}</option>
                        ))}
                    </ProjectDropdown>
                </ProjectSelect>
                <div>
                    <TextField
                        label={t('CreateProjectView.Name')}
                        id="textfield-name"
                        placeholder={selectedProject?.name!}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => (
                            updateNameHandler(event)
                        )}
                    />
                </div>
                <div>
                    <TextField
                        label={t('CreateProjectView.Description')}
                        id="textfield-description"
                        placeholder={selectedProject?.description!}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => (
                            updateDescriptionHandler(event)
                        )}
                    />
                </div>
                <div>
                    <TextField
                        label={t('CreateProjectView.Category')}
                        id="textfield-description"
                        placeholder={new ProjectCategory(selectedProject?.projectCategory!).toString()}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <div>
                    <TextField
                        label={t('CreateProjectView.Phase')}
                        id="textfield-description"
                        placeholder={new ProjectPhase(selectedProject?.projectPhase!).toString()}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <div>
                    <TextField
                        label={t('CreateProjectView.Country')}
                        id="textfield-description"
                        placeholder={selectedProject?.country!}
                        autoComplete="off"
                        readOnly
                    />
                </div>
                <div>
                    <Button onClick={handleOkClick}>{t('CreateProjectView.CreateProject')}</Button>
                    <Button onClick={handleCancelClick} variant="outlined">{t('CreateProjectView.Cancel')}</Button>
                </div>
            </div>
        </Modal>
    )
}

export default CreateProjectView
