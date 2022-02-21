import { ChangeEvent, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { UseComboboxStateChange } from 'downshift';
import { tokens } from '@equinor/eds-tokens';
import { search } from '@equinor/eds-icons';
import { Icon, SingleSelect, TextField, Button } from '@equinor/eds-core-react';
import { CommonLibraryService } from '../Services/CommonLibraryService';
import { ProjectService } from '../Services/ProjectService'
import { Modal } from '../Components/Modal';
import { ConvertProjectPhaseEnumToString, ConvertProjectCategoryEnumToString } from '../Utils/common';

const ProjectSelect = styled.div`
    display: flex;
    align-items: center;
`

const ProjectDropdown = styled(SingleSelect)`
    width: 25rem;
    margin-left: 0.5rem;
`

const grey = tokens.colors.ui.background__scrim.rgba;

interface Props {
	isOpen: boolean;
	closeModal: Function;
    shards: any[];
}

const CreateProjectView = ({ isOpen, closeModal, shards }: Props) => {
    const navigate = useNavigate();
    const [projects, setProjects] = useState<Components.Schemas.CommonLibraryProjectDto[]>();
    const [selectedProject, setSelectedProject] = useState<Components.Schemas.CommonLibraryProjectDto>();
    const [inputName, setName] = useState<string>();
    const [inputDescription, setDescription] = useState<string>();
    const onSelected = (selectedValue: string | null | undefined) => {
        const project = projects?.find(p => p.name === selectedValue);
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
                const res = await CommonLibraryService.getProjects();
                setProjects(res);
            } catch (error) {
                console.error(`[CreateProjectView] Error while fetching common library projects.`, error);
            }
        })()
    }, []);

    if (!projects) return null;

	return (
		<Modal isOpen={isOpen} title='Create Project' shards={shards}>
            <div>
                <ProjectSelect>
                    <Icon data={search} color={grey} />
                    <ProjectDropdown
                        id="textfield-name"
                        label={'CommonLib project'}
                        placeholder={'Select a CommonLib project'}
                        items={projects!.map(p => p.name!)}
                        handleSelectedItemChange={(changes: UseComboboxStateChange<string>) => onSelected(changes.selectedItem)}
                    />
                </ProjectSelect>
                <div>
                    <TextField
                        label="Name"
                        id="textfield-name"
                        placeholder={selectedProject?.name!}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => updateNameHandler(event)}
                    />
                </div>
                <div>
                    <TextField
                        label="Description"
                        id="textfield-description"
                        placeholder={selectedProject?.description!}
                        autoComplete="off"
                        onChange={(event: ChangeEvent<HTMLTextAreaElement>) => updateDescriptionHandler(event)}
                    />
                </div>
                <div>
                    <TextField
                        label="Category"
                        id="textfield-description"
                        placeholder={ConvertProjectCategoryEnumToString(selectedProject?.projectCategory!)}
                        autoComplete="off"
                        readOnly={true}
                    />
                </div>
                <div>
                    <TextField
                        label="Phase"
                        id="textfield-description"
                        placeholder={ConvertProjectPhaseEnumToString(selectedProject?.projectPhase!)}
                        autoComplete="off"
                        readOnly={true}
                    />
                </div>
                <div>
                    <TextField
                        label="Country"
                        id="textfield-description"
                        placeholder={selectedProject?.country!}
                        autoComplete="off"
                        readOnly={true}
                    />
                </div>
                <div>
                    <Button onClick={handleOkClick}>Create Project</Button>
                    <Button onClick={handleCancelClick} variant='outlined'>Cancel</Button>
                </div>
            </div>
		</Modal>
	);
}

export default CreateProjectView;
