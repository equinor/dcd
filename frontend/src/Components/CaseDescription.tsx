import {
    Typography,
    EdsProvider,
    Tooltip,
    Button,
    Icon,
} from "@equinor/eds-core-react"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { edit } from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetCaseService } from "../Services/CaseService"
import { Modal } from "../Components/Modal"
import { unwrapCase } from "../Utils/common"

const Wrapper = styled.div`
    display: flex;
    width: 70%;
    flex-direction: row;
    margin-bottom: 2rem;
`
const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`
const TextArea = styled.textarea`
    width: 100%;
    height: 10rem;
    font-size: 17px;
    max-width:100%;
    min-width:100%;
`

const PreserveLinebreak = styled.div`
    white-space: pre-wrap;
`

const CreateDescriptionForm = styled.form`
    width: 40rem;
    > * {
        margin-bottom: 0.5rem;
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
    const toggleEditCaseDescriptionModal = () => setCaseDescriptionModalIsOpen(!caseDescriptionModalIsOpen)
    const handleDescriptionChange: ChangeEventHandler<HTMLTextAreaElement> = async (e) => {
        setCaseDescription(e.target.value)
    }
    const submitUpdateDescription = async () => {
        const unwrappedCase: Case = unwrapCase(caseItem)
        const caseDto = Case.Copy(unwrappedCase)
        caseDto.description = caseDescription
        const newProject: Project = await GetCaseService().updateCase(caseDto)
        setProject(newProject)
        const caseResult: Case = unwrapCase(newProject.cases.find((o) => o.id === params.caseId))
        setCase(caseResult)
    }
    const submitDescriptionForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            submitUpdateDescription()
            toggleEditCaseDescriptionModal()
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }
    return (
        <>
            <Wrapper>
                <PreserveLinebreak>
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
                </PreserveLinebreak>
            </Wrapper>
            <Modal isOpen={caseDescriptionModalIsOpen} title="Edit description" shards={[]}>
                <CreateDescriptionForm>
                    <TextArea
                        id="description"
                        name="description"
                        defaultValue={caseItem?.description}
                        onChange={handleDescriptionChange}
                    />
                    <div>
                        <Button
                            type="submit"
                            onClick={submitDescriptionForm}
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
                </CreateDescriptionForm>
            </Modal>
        </>
    )
}
export default CaseDescription
