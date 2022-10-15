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
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import CaseDescriptionTab from "./Case/CaseDescriptionTab"
import ExplorationViewTab from "./ExplorationViewTab"
import { EditCaseInputModal } from "./EditCaseInputModal"
import ReadOnlyCostProfile from "../Components/ReadOnlyCostProfile"
import { OpexCostProfile } from "../models/case/OpexCostProfile"
import { GetCaseService } from "../Services/CaseService"
import { StudyCostProfile } from "../models/case/StudyCostProfile"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { WellProject } from "../models/assets/wellproject/WellProject"
import { Surf } from "../models/assets/surf/Surf"
import { Topside } from "../models/assets/topside/Topside"
import { Substructure } from "../models/assets/substructure/Substructure"
import { Exploration } from "../models/assets/exploration/Exploration"
import { Transport } from "../models/assets/transport/Transport"
import CaseScheduleTab from "./Case/CaseScheduleTab"

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

function CaseView() {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [activeTab, setActiveTab] = useState<number>(0)
    const { caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()

    // const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    // const [exploration, setExploration] = useState<Exploration>()
    // const [wellProject, setWellProject] = useState<WellProject>()
    // const [surf, setSurf] = useState<Surf>()
    // const [topside, setTopside] = useState<Topside>()
    // const [substructure, setSubstructure] = useState<Substructure>()
    // const [transport, setTransport] = useState<Transport>()

    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === caseId)
                setCase(caseResult)
                // setDrainageStrategy(project?.drainageStrategies.find((ds) => ds.id === caseResult?.drainageStrategyLink))
                // setExploration(project?.explorations.find((e) => e.id === caseResult?.explorationLink))
                // setWellProject(project?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink))
                // setSurf(project?.surfs.find((s) => s.id === caseResult?.surfLink))
                // setTopside(project?.topsides.find((t) => t.id === caseResult?.topsideLink))
                // setSubstructure(project?.substructures.find((s) => s.id === caseResult?.substructureLink))
                // setTransport(project?.transports.find((t) => t.id === caseResult?.transportLink))
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
                    onClick={() => toggleEditCaseModal()}
                >
                    Edit Case input
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
                        <Tab>  </Tab>
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
                            <p>Production Profiles</p>
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
                            <p>Facilities</p>
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
