import {
    Tabs,
    Typography,
    Button,
    EdsProvider,
    TextField,
    Input,
    Tooltip,
    NativeSelect,
    Icon,
} from "@equinor/eds-core-react"
import {
    useEffect,
    useState,
    ChangeEventHandler,
    MouseEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { add, link } from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetProjectService } from "../Services/ProjectService"
import DescriptionView from "./DescriptionView"
import { Modal } from "../Components/Modal"
import { GetCaseService } from "../Services/CaseService"

const {
    Panels, Panel,
} = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const CaseHeader = styled(Typography)`
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

const CreateAssetForm = styled.form`
    width: 30rem;

    > * {
        margin-bottom: 1.5rem;
    }
`

const AssetDropdown = styled(NativeSelect)`
    width: 30rem;
`

const Dg4Field = styled(Typography)`
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [createAssetModalIsOpen, setCreateAssetModalIsOpen] = useState<boolean>(false)
    const [createAssetFormData, setCreateAssetFormData] = useState<Record<string, any>>({})
    const [submitIsDisabled, setSubmitIsDisabled] = useState<boolean>(false)
    const [linkAssetModalIsOpen, setLinkAssetModalIsOpen] = useState<boolean>(false)
    const [dg4DateRec, setDg4DateRec] = useState<Record<string, any>>({})

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

    const handleTabChange = (index: number) => {
        setActiveTab(index)
    }

    const toggleCreateAssetModal = () => setCreateAssetModalIsOpen(!createAssetModalIsOpen)

    const toggleLinkAssetModal = () => setLinkAssetModalIsOpen(!linkAssetModalIsOpen)

    const handleCreateAssetFormFieldChange: ChangeEventHandler<HTMLInputElement> = (e) => {
        setCreateAssetFormData({
            ...createAssetFormData,
            [e.target.name]: e.target.value,
        })
    }

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDg4DateRec({
            ...dg4DateRec,
            [e.target.name]: e.target.value,
        })
        const updateDG4 = {
            id: params.caseId,
            projectId: project?.id,
            name: caseItem?.name,
            description: caseItem?.description,
            dG4Date: dg4DateRec?.dg4Date,
        }
        const newProject = await GetCaseService().updateCase(updateDG4)
        setProject(newProject)
    }

    const submitCreateAssetForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        setSubmitIsDisabled(true)

        try {
            setSubmitIsDisabled(false)
            toggleCreateAssetModal()
        } catch (error) {
            setSubmitIsDisabled(false)
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const submitLinkAssetForm: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        setSubmitIsDisabled(true)

        try {
            setSubmitIsDisabled(false)
            toggleLinkAssetModal()
        } catch (error) {
            setSubmitIsDisabled(false)
            console.error("[CaseView] error while submitting form data", error)
        }
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
                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title="Create an asset">
                            <Button variant="ghost_icon" aria-label="Create an asset" onClick={toggleCreateAssetModal}>
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                        <Tooltip title="Link to asset">
                            <Button variant="ghost_icon" aria-label="Link to asset" onClick={toggleLinkAssetModal}>
                                <Icon data={link} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </CaseHeader>
            <Modal isOpen={createAssetModalIsOpen} title="Create an asset" shards={[]}>
                <CreateAssetForm>
                    <AssetDropdown
                        label="Asset type"
                        id="asset"
                        name="asset"
                        placeholder="Choose an asset"
                    />

                    <TextField
                        label="Name"
                        id="name"
                        name="name"
                        placeholder="Name"
                        onChange={handleCreateAssetFormFieldChange}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitCreateAssetForm}
                            disabled={submitIsDisabled}
                        >
                            Create asset
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleCreateAssetModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </CreateAssetForm>
            </Modal>
            <Modal isOpen={linkAssetModalIsOpen} title="Link to asset" shards={[]}>
                <CreateAssetForm>
                    <AssetDropdown
                        label="Asset type"
                        id="asset"
                        name="asset"
                        placeholder="Choose an asset"
                    />

                    <AssetDropdown
                        label="Name"
                        id="name"
                        name="name"
                        placeholder="Name"
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitLinkAssetForm}
                            disabled={submitIsDisabled}
                        >
                            Link to asset
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleLinkAssetModal}
                        >
                            Cancel
                        </Button>
                    </div>
                </CreateAssetForm>
            </Modal>
            <Tabs activeTab={activeTab} onChange={handleTabChange}>
                <Panels>
                    <Panel>
                        <DescriptionView />
                    </Panel>
                </Panels>
                <Panels>
                    <Panel>
                        <Typography>DG4</Typography>
                        <Dg4Field>
                            <Input
                                value={dg4ReturnDate()}
                                id="dg4Date"
                                type="date"
                                name="dg4Date"
                                onInput={handleDg4FieldChange}
                            />
                        </Dg4Field>
                    </Panel>
                </Panels>
            </Tabs>
        </CaseViewDiv>
    )
}

export default CaseView
