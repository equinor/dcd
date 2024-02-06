import {
    Button, Icon, Menu, Progress, Tabs, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useHistory, useLocation, useParams } from "react-router-dom"
import styled from "styled-components"
import {
    add,
    bookmark_filled,
    bookmark_outlined,
    delete_to_trash,
    edit,
    library_add,
    more_vertical,
} from "@equinor/eds-icons"
import { useCurrentContext } from "@equinor/fusion"
import { tokens } from "@equinor/eds-tokens"
import { Tooltip } from "@material-ui/core"
import { Project } from "../models/Project"
import { Case } from "../models/case/Case"
import { GetProjectService } from "../Services/ProjectService"
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
import CaseCO2Tab from "./Case/CaseCO2Tab"
import { GetCaseWithAssetsService } from "../Services/CaseWithAssetsService"
import { EMPTY_GUID } from "../Utils/constants"
import { CessationOffshoreFacilitiesCost } from "../models/case/CessationOffshoreFacilitiesCost"
import { CessationWellsCost } from "../models/case/CessationWellsCost"
import { OffshoreFacilitiesOperationsCostProfile } from "../models/case/OffshoreFacilitiesOperationsCostProfile"
import { TotalFeasibilityAndConceptStudies } from "../models/case/TotalFeasibilityAndConceptStudies"
import { TotalFEEDStudies } from "../models/case/TotalFEEDStudies"
import { WellInterventionCostProfile } from "../models/case/WellInterventionCostProfile"
import { GAndGAdminCost } from "../models/assets/exploration/GAndGAdminCost"
import { Co2Emissions } from "../models/assets/drainagestrategy/Co2Emissions"
import { FuelFlaringAndLosses } from "../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { NetSalesGas } from "../models/assets/drainagestrategy/NetSalesGas"
import { ImportedElectricity } from "../models/assets/drainagestrategy/ImportedElectricity"

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

