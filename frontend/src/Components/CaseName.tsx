import {
    Typography,
    Tooltip,
    Icon,
    EdsProvider,
    Button,
    TextField,
} from "@equinor/eds-core-react"
import {
    useState,
    ChangeEventHandler,
    MouseEventHandler,
    Dispatch,
    SetStateAction,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { edit } from "@equinor/eds-icons"
import { Modal } from "../Components/Modal"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetCaseService } from "../Services/CaseService"

const CaseHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

const EditCaseNameForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>
}

const CaseName = ({
    setProject,
    caseItem,
    setCase,
}: Props) => {
    const params = useParams()
    const [caseNameModalIsOpen, setCaseNameModalIsOpen] = useState<boolean>(false)
    const [caseName, setCaseName] = useState<string>(caseItem?.name ?? "")

    const toggleEditCaseNameModal = () => setCaseNameModalIsOpen(!caseNameModalIsOpen)

    const handleCaseNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseName(e.target.value)
    }

    const submitUpdateCaseName = async () => {
        const caseDto = Case.Copy(caseItem!)
        caseDto.name = caseName
        const newProject = await GetCaseService().updateCase(caseDto)
        setProject(newProject)
        const caseResult = newProject.cases.find((o) => o.id === params.caseId)
        setCase(caseResult)
    }

    const submitEditCaseNameForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()

        try {
            submitUpdateCaseName()
            toggleEditCaseNameModal()
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    return (
        <>
            <CaseHeader>
                <Typography
                    variant="h2"
                    defaultValue={caseItem?.name}
                    key={caseItem?.name}
                >
                    {caseItem?.name}
                </Typography>
                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title="Edit case name">
                            <Button
                                variant="ghost_icon"
                                aria-label={`Edit ${caseItem?.name}`}
                                onClick={toggleEditCaseNameModal}
                            >
                                <Icon data={edit} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </CaseHeader>
            <Modal isOpen={caseNameModalIsOpen} title="Edit case name" shards={[]}>
                <EditCaseNameForm>
                    <TextField
                        label="Case name"
                        id="caseName"
                        name="name"
                        placeholder={caseItem?.name}
                        onChange={handleCaseNameChange}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitEditCaseNameForm}
                            disabled={caseName === ""}
                        >
                            Submit
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleEditCaseNameModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </EditCaseNameForm>
            </Modal>
        </>
    )
}

export default CaseName
