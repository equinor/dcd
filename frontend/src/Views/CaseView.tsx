import {
    Tabs,
    Typography,
    Input,
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

const {
    Panels, Panel,
} = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const CaseHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const Dg4Field = styled.div`
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

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

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [dg4DateRec, setDg4DateRec] = useState<Record<string, any>>({})
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

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

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

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDg4DateRec({
            dg4DateRec,
            [e.target.name]: e.target.value,
        })
        const updateDG4 = {
            id: params.caseId,
            projectId: project?.id,
            name: caseItem?.name,
            description: caseItem?.description,
            dG4Date: e.target.value,
        }
        const newProject = await GetCaseService().updateCase(updateDG4)
        setProject(newProject)
    }

    const dg4ReturnDate = () => {
        const dg4DateGet = caseItem?.DG4Date?.toLocaleDateString("en-CA")
        if (dg4DateGet !== "0001-01-01") {
            return dg4DateGet
        }
        return ""
    }

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader>
                <Typography variant="h2">{caseItem?.name}</Typography>
            </CaseHeader>
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <Panels>
                    <Panel>
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
                    </Panel>
                </Panels>
                <Panels>
                    <Panel>
                        <Typography>DG4</Typography>
                        <Dg4Field>
                            <Input
                                defaultValue={dg4ReturnDate()}
                                key={dg4ReturnDate()}
                                id="dg4Date"
                                type="date"
                                name="dg4Date"
                                onChange={handleDg4FieldChange}
                            />
                        </Dg4Field>
                    </Panel>
                </Panels>
            </Tabs>
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
        </CaseViewDiv>
    )
}

export default CaseView
