import {
    Typography,
    EdsProvider,
    Tooltip,
    Button,
    Icon,
    TextField,
} from "@equinor/eds-core-react"
import {
    useEffect,
    useState,
    ChangeEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { edit } from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetProjectService } from "../Services/ProjectService"
import { GetCaseService } from "../Services/CaseService"
import { Modal } from "../Components/Modal"

const Wrapper = styled.div`
    display: flex;
    width: 70%;
    flex-direction: row;
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

const CreateAssetForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

interface Props {
    setProject: React.Dispatch<React.SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: React.Dispatch<React.SetStateAction<Case | undefined>>
}

const CaseDescription = ({
    setProject,
    caseItem,
    setCase,
}: Props) => {
    const params = useParams()
    const [caseDescriptionModalIsOpen, setCaseDescriptionModalIsOpen] = useState<boolean>(false)
    const [caseDescription, setCaseDescription] = useState<string>("")

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const toggleEditCaseDescriptionModal = () => setCaseDescriptionModalIsOpen(!caseDescriptionModalIsOpen)

    const handleDescriptionChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDescription(e.target.value)
    }

    const submitUpdateDescription = async () => {
        const caseDto = Case.Copy(caseItem!)
        caseDto.description = caseDescription
        const newProject = await GetCaseService().updateCase(caseDto)
        setProject(newProject)
        const caseResult = newProject.cases.find((o) => o.id === params.caseId)
        setCase(caseResult)
    }

    return (
        <>
            <Wrapper>
                <Typography
                    variant="h4"
                    id="textfield-description"
                    defaultValue={caseItem?.description}
                    key={caseItem?.description}
                >
                    {caseItem?.description}
                    <EdsProvider density="compact">
                        <ActionsContainer>
                            <Tooltip title="Edit description">
                                <Button
                                    variant="ghost_icon"
                                    aria-label={`Edit ${caseItem?.description}`}
                                    onClick={toggleEditCaseDescriptionModal}
                                >
                                    <Icon data={edit} />
                                </Button>
                            </Tooltip>
                        </ActionsContainer>
                    </EdsProvider>
                </Typography>
            </Wrapper>
            <Modal isOpen={caseDescriptionModalIsOpen} title="Edit description" shards={[]}>
                <CreateAssetForm>
                    <TextField
                        label="Description"
                        id="description"
                        name="description"
                        placeholder={caseItem?.description}
                        onChange={handleDescriptionChange}
                    />
                    <div>
                        <Button
                            type="submit"
                            onClick={submitUpdateDescription}
                            disabled={caseDescription === ""}
                        >
                            Submit
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleEditCaseDescriptionModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </CreateAssetForm>
            </Modal>
        </>
    )
}

export default CaseDescription