const TransparentButton = styled(Button)`
    color: #007079;
    background-color: white;
    border: 1px solid #007079;
    margin-left: 1rem;
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
const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
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

    const [originalCase, setOriginalCase] = useState<Case>()
    const [originalDrainageStrategy, setOriginalDrainageStrategy] = useState<DrainageStrategy>()
    const [originalWellProject, setOriginalWellProject] = useState<WellProject>()
    const [originalExploration, setOriginalExploration] = useState<Exploration>()
    const [originalSurf, setOriginalSurf] = useState<Surf>()
    const [originalSubstructure, setOriginalSubstructure] = useState<Substructure>()
    const [originalTopside, setOriginalTopside] = useState<Topside>()
    const [originalTransport, setOriginalTransport] = useState<Transport>()

    const [wells, setWells] = useState<Well[]>()
    const [wellProjectWells, setWellProjectWells] = useState<WellProjectWell[]>()
    const [explorationWells, setExplorationWells] = useState<ExplorationWell[]>()

    const [originalWellProjectWells, setOriginalWellProjectWells] = useState<WellProjectWell[]>()
    const [originalExplorationWells, setOriginalExplorationWells] = useState<ExplorationWell[]>()

    const [totalFeasibilityAndConceptStudies,
        setTotalFeasibilityAndConceptStudies] = useState<TotalFeasibilityAndConceptStudies>()

    const [totalFEEDStudies, setTotalFEEDStudies] = useState<TotalFEEDStudies>()

    const [offshoreFacilitiesOperationsCostProfile,
        setOffshoreFacilitiesOperationsCostProfile] = useState<OffshoreFacilitiesOperationsCostProfile>()

    const [wellInterventionCostProfile, setWellInterventionCostProfile] = useState<WellInterventionCostProfile>()

    const [cessationWellsCost, setCessationWellsCost] = useState<CessationWellsCost>()
    const [cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost] = useState<CessationOffshoreFacilitiesCost>()

    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCost>()

    const [co2Emissions, setCo2Emissions] = useState<Co2Emissions>()

    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()
    const [importedElectricity, setImportedElectricity] = useState<ImportedElectricity>()

    const [editCaseModalIsOpen, setEditCaseModalIsOpen] = useState<boolean>(false)
    const [createCaseModalIsOpen, setCreateCaseModalIsOpen] = useState<boolean>(false)

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)
    const toggleEditCaseModal = () => setEditCaseModalIsOpen(!editCaseModalIsOpen)
    const toggleCreateCaseModal = () => setCreateCaseModalIsOpen(!createCaseModalIsOpen)

    const history = useHistory()
    const location = useLocation()

    const [isLoading, setIsLoading] = useState<boolean>()
    const [isSaving, setIsSaving] = useState<boolean>()
    const [updateFromServer, setUpdateFromServer] = useState<boolean>(true)

    useEffect(() => {
        (async () => {
            try {
                setUpdateFromServer(true)
                setIsLoading(true)
                const projectId = unwrapProjectId(currentProject?.externalId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentProject?.externalId}`, error)
            }
        })()
    }, [currentProject?.externalId, caseId, fusionContextId])

    useEffect(() => {
        if (project && updateFromServer) {
            const caseResult = project.cases.find((o) => o.id === caseId)
            if (!caseResult) {
                if (location.pathname.indexOf("/case") > -1) {
                    const projectUrl = location.pathname.split("/case")[0]
                    history.push(projectUrl)
                }
            }
            setOriginalCase(caseResult)
            setCase(caseResult)

            const drainageStrategyResult = project?.drainageStrategies
                .find((drain) => drain.id === caseResult?.drainageStrategyLink)
            setOriginalDrainageStrategy(drainageStrategyResult)
            setDrainageStrategy(
                drainageStrategyResult,
            )

            const explorationResult = project
                ?.explorations.find((exp) => exp.id === caseResult?.explorationLink)
            setOriginalExploration(explorationResult)
            setExploration(explorationResult)

            const wellProjectResult = project
                ?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink)
            setOriginalWellProject(wellProjectResult)
            setWellProject(wellProjectResult)

            const surfResult = project?.surfs.find((sur) => sur.id === caseResult?.surfLink)
            setOriginalSurf(surfResult)
            setSurf(surfResult)

            const topsideResult = project?.topsides.find((top) => top.id === caseResult?.topsideLink)
            setOriginalTopside(topsideResult)
            setTopside(topsideResult)

            const substructureResult = project?.substructures.find((sub) => sub.id === caseResult?.substructureLink)
            setOriginalSubstructure(substructureResult)
            setSubstructure(substructureResult)

            const transportResult = project?.transports.find((tran) => tran.id === caseResult?.transportLink)
            setOriginalTransport(transportResult)
            setTransport(transportResult)

            setWells(project.wells)

            const wellProjectWellsResult = structuredClone(wellProjectResult?.wellProjectWells)
            setWellProjectWells(wellProjectResult?.wellProjectWells ?? [])
            setOriginalWellProjectWells(wellProjectWellsResult ?? [])

            const explorationWellsResult = structuredClone(explorationResult?.explorationWells)
            setExplorationWells(explorationResult?.explorationWells ?? [])
            setOriginalExplorationWells(explorationWellsResult ?? [])

            setUpdateFromServer(false)
            setIsLoading(false)
        } else if (project) {
            const caseResult = project.cases.find((o) => o.id === caseId)

            const surfResult = project.surfs.find((sur) => sur.id === caseResult?.surfLink)
            setOriginalSurf(surfResult)
            setSurf(surfResult)

            const topsideResult = project.topsides.find((top) => top.id === caseResult?.topsideLink)
            setOriginalTopside(topsideResult)
            setTopside(topsideResult)

            const substructureResult = project.substructures.find((sub) => sub.id === caseResult?.substructureLink)
            setOriginalSubstructure(substructureResult)
            setSubstructure(substructureResult)

            const transportResult = project.transports.find((tran) => tran.id === caseResult?.transportLink)
            setOriginalTransport(transportResult)
            setTransport(transportResult)

            setWells(project.wells)
        }
    }, [project])

    const duplicateCase = async () => {
        try {
            if (caseItem?.id && project?.id) {
                const newProject = await (await GetCaseService()).duplicateCase(project.id, caseItem?.id, {})
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
                const newProject = await (await GetCaseService()).deleteCase(project.id, caseItem?.id)
                setProject(newProject)
                history.push(ProjectPath(fusionContextId!))
            }
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    if (isLoading || !project || !caseItem
        || !drainageStrategy || !exploration
        || !wellProject || !surf || !topside
        || !substructure || !transport
        || !explorationWells || !wellProjectWells) {
        return (
            <>
                <Progress.Circular size={16} color="primary" />
                <p>Loading case</p>
            </>
        )
    }

    const setCaseAsReference = async () => {
        try {
            const projectDto = Project.Copy(project)
            if (projectDto.referenceCaseId === caseItem.id) {
                projectDto.referenceCaseId = EMPTY_GUID
            } else {
                projectDto.referenceCaseId = caseItem.id
            }
            const newProject = await (await GetProjectService()).setReferenceCase(projectDto)
            setProject(newProject)
        } catch (error) {
            console.error("[ProjectView] error while submitting form data", error)
        }
    }

    const handleSave = async () => {
        const dto: Components.Schemas.CaseWithAssetsWrapperDto = {}

        dto.caseDto = caseItem
        if (!(JSON.stringify(caseItem) === JSON.stringify(originalCase))) {
            dto.caseDto.hasChanges = true
        }

        dto.drainageStrategyDto = drainageStrategy
        if (!(JSON.stringify(drainageStrategy) === JSON.stringify(originalDrainageStrategy))) {
            dto.drainageStrategyDto.hasChanges = true
        }

        dto.wellProjectDto = wellProject
        if (!(JSON.stringify(wellProject) === JSON.stringify(originalWellProject))) {
            dto.wellProjectDto.hasChanges = true
        }

        dto.explorationDto = exploration
        if (!(JSON.stringify(exploration) === JSON.stringify(originalExploration))) {
            dto.explorationDto.hasChanges = true
        }

        dto.surfDto = surf
        if (!(JSON.stringify(surf) === JSON.stringify(originalSurf))) {
            dto.surfDto.hasChanges = true
        }

        dto.substructureDto = substructure
        if (!(JSON.stringify(substructure) === JSON.stringify(originalSubstructure))) {
            dto.substructureDto.hasChanges = true
        }

        dto.transportDto = transport
        if (!(JSON.stringify(transport) === JSON.stringify(originalTransport))) {
            dto.transportDto.hasChanges = true
        }

        dto.topsideDto = topside
        if (!(JSON.stringify(topside) === JSON.stringify(originalTopside))) {
            dto.topsideDto.hasChanges = true
        }

        dto.explorationWellDto = explorationWells
        if (dto.explorationWellDto?.length > 0) {
            dto.explorationWellDto.forEach((expWellDto, index) => {
                if (expWellDto.drillingSchedule?.id !== EMPTY_GUID) {
                    const originalExpWell = originalExplorationWells
                        ?.find((oew) => oew.explorationId === expWellDto.explorationId
                            && oew.wellId === expWellDto.wellId)

                    if (!(JSON.stringify(expWellDto) === JSON.stringify(originalExpWell))) {
                        dto.explorationWellDto![index].hasChanges = true
                    }
                }
            })
        }

        dto.wellProjectWellDtos = wellProjectWells
        if (dto.wellProjectWellDtos?.length > 0) {
            dto.wellProjectWellDtos.forEach((wpWellDto, index: number) => {
                if (wpWellDto.drillingSchedule?.id !== EMPTY_GUID) {
                    const originalWpWell = originalWellProjectWells
                        ?.find((owpw) => owpw.wellProjectId === wpWellDto.wellProjectId
                            && owpw.wellId === wpWellDto.wellId)

                    if (!(JSON.stringify(wpWellDto) === JSON.stringify(originalWpWell))) {
                        dto.wellProjectWellDtos![index].hasChanges = true
                    }
                }
            })
        }

        setIsSaving(true)
        setUpdateFromServer(true)
        try {
            const result = await (await GetCaseWithAssetsService()).update(dto)
            const projectResult = Project.fromJSON(result.projectDto!)
            setProject(projectResult)
            if (result.generatedProfilesDto?.studyCostProfileWrapperDto !== null && result.generatedProfilesDto?.studyCostProfileWrapperDto !== undefined) {
                setTotalFeasibilityAndConceptStudies(TotalFeasibilityAndConceptStudies.fromJSON(result.generatedProfilesDto.studyCostProfileWrapperDto.totalFeasibilityAndConceptStudiesDto))
                setTotalFEEDStudies(TotalFEEDStudies.fromJSON(result.generatedProfilesDto.studyCostProfileWrapperDto.totalFEEDStudiesDto))
            }
            if (result.generatedProfilesDto?.opexCostProfileWrapperDto !== null && result.generatedProfilesDto?.opexCostProfileWrapperDto !== undefined) {
                setOffshoreFacilitiesOperationsCostProfile(OffshoreFacilitiesOperationsCostProfile.fromJSON(result.generatedProfilesDto.opexCostProfileWrapperDto?.offshoreFacilitiesOperationsCostProfileDto))
                setWellInterventionCostProfile(WellInterventionCostProfile.fromJSON(result.generatedProfilesDto.opexCostProfileWrapperDto?.wellInterventionCostProfileDto))
            }
            if (result.generatedProfilesDto?.cessationCostWrapperDto !== null && result.generatedProfilesDto?.cessationCostWrapperDto !== undefined) {
                setCessationWellsCost(CessationWellsCost.fromJSON(result.generatedProfilesDto.cessationCostWrapperDto.cessationWellsCostDto))
                setCessationOffshoreFacilitiesCost(CessationOffshoreFacilitiesCost.fromJSON(result.generatedProfilesDto.cessationCostWrapperDto.cessationOffshoreFacilitiesCostDto))
            }
            if (result.generatedProfilesDto?.gAndGAdminCostDto !== null && result.generatedProfilesDto?.gAndGAdminCostDto !== undefined) {
                setGAndGAdminCost(GAndGAdminCost.fromJSON(result.generatedProfilesDto.gAndGAdminCostDto))
            }
            if (result.generatedProfilesDto?.co2EmissionsDto !== null && result.generatedProfilesDto?.co2EmissionsDto !== undefined) {
                setCo2Emissions(Co2Emissions.fromJson(result.generatedProfilesDto.co2EmissionsDto))
            }
            if (result.generatedProfilesDto?.fuelFlaringAndLossesDto !== null && result.generatedProfilesDto?.fuelFlaringAndLossesDto !== undefined) {
                setFuelFlaringAndLosses(FuelFlaringAndLosses.fromJson(result.generatedProfilesDto.fuelFlaringAndLossesDto))
            }
            if (result.generatedProfilesDto?.netSalesGasDto !== null && result.generatedProfilesDto?.netSalesGasDto !== undefined) {
                setNetSalesGas(NetSalesGas.fromJson(result.generatedProfilesDto.netSalesGasDto))
            }
            if (result.generatedProfilesDto?.importedElectricityDto !== null && result.generatedProfilesDto?.importedElectricityDto !== undefined) {
                setImportedElectricity(ImportedElectricity.fromJson(result.generatedProfilesDto.importedElectricityDto))
            }
            setIsSaving(false)
        } catch (e) {
            setIsSaving(false)
            console.error("Error when saving case and assets: ", e)
        }
    }

    const withReferenceCase = () => {
        if (project.referenceCaseId === caseItem.id) {
            return bookmark_filled
        }
        return undefined
    }

    return (
        <div>
            <HeaderWrapper>
                <RowWrapper>
                    <PageTitle variant="h4">
                        {project.referenceCaseId === caseItem.id && (
                            <Tooltip title="Reference case">
                                <MenuIcon data={withReferenceCase()} size={18} />
                            </Tooltip>
                        )}
                        {caseItem.name}
                    </PageTitle>
                    <ColumnWrapper>
                        <CaseButtonsWrapper>
                            {!isSaving ? <Button onClick={handleSave}>Save</Button> : (
                                <Button>
                                    <Progress.Dots />
                                </Button>
                            )}
                            <TransparentButton
                                onClick={() => toggleTechnicalInputModal()}
                                variant="outlined"
                            >
                                Edit technical input
                            </TransparentButton>
                            <Button
                                variant="ghost"
                                aria-label="case menu"
                                ref={setMenuAnchorEl}
                                onClick={() => (isMenuOpen ? setIsMenuOpen(false) : setIsMenuOpen(true))}
                            >
                                <Icon data={more_vertical} />
                            </Button>
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
                    {project.referenceCaseId === caseItem.id
                        ? (
                            <Menu.Item
                                onClick={setCaseAsReference}
                            >
                                <Icon data={bookmark_outlined} size={16} />
                                <Typography group="navigation" variant="menu_title" as="span">
                                    Remove as reference case
                                </Typography>
                            </Menu.Item>
                        )
                        : (
                            <Menu.Item
                                onClick={setCaseAsReference}
                            >
                                <Icon data={bookmark_filled} size={16} />
                                <Typography group="navigation" variant="menu_title" as="span">
                                    Set as reference case
                                </Typography>
                            </Menu.Item>
                        )}
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
                                    activeTab={activeTab}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseProductionProfilesTab
                                    project={project}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    drainageStrategy={drainageStrategy}
                                    setDrainageStrategy={setDrainageStrategy}
                                    activeTab={activeTab}
                                    fuelFlaringAndLosses={fuelFlaringAndLosses}
                                    setFuelFlaringAndLosses={setFuelFlaringAndLosses}
                                    netSalesGas={netSalesGas}
                                    setNetSalesGas={setNetSalesGas}
                                    importedElectricity={importedElectricity}
                                    setImportedElectricity={setImportedElectricity}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseScheduleTab
                                    project={project}
                                    setProject={setProject}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    activeTab={activeTab}
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
                                    activeTab={activeTab}
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
                                    activeTab={activeTab}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseCostTab
                                    project={project}
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
                                    activeTab={activeTab}
                                    totalFeasibilityAndConceptStudies={totalFeasibilityAndConceptStudies}
                                    setTotalFeasibilityAndConceptStudies={setTotalFeasibilityAndConceptStudies}
                                    totalFEEDStudies={totalFEEDStudies}
                                    setTotalFEEDStudies={setTotalFEEDStudies}
                                    offshoreFacilitiesOperationsCostProfile={offshoreFacilitiesOperationsCostProfile}
                                    setOffshoreFacilitiesOperationsCostProfile={setOffshoreFacilitiesOperationsCostProfile}
                                    wellInterventionCostProfile={wellInterventionCostProfile}
                                    setWellInterventionCostProfile={setWellInterventionCostProfile}
                                    cessationWellsCost={cessationWellsCost}
                                    setCessationWellsCost={setCessationWellsCost}
                                    cessationOffshoreFacilitiesCost={cessationOffshoreFacilitiesCost}
                                    setCessationOffshoreFacilitiesCost={setCessationOffshoreFacilitiesCost}
                                    gAndGAdminCost={gAndGAdminCost}
                                    setGAndGAdminCost={setGAndGAdminCost}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseCO2Tab
                                    project={project}
                                    caseItem={caseItem}
                                    activeTab={activeTab}
                                    topside={topside}
                                    setTopside={setTopside}
                                    drainageStrategy={drainageStrategy}
                                    setDrainageStrategy={setDrainageStrategy}
                                    co2Emissions={co2Emissions}
                                    setCo2Emissions={setCo2Emissions}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                <CaseSummaryTab
                                    project={project}
                                    caseItem={caseItem}
                                    setCase={setCase}
                                    topside={topside}
                                    surf={surf}
                                    substructure={substructure}
                                    transport={transport}
                                    activeTab={activeTab}
                                />
                            </StyledTabPanel>
                        </Panels>
                    </TabContentWrapper>
                </Tabs>
                <DividerLine />
            </CaseViewDiv>
            <EditTechnicalInputModal
                toggleEditTechnicalInputModal={toggleTechnicalInputModal}
                isOpen={editTechnicalInputModalIsOpen}
                project={project}
                setProject={setProject}
                setWells={setWells}
                caseId={caseItem.id}
                setExploration={setExploration}
                setWellProject={setWellProject}
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
