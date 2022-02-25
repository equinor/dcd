import { ChangeEvent, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { tokens } from '@equinor/eds-tokens';
import { search } from '@equinor/eds-icons';
import { Dialog, Icon, NativeSelect, TextField, Button, Typography } from '@equinor/eds-core-react';
import { CommonLibraryService } from '../Services/CommonLibraryService';
import { ProjectService } from '../Services/ProjectService'
import { ConvertProjectPhaseEnumToString, ConvertProjectCategoryEnumToString } from '../Utils/common';

const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(NativeSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const grey = tokens.colors.ui.background__scrim.rgba;

interface Props {
    isOpen: boolean;
    closeModal: Function;
}

const CreateProjectView = ({ isOpen, closeModal }: Props) => {
    const navigate = useNavigate();
    const [projects, setProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>();
    const [selectedProject, setSelectedProject] = useState<Components.Schemas.CommonLibraryProjectDto>();
    const [inputName, setName] = useState<string>();
    const [inputDescription, setDescription] = useState<string>();
    const [error, setError] = useState<boolean>();
    const onSelected = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const project = projects?.find(p => p.id === event.currentTarget.selectedOptions[0].value);
        setSelectedProject(project);
    }

    const handleOkClick = () => {
        closeCreateProjectView(true);
    }
    const handleCancelClick = () => {
        closeCreateProjectView(false);
    }

    const closeCreateProjectView = async (pressedOkButton: boolean) => {
        let project;
        if (pressedOkButton === true) {
            project = convertCommonLibProjectToProject(selectedProject!);
            var createdProject = await ProjectService.createProject(project);
            navigate('/project/' + createdProject.projectId);
        }
        setSelectedProject(undefined);
        closeModal();
    }

    const convertCommonLibProjectToProject = (commonLibraryProject: Components.Schemas.CommonLibraryProjectDto): Components.Schemas.ProjectDto => {
        let project: Components.Schemas.ProjectDto = {
            name: inputName ?? commonLibraryProject?.name,
            commonLibraryName: commonLibraryProject?.name,
            description: inputDescription ?? commonLibraryProject?.description,
            country: commonLibraryProject?.country ?? '',
            projectPhase: commonLibraryProject?.projectPhase,
            projectCategory: commonLibraryProject?.projectCategory,
            cases: [],
            explorations: [],
            surfs: [],
            substructures: [],
            topsides: [],
            transports: [],
            drainageStrategies: [],
            wellProjects: []
        }
        return project;
    }

    const updateNameHandler = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setName(event.target.value);
    }

    const updateDescriptionHandler = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setDescription(event.target.value);
    }

    useEffect(() => {
        (async () => {
            try {
                setError(false);
                const res = await CommonLibraryService.getProjects();
                setProjects(res);
            } catch (error) {
                setError(true);
                console.error(`[CreateProjectView] Error while fetching common library projects.`, error);
            }
        })()
    }, []);

    if (!isOpen) {
        return null;
    }

    if (error)
        return (
            <Dialog title='Oops!'>
                <Dialog.CustomContent>
                    <Typography>Something went wrong while retrieving projects from Common Library. Unable to create a new DCD project right now.</Typography>
                    <Button onClick={handleCancelClick}>Close</Button>
                </Dialog.CustomContent>
            </Dialog>
        );

    if (!projects)
        return (
            <Dialog title='Getting data'>
                <Dialog.CustomContent>
                    <Typography>Retrieving projects from Common Library.</Typography>
                </Dialog.CustomContent>
            </Dialog>
        );

    return (
        <Dialog>
            <Dialog.Title>
                Create Project
            </Dialog.Title>
            <Dialog.CustomContent>
                <ProjectSelect>
                    <Icon data={search} color={grey} />
                    <ProjectDropdown
                        id="select-project"
                        label={'CommonLib project'}
                        placeholder={'Select a CommonLib project'}
                        onChange={(event: ChangeEvent<HTMLSelectElement>) => onSelected(event)}
                    >
                        <option disabled selected />
                        {projects.map(project => (<option value={project.id!} key={project.id}>{project.name!}</option>))}
                    </ProjectDropdown>
                </ProjectSelect>
                <TextField
                    label="Name"
                    id="textfield-name"
                    placeholder={selectedProject?.name!}
                    autoComplete="off"
                    onChange={(event: ChangeEvent<HTMLTextAreaElement>) => updateNameHandler(event)}
                />
                <TextField
                    label="Description"
                    id="textfield-description"
                    placeholder={selectedProject?.description!}
                    autoComplete="off"
                    onChange={(event: ChangeEvent<HTMLTextAreaElement>) => updateDescriptionHandler(event)}
                />
                <TextField
                    label="Category"
                    id="textfield-description"
                    placeholder={ConvertProjectCategoryEnumToString(selectedProject?.projectCategory!)}
                    autoComplete="off"
                    readOnly={true}
                />
                <TextField
                    label="Phase"
                    id="textfield-description"
                    placeholder={ConvertProjectPhaseEnumToString(selectedProject?.projectPhase!)}
                    autoComplete="off"
                    readOnly={true}
                />
                <TextField
                    label="Country"
                    id="textfield-description"
                    placeholder={selectedProject?.country!}
                    autoComplete="off"
                    readOnly={true}
                />
            </Dialog.CustomContent>
            <Dialog.Actions>
                <Button onClick={handleOkClick}>Create Project</Button>
                <Button onClick={handleCancelClick} variant='outlined'>Cancel</Button>
            </Dialog.Actions>
        </Dialog>
    );
}

export default CreateProjectView;
