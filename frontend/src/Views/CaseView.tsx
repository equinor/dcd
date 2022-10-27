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
import { useHistory, useLocation, useParams } from "react-router-dom"
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
import CaseScheduleTab from "./Case/CaseScheduleTab"
import CaseSummaryTab from "./Case/CaseSummaryTab"
import CaseDrillingScheduleTab from "./Case/CaseDrillingScheduleTab"
import { Well } from "../models/Well"
import { WellProjectWell } from "../models/WellProjectWell"
import { ExplorationWell } from "../models/ExplorationWell"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const CaseViewDiv = styled.div`
    display: flex;
    flex-direction: column;
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
    const [activeTab, setActiveTab] = useState<number>(0)
    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [exploration, setExploration] = useState<Exploration>()
    const [wellProject, setWellProject] = useState<WellProject>()
    const [surf, setSurf] = useState<Surf>()
    const [topside, setTopside] = useState<Topside>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [transport, setTransport] = useState<Transport>()

    const [wells, setWells] = useState<Well[]>()
    // Development
    // eslint-disable-next-line max-len
    const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[]>()
    // Exploration
    // eslint-disable-next-line max-len
    const [explorationWells, setExplorationWells] = useState<ExplorationWell[]>()
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)
    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)
    const toggleCreateCaseModal = () => setCreateCaseModalIsOpen(!createCaseModalIsOpen)

    const history = useHistory()
    const location = useLocation()

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [currentProject?.externalId, caseId, fusionContextId])

    useEffect(() => {
        if (project) {
            const caseResult = project.cases.find((o) => o.id === caseId)
            if (!caseResult) {
                if (location.pathname.indexOf("/case") > -1) {
                    const projectUrl = location.pathname.split("/case")[0]
                    history.push(projectUrl)
                }
            }
            setCase(caseResult)
            setDrainageStrategy(
                project?.drainageStrategies.find((drain) => drain.id === caseResult?.drainageStrategyLink),
            )
            const explorationResult = project
                ?.explorations.find((exp) => exp.id === caseResult?.explorationLink)
            setExploration(explorationResult)
            const wellProjectResult = project
                ?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink)
            setWellProject(wellProjectResult)
            setSurf(project?.surfs.find((sur) => sur.id === caseResult?.surfLink))
            setTopside(project?.topsides.find((top) => top.id === caseResult?.topsideLink))
            setSubstructure(project?.substructures.find((sub) => sub.id === caseResult?.substructureLink))
            setTransport(project?.transports.find((tran) => tran.id === caseResult?.transportLink))

            setWells(project.wells)
            setWellProjectWells(wellProjectResult?.wellProjectWells ?? [])
            setExplorationWells(explorationResult?.explorationWells ?? [])
        }
    }, [project])

    const duplicateCase = async () => {
        try {
            if (caseItem?.id) {
                const newProject = await (await GetCaseService()).duplicateCase(caseItem?.id, {})
                setProject(newProject)
                history.push(ProjectPath(fusionContextId!))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const deleteCase = async () => {
        try {
            if (caseItem?.id && project?.id) {
                const newProject = await (await GetCaseService()).deleteCase(caseItem?.id)
                setProject(newProject)
                history.push(ProjectPath(fusionContextId!))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    if (!project || !caseItem
        || !drainageStrategy || !exploration
        || !wellProject || !surf || !topside
        || !substructure || !transport
        || !explorationWells || !wellProjectWells) {
        return (
            <p>
                Case is missing data:
                {project ? null : "project"}
                <br />
                {caseItem ? null : "case"}
                <br />
                {drainageStrategy ? null : "drainageStrategy"}
                <br />
                {exploration ? null : "exploration"}
                <br />
                {wellProject ? null : "wellProject"}
                <br />
                {surf ? null : "surf"}
                <br />
                {topside ? null : "topside"}
                <br />
                {substructure ? null : "substructure"}
                <br />
                {transport ? null : "transport"}
                <br />
                {explorationWells ? null : "explorationWells"}
                <br />
                {wellProjectWells ? null : "wellProjectWells"}
            </p>
        )
    }

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
                                <CaseDrillingScheduleTab
                                    project={project}
                                    setProject={setProject}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    exploration={exploration}
                                    setExploration={setExploration}
                                    wellProject={wellProject}
                                    setWellProject={setWellProject}
                                    explorationWells={explorationWells}
                                    setExplorationWells={setExplorationWells}
                                    wellProjectWells={wellProjectWells}
                                    setWellProjectWells={setWellProjectWells}
                                    wells={wells}
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
                navigate
            />
            <EditCaseModal
                setProject={setProject}
                project={project}
                caseId={caseItem.id}
                isOpen={createCaseModalIsOpen}
                toggleModal={toggleCreateCaseModal}
                editMode={false}
                navigate
            />
        </div>
    )
}

export default CaseView
