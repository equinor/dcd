/* eslint-disable max-len */
/* eslint-disable camelcase */
import {
    Switch,
    Button,
    Icon,
    Menu,
    Tabs,
    Typography,
} from "@equinor/eds-core-react"
import {
    MouseEventHandler,
    useEffect,
    useState,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import {
    add, delete_to_trash, edit, library_add, more_vertical,
} from "@equinor/eds-icons"
import { Project } from "../models/Project"
import { Case } from "../models/case/Case"
import { GetProjectService } from "../Services/ProjectService"
import CaseAsset from "../Components/Case/CaseAsset"
import CaseDescription from "../Components/Case/CaseDescription"
import CaseName from "../Components/Case/CaseName"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import CaseDGDate from "../Components/Case/CaseDGDate"
import CaseArtificialLift from "../Components/Case/CaseArtificialLift"
import DGEnum from "../models/DGEnum"
import ProductionStrategyOverview from "../Components/ProductionStrategyOverview"
import NumberInput from "../Components/NumberInput"
import { GetCaseService } from "../Services/CaseService"
import ExcelUpload from "../Components/ExcelUpload"
import DefinitionView from "./DefinitionView"
import ExplorationView from "./ExplorationView"
import ExplorationViewTab from "./ExplorationViewTab"
import { Modal } from "../Components/Modal"
import { EditCaseInputModal } from "./EditCaseInputModal"
import CaseCessationCostProfile from "../Components/Case/CaseCessationCostProfile"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    > *:not(:last-child) {
        margin-right: 1rem;
    }
    flex-direction: row;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    padding: 1.5rem 2rem;
`

const PageTitle = styled(Typography)`
    flex-grow: 1;
`

const InvisibleButton = styled(Button)`
    border: 1px solid #007079;
`

const TransparentButton = styled(Button)`
    color: #007079;
    background-color: white;
    border: 1px solid #007079;
`

const DividerLine = styled.div`
    background: gray;
    height: 0.05rem;
    width: 50rem;
    margin-bottom: 2rem;
    margin-top: 2rem;
`

const StyledTabPanel = styled(Panel)`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

function CaseView() {
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)

    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const { fusionProjectId, caseId } = useParams<Record<string, string | undefined>>()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift>(0)
    const [prodStratOverview, setProdStratOverview] = useState<Components.Schemas.ProductionStrategyOverview>(0)
    const [producerCount, setProducerCount] = useState<number>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>()
    const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()
    const [isReferenceCase, setIsReferenceCase] = useState<boolean | undefined>()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [element, setElement] = useState<HTMLButtonElement>()

    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(fusionProjectId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${fusionProjectId}`, error)
            }
        })()
    }, [fusionProjectId, caseId])

    useEffect(() => {
        if (project !== undefined) {
            const caseResult = project.cases.find((o) => o.id === caseId)
            if (caseResult !== undefined) {
                setArtificialLift(caseResult.artificialLift)
                setProdStratOverview(caseResult.productionStrategyOverview)
                setFacilitiesAvailability(caseResult?.facilitiesAvailability)
                setIsReferenceCase(caseResult?.referenceCase ?? false)
            }
            setCase(caseResult)
            setProducerCount(caseResult?.producerCount)
            setGasInjectorCount(caseResult?.gasInjectorCount)
            setWaterInjectorCount(caseResult?.waterInjectorCount)
            setFacilitiesAvailability(caseResult?.facilitiesAvailability)
        }
    }, [project])

    useEffect(() => {
        (async () => {
            if (caseItem) {
                const caseDto = Case.Copy(caseItem)
                caseDto.producerCount = producerCount
                caseDto.gasInjectorCount = gasInjectorCount
                caseDto.waterInjectorCount = waterInjectorCount
                caseDto.facilitiesAvailability = facilitiesAvailability
                caseDto.referenceCase = isReferenceCase ?? false

                const newProject = await (await GetCaseService()).updateCase(caseDto)
                setCase(newProject.cases.find((o) => o.id === caseItem.id))
            }
        })()
    }, [producerCount, gasInjectorCount, waterInjectorCount, facilitiesAvailability, isReferenceCase])

    const onMoreClick = (target: any) => {
        setElement(target)
        setIsMenuOpen(!isMenuOpen)
    }

    const switchReference: MouseEventHandler<HTMLInputElement> = () => {
        if (!isReferenceCase || isReferenceCase === undefined) {
            setIsReferenceCase(true)
        } else setIsReferenceCase(false)
    }

    if (!project) return null
    if (!caseItem) return null

    return (
        <div>
            <TopWrapper>
                <PageTitle variant="h4">{caseItem.name}</PageTitle>
                <TransparentButton
                    onClick={() => toggleEditCaseModal()}
                >
                    Edit Case input
                </TransparentButton>
                <InvisibleButton
                    onClick={(e) => onMoreClick(e.target)}
                >
                    <Icon data={more_vertical} />
                </InvisibleButton>
            </TopWrapper>
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={element}
                onClose={() => setIsMenuOpen(false)}
                placement="bottom"
            >
                <Menu.Item
                    onClick={() => console.log("Add new case clicked")}
                >
                    <Icon data={add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Add New Case
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => console.log("Duplicate clicked")}
                >
                    <Icon data={library_add} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Duplicate
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => console.log("Rename clicked")}
                >
                    <Icon data={edit} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Rename
                    </Typography>
                </Menu.Item>
                <Menu.Item
                    onClick={() => console.log("Delete clicked")}
                >
                    <Icon data={delete_to_trash} size={16} />
                    <Typography group="navigation" variant="menu_title" as="span">
                        Delete
                    </Typography>
                </Menu.Item>
            </Menu>
            <CaseViewDiv>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <List>
                        <Tab>Definition </Tab>
                        <Tab>Facilities </Tab>
                        <Tab>Drainage Strategy</Tab>
                        <Tab>Exploration</Tab>
                        <Tab>Well</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <DefinitionView
                                project={project}
                                setProject={setProject}
                                caseItem={caseItem}
                                setCase={setCase}
                                artificialLift={artificialLift}
                                setArtificialLift={setArtificialLift}
                                productionStrategyOverview={prodStratOverview}
                                setProductionStrategyOverview={setProdStratOverview}
                                switchReference={switchReference}
                                isReferenceCase={isReferenceCase}
                                facilitiesAvailability={facilitiesAvailability}
                                setFacilitiesAvailability={setFacilitiesAvailability}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Facilities</p>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Case with name: </p>
                            {caseItem.name}
                            <p>Drainage Strategy</p>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <ExplorationViewTab
                                _case={caseItem}
                                _project={project}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Well</p>
                        </StyledTabPanel>
                    </Panels>
                </Tabs>

                <CaseName
                    caseItem={caseItem}
                    setProject={setProject}
                    setCase={setCase}
                />
                <ExcelUpload setProject={setProject} setCase={setCase} />
                <CaseDescription
                    caseItem={caseItem}
                    setProject={setProject}
                    setCase={setCase}
                />
                <Switch
                    onClick={switchReference}
                    label="Reference case"
                    readOnly
                    checked={isReferenceCase ?? false}
                />
                <Wrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG0}
                        dGName="DG0"
                    />
                </Wrapper>
                <Wrapper>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG1}
                        dGName="DG1"
                    />
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG3}
                        dGName="DG3"
                    />
                </Wrapper>
                <Wrapper style={{ marginBottom: -35 }}>
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG2}
                        dGName="DG2"
                    />
                    <CaseDGDate
                        caseItem={caseItem}
                        setProject={setProject}
                        setCase={setCase}
                        dGType={DGEnum.DG4}
                        dGName="DG4"
                    />
                </Wrapper>
                <CaseArtificialLift
                    currentValue={artificialLift}
                    setArtificialLift={setArtificialLift}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <ProductionStrategyOverview
                    currentValue={prodStratOverview}
                    setProductionStrategyOverview={setProdStratOverview}
                    setProject={setProject}
                    caseItem={caseItem}
                />
                <DividerLine />
                <Wrapper style={{ marginBottom: -15 }}>
                    <CaseArtificialLift
                        currentValue={artificialLift}
                        setArtificialLift={setArtificialLift}
                        setProject={setProject}
                        caseItem={caseItem}
                    />
                    <ProductionStrategyOverview
                        currentValue={prodStratOverview}
                        setProductionStrategyOverview={setProdStratOverview}
                        setProject={setProject}
                        caseItem={caseItem}
                    />
                </Wrapper>
                <DividerLine />
                <Wrapper style={{ marginBottom: 45 }}>
                    <NumberInput
                        setValue={setProducerCount}
                        value={producerCount ?? 0}
                        integer
                        disabled={false}
                        label="Producer count"
                    />
                    <NumberInput
                        setValue={setGasInjectorCount}
                        value={gasInjectorCount ?? 0}
                        integer
                        disabled={false}
                        label="Gas injector count"
                    />
                    <NumberInput
                        setValue={setWaterInjectorCount}
                        value={waterInjectorCount ?? 0}
                        integer
                        disabled={false}
                        label="Water injector count"
                    />
                </Wrapper>
                <DividerLine />
                <CaseCessationCostProfile
                    dG4Year={caseItem.DG4Date?.getFullYear()}
                    timeSeries={caseItem.cessationCost}
                />
                <DividerLine />
                <CaseAsset
                    caseItem={caseItem}
                    project={project}
                    setProject={setProject}
                    setCase={setCase}
                    caseId={caseId}
                />

            </CaseViewDiv>
            <EditCaseInputModal
                toggleEditCaseModal={toggleEditCaseModal}
                caseItem={caseItem}
                isOpen={editCaseModalIsOpen}
                shards={[]}
            />
        </div>
    )
}

export default CaseView
