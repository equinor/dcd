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
import DefinitionView from "./DefinitionView"
import ExplorationViewTab from "./ExplorationViewTab"
import { EditCaseInputModal } from "./EditCaseInputModal"
import ReadOnlyCostProfile from "../Components/ReadOnlyCostProfile"
import { OpexCostProfile } from "../models/case/OpexCostProfile"
import { GetCaseService } from "../Services/CaseService"
import { StudyCostProfile } from "../models/case/StudyCostProfile"

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
    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const currentProject = useCurrentContext()
    const [opex, setOpex] = useState<OpexCostProfile>()
    const [study, setStudy] = useState<StudyCostProfile>()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)

    const [element, setElement] = useState<HTMLButtonElement>()

    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === caseId)
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [currentProject?.externalId, caseId])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = unwrapCase(project.cases.find((o) => o.id === caseId))
                setCase(caseResult)
                const generatedGAndGAdminCost = await (await GetCaseService()).generateOpexCost(caseResult.id!)
                setOpex(generatedGAndGAdminCost)
                const generateStudy = await (await GetCaseService()).generateStudyCost(caseResult.id!)
                setStudy(generateStudy)
            }
        })()
    }, [project])

    const onMoreClick = (target: any) => {
        setElement(target)
        setIsMenuOpen(!isMenuOpen)
    }

    if (!project) return null
    if (!caseItem) return null

    return (
        <div>
            <TopWrapper>
                <PageTitle variant="h2">{caseItem.name}</PageTitle>
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
                            Exploration
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
                <ReadOnlyCostProfile
                    dG4Year={caseItem.DG4Date?.getFullYear()}
                    timeSeries={caseItem.cessationCost}
                    title="Cessation cost profile"
                />
                <ReadOnlyCostProfile
                    dG4Year={caseItem.DG4Date?.getFullYear()}
                    timeSeries={opex}
                    title="OPEX cost profile"
                />
                <ReadOnlyCostProfile
                    dG4Year={caseItem.DG4Date?.getFullYear()}
                    timeSeries={study}
                    title="Study cost profile"
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
