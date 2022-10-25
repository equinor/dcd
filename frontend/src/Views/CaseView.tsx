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
import { useHistory, useParams } from "react-router-dom"
import styled from "styled-components"
import {
    add, delete_to_trash, edit, library_add, more_vertical,
} from "@equinor/eds-icons"
import { useCurrentContext } from "@equinor/fusion"
import { Project } from "../models/Project"
import { Case } from "../models/case/Case"
import { GetProjectService } from "../Services/ProjectService"
import CaseAsset from "../Components/Case/CaseAsset"
import { ProjectPath, unwrapProjectId } from "../Utils/common"
import CaseDescriptionTab from "./Case/CaseDescriptionTab"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Surf } from "../models/assets/surf/Surf"
import { Topside } from "../models/assets/topside/Topside"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Transport } from "../models/assets/transport/Transport"
import EditTechnicalInputModal from "../Components/EditTechnicalInput/EditTechnicalInputModal"
import CaseCostTab from "./Case/CaseCostTab"
import CaseFacilitiesTab from "./Case/CaseFacilitiesTab"
import CaseProductionProfilesTab from "./Case/CaseProductionProfilesTab"
import { GetCaseService } from "../Services/CaseService"
import EditCaseModal from "../Components/Case/EditCaseModal"
import CreateCaseModal from "../Components/Case/CreateCaseModal"
import CaseScheduleTab from "./Case/CaseScheduleTab"
import CaseSummaryTab from "./Case/CaseSummaryTab"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const CaseViewDiv = styled.div`
    display: flex;
    flex-direction: column;
`

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    z-index: 1000;
`

const PageTitle = styled(Typography)`
    flex-grow: 1;
    padding-left: 30px;
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
`

const StyledTabPanel = styled(Panel)`
    margin-left: 40px;
    margin-right: 40px;
    padding-top: 0px;
`
const HeaderWrapper = styled.div`
    background-color: white;
    width: calc(100% - 16rem);
    position: fixed;
    z-index: 100;
    padding-top: 30px;
`
const TabMenuWrapper = styled.div`
    position: fixed;
    z-index: 1000;
    width: calc(100% - 16rem);
    border-bottom: 1px solid LightGray;
    margin-top: 95px;
`

const TabContentWrapper = styled.div`
    margin-top: 145px;
`

const CaseButtonsWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    align-content: right;
    margin-left: auto;
    z-index: 110;
`

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 78px;
`

const CaseView = () => {
    const [editTechnicalInputModalIsOpen, setEditTechnicalInputModalIsOpen] = useState<boolean>(false)

    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(5)
    const { caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [exploration, setExploration] = useState<Exploration>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [surf, setSurf] = useState<Surf>()
    const [topside, setTopside] = useState<Topside>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [transport, setTransport] = useState<Transport>()

    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)

    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)
    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)
    const toggleCreateCaseModal = () => setCreateCaseModalIsOpen(!createCaseModalIsOpen)

    const history = useHistory()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === caseId)
                setCase(caseResult)
                setDrainageStrategy(
                    projectResult?.drainageStrategies.find((drain) => drain.id === caseResult?.drainageStrategyLink),
                )
                setExploration(projectResult?.explorations.find((exp) => exp.id === caseResult?.explorationLink))
                setWellProject(projectResult?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink))
                setSurf(projectResult?.surfs.find((sur) => sur.id === caseResult?.surfLink))
                setTopside(projectResult?.topsides.find((top) => top.id === caseResult?.topsideLink))
                setSubstructure(projectResult?.substructures.find((sub) => sub.id === caseResult?.substructureLink))
                setTransport(projectResult?.transports.find((tran) => tran.id === caseResult?.transportLink))
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [currentProject?.externalId, caseId])

    const duplicateCase = async () => {
        try {
            if (caseItem?.id) {
                const newProject = await (await GetCaseService()).duplicateCase(caseItem?.id, {})
                setProject(newProject)
                history.push(ProjectPath(newProject?.id))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const deleteCase = async () => {
        try {
            if (caseItem?.id) {
                const newProject = await (await GetCaseService()).deleteCase(caseItem?.id)
                setProject(newProject)
                history.push(ProjectPath(newProject?.id))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    if (!project) return null
    if (!caseItem) return null
    if (!project || !caseItem
        || !drainageStrategy || !exploration
        || !wellProject || !surf || !topside
        || !substructure || !transport) { return null }

    return (
        <div>
            <HeaderWrapper>
                <RowWrapper>
                    <PageTitle variant="h4">{caseItem.name}</PageTitle>
                    <ColumnWrapper>
                        <CaseButtonsWrapper>
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
                        </CaseButtonsWrapper>
                    </ColumnWrapper>
                </RowWrapper>
                <Menu
                    id="menu-complex"
                    open={isMenuOpen}
                    anchorEl={menuAnchorEl}
                    onClose={() => setIsMenuOpen(false)}
                    placement="bottom"
                >
                    <Menu.Item
                        onClick={toggleCreateCaseModal}
                    >
                        <Icon data={add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Add New Case
                        </Typography>
                    </Menu.Item>
                    <Menu.Item
                        onClick={duplicateCase}
                    >
                        <Icon data={library_add} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Duplicate
                        </Typography>
                    </Menu.Item>
                    <Menu.Item
                        onClick={toggleEditCaseModal}
                    >
                        <Icon data={edit} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Rename
                        </Typography>
                    </Menu.Item>
                    <Menu.Item
                        onClick={deleteCase}
                    >
                        <Icon data={delete_to_trash} size={16} />
                        <Typography group="navigation" variant="menu_title" as="span">
                            Delete
                        </Typography>
                    </Menu.Item>
                </Menu>
            </HeaderWrapper>
            <CaseViewDiv>
                <Tabs activeTab={activeTab} onChange={setActiveTab}>
                    <TabMenuWrapper>
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
                    </TabMenuWrapper>
                    <TabContentWrapper>
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
                                <p>Drilling Schedule</p>
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
                                <CaseCostTab
                                    project={project}
                                    setProject={setProject}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    exploration={exploration}
                                    setExploration={setExploration}
                                    wellProject={wellProject}
                                    setWellProject={setWellProject}
                                    topside={topside}
                                    setTopside={setTopside}
                                    surf={surf}
                                    setSurf={setSurf}
                                    substructure={substructure}
                                    setSubstructure={setSubstructure}
                                    transport={transport}
                                    setTransport={setTransport}
                                    drainageStrategy={drainageStrategy}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <p>CO2 Emissions</p>
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseSummaryTab
                                    project={project}
                                    setProject={setProject}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    exploration={exploration}
                                    setExploration={setExploration}
                                    wellProject={wellProject}
                                    setWellProject={setWellProject}
                                    topside={topside}
                                    setTopside={setTopside}
                                    surf={surf}
                                    setSurf={setSurf}
                                    substructure={substructure}
                                    setSubstrucutre={setSubstructure}
                                    transport={transport}
                                    setTransport={setTransport}
                                    drainageStrategy={drainageStrategy}
                                />
                            </StyledTabPanel>
                        </Panels>
                    </TabContentWrapper>
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
            <EditCaseModal
                setProject={setProject}
                project={project}
                caseId={caseItem.id}
                isOpen={editCaseModalIsOpen}
                toggleModal={toggleEditCaseModal}
                editMode
            />
            <CreateCaseModal
                setProject={setProject}
                isOpen={createCaseModalIsOpen}
                project={project}
                toggleModal={toggleCreateCaseModal}
                editMode={false}
            />
        </div>
    )
}

export default CaseView
