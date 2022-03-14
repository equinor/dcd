import {
    Tabs,
    Typography,
    Button,
    EdsProvider,
    TextField,
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
import { useTranslation } from "react-i18next"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetProjectService } from "../Services/ProjectService"
import DescriptionView from "./DescriptionView"
import { Modal } from "../Components/Modal"
import DrainageStrategyView from "./DrainageStrategyView"
import ExplorationView from "./ExplorationView"

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

const CaseView = () => {
    const { t } = useTranslation()
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const params = useParams()
    const [createAssetModalIsOpen, setCreateAssetModalIsOpen] = useState<boolean>(false)
    const [createAssetFormData, setCreateAssetFormData] = useState<Record<string, any>>({})
    const [submitIsDisabled, setSubmitIsDisabled] = useState<boolean>(false)
    const [linkAssetModalIsOpen, setLinkAssetModalIsOpen] = useState<boolean>(false)

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

    if (!project) return null

    return (
        <CaseViewDiv>
            <CaseHeader>
                <Typography variant="h2">{caseItem?.name}</Typography>
                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title={t("CaseView.CreateAnAsset")}>
                            <Button variant="ghost_icon" aria-label="Create an asset" onClick={toggleCreateAssetModal}>
                                <Icon data={add} />
                            </Button>
                        </Tooltip>
                        <Tooltip title={t("CaseView.LinkToAsset")}>
                            <Button variant="ghost_icon" aria-label="Link to asset" onClick={toggleLinkAssetModal}>
                                <Icon data={link} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </CaseHeader>
            <Modal isOpen={createAssetModalIsOpen} title={t("CaseView.CreateAnAsset")} shards={[]}>
                <CreateAssetForm>
                    <AssetDropdown
                        label={t("CaseView.AssetType")}
                        id="asset"
                        name="asset"
                        placeholder={t("CaseView.ChooseAnAsset")}
                    />

                    <TextField
                        label={t("CaseView.Name")}
                        id="name"
                        name="name"
                        placeholder={t("CaseView.Name")}
                        onChange={handleCreateAssetFormFieldChange}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitCreateAssetForm}
                            disabled={submitIsDisabled}
                        >
                            {t("CaseView.CreateAsset")}
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleCreateAssetModal}
                        >
                            {t("CaseView.Cancel")}
                        </Button>
                    </div>
                </CreateAssetForm>
            </Modal>
            <Modal isOpen={linkAssetModalIsOpen} title={t("CaseView.LinkToAsset")} shards={[]}>
                <CreateAssetForm>
                    <AssetDropdown
                        label={t("CaseView.AssetType")}
                        id="asset"
                        name="asset"
                        placeholder={t("CaseView.ChooseAnAsset")}
                    />

                    <AssetDropdown
                        label={t("CaseView.Name")}
                        id="name"
                        name="name"
                        placeholder={t("CaseView.Name")}
                    />

                    <div>
                        <Button
                            type="submit"
                            onClick={submitLinkAssetForm}
                            disabled={submitIsDisabled}
                        >
                            {t("CaseView.LinkToAsset")}
                        </Button>
                        <Button
                            type="button"
                            color="secondary"
                            variant="ghost"
                            onClick={toggleLinkAssetModal}
                        >
                            {t("CaseView.Cancel")}
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
                        <Typography variant="h4">DD/MM/YYYY</Typography>
                    </Panel>
                </Panels>
                <Panel>
                    {project.drainageStrategies.length > 0 ? <DrainageStrategyView />
                        : <p>No Drainage Strategy for case</p> }
                </Panel>
                <Panel>
                    {project.explorations.length > 0 ? <ExplorationView />
                        : <p>No Explorations for case</p>}
                </Panel>
                <Panel>
                    {project.substructures.length > 0 ? <ExplorationView />
                        : <p>No Subsctructures for case</p>}
                </Panel>
                <Panel>
                    {project.surfs.length > 0 ? <ExplorationView />
                        : <p>No Surfs for case</p>}
                </Panel>
                <Panel>
                    {project.topsides.length > 0 ? <ExplorationView />
                        : <p>No Topsides for case</p>}
                </Panel>
                <Panel>
                    {project.transports.length > 0 ? <ExplorationView />
                        : <p>No Transports for case</p>}
                </Panel>
                <Panel>
                    {project.wellProjects.length > 0 ? <ExplorationView />
                        : <p>No Wellprojects for case</p>}
                </Panel>
            </Tabs>
        </CaseViewDiv>
    )
}

export default CaseView
