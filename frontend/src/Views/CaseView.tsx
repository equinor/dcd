/* eslint-disable max-len */
/* eslint-disable camelcase */
import {
    Button,
    Icon,
    Menu,
    Tabs,
    Typography,
} from "@equinor/eds-core-react"
import {
    useEffect,
    useState,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import {
    add, delete_to_trash, edit, library_add, more_vertical,
} from "@equinor/eds-icons"
import { useCurrentContext } from "@equinor/fusion"
import { Project } from "../models/Project"
import { Case } from "../models/case/Case"
import { GetProjectService } from "../Services/ProjectService"
import CaseAsset from "../Components/Case/CaseAsset"
import { unwrapProjectId } from "../Utils/common"
import CaseDescriptionTab from "./Case/CaseDescriptionTab"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Surf } from "../models/assets/surf/Surf"
import { Topside } from "../models/assets/topside/Topside"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Transport } from "../models/assets/transport/Transport"
import CaseScheduleTab from "./Case/CaseScheduleTab"
import CaseFacilitiesTab from "./Case/CaseFacilitiesTab"
import CaseProductionProfilesTab from "./Case/CaseProductionProfilesTab"
import EditTechnicalInputModal from "../Components/EditTechnicalInput/EditTechnicalInputModal"
import DrillingScheduleViewTab from "./DrillingScheduleViewTab"
import { Well } from "../models/Well"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
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

const CaseView = () => {
    const [editTechnicalInputModalIsOpen, setEditTechnicalInputModalIsOpen] = useState<boolean>(false)

    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(3)
    const { caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [exploration, setExploration] = useState<Exploration>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [surf, setSurf] = useState<Surf>()
    const [topside, setTopside] = useState<Topside>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [transport, setTransport] = useState<Transport>()
    const [wells, setWells] = useState<Well[]>()

    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === caseId)
                setCase(caseResult)
                setDrainageStrategy(projectResult?.drainageStrategies.find((drain) => drain.id === caseResult?.drainageStrategyLink))
                setExploration(projectResult?.explorations.find((exp) => exp.id === caseResult?.explorationLink))
                setWellProject(projectResult?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink))
                setSurf(projectResult?.surfs.find((sur) => sur.id === caseResult?.surfLink))
                setTopside(projectResult?.topsides.find((top) => top.id === caseResult?.topsideLink))
                setSubstructure(projectResult?.substructures.find((sub) => sub.id === caseResult?.substructureLink))
                setTransport(projectResult?.transports.find((tran) => tran.id === caseResult?.transportLink))
                setWells(projectResult?.wells)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [currentProject?.externalId, caseId])

    if (!project) return null
    if (!caseItem) return null

    return (
        <div>
            <TopWrapper>
                <PageTitle variant="h4">{caseItem.name}</PageTitle>
                <TransparentButton
                    onClick={() => toggleTechnicalInputModal()}
                >
                    Edit technical input
                </TransparentButton>
                <InvisibleButton
                    variant="outlined"
                    ref={setMenuAnchorEl}
                    onClick={() => (isMenuOpen ? setIsMenuOpen(false) : setIsMenuOpen(true))}
                >
                    <Icon data={more_vertical} />
                </InvisibleButton>
            </TopWrapper>
            <Menu
                id="menu-complex"
                open={isMenuOpen}
                anchorEl={menuAnchorEl}
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
                        <Tab>Description</Tab>
                        <Tab>Production Profiles</Tab>
                        <Tab>Schedule</Tab>
                        <Tab>Drilling Schedule</Tab>
                        <Tab>Facilities</Tab>
                        <Tab>Cost</Tab>
                        <Tab>CO2 Emissions</Tab>
                        <Tab>Summary</Tab>
                    </List>
                    <Panels>
                        <StyledTabPanel>
                            <CaseDescriptionTab
                                project={project}
                                setProject={setProject}
                                caseItem={caseItem}
                                setCase={setCase}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <CaseProductionProfilesTab
                                project={project}
                                setProject={setProject}
                                caseItem={caseItem}
                                setCase={setCase}
                                drainageStrategy={drainageStrategy}
                                setDrainageStrategy={setDrainageStrategy}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <CaseScheduleTab
                                project={project}
                                setProject={setProject}
                                caseItem={caseItem}
                                setCase={setCase}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <DrillingScheduleViewTab
                                _case={caseItem}
                                _project={project}
                                exploration={exploration}
                                setExploration={setExploration}
                                wellProject={wellProject}
                                setWellProject={setWellProject}
                                _wells={wells}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <CaseFacilitiesTab
                                project={project}
                                setProject={setProject}
                                caseItem={caseItem}
                                setCase={setCase}
                                topside={topside}
                                setTopside={setTopside}
                                surf={surf}
                                setSurf={setSurf}
                                substructure={substructure}
                                setSubstrucutre={setSubstructure}
                                transport={transport}
                                setTransport={setTransport}
                            />
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Cost</p>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>CO2 Emissions</p>
                        </StyledTabPanel>
                        <StyledTabPanel>
                            <p>Summary</p>
                        </StyledTabPanel>
                    </Panels>
                </Tabs>
                <DividerLine />
                <CaseAsset
                    caseItem={caseItem}
                    project={project}
                    setProject={setProject}
                    setCase={setCase}
                    caseId={caseId}
                />

            </CaseViewDiv>
            <EditTechnicalInputModal
                toggleEditTechnicalInputModal={toggleTechnicalInputModal}
                isOpen={editTechnicalInputModalIsOpen}
                project={project}
                setProject={setProject}
            />
        </div>
    )
}

export default CaseView
