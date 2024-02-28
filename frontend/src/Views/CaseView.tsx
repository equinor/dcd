import {
    Button, Icon, Progress, Tabs, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import styled from "styled-components"
import {
    bookmark_filled,

    more_vertical,
    arrow_back,
} from "@equinor/eds-icons"
import { tokens } from "@equinor/eds-tokens"
import { Tooltip } from "@mui/material"
import { projectPath, unwrapProjectId } from "../Utils/common"
import CaseDropMenu from "../Components/Case/Components/CaseDropMenu"
import { GetProjectService } from "../Services/ProjectService"
import CaseDescriptionTab from "../Components/Case/Tabs/CaseDescriptionTab"
import CaseCostTab from "../Components/Case/Tabs/CaseCostTab"
import CaseFacilitiesTab from "../Components/Case/Tabs/CaseFacilitiesTab"
import CaseProductionProfilesTab from "../Components/Case/Tabs/CaseProductionProfilesTab"
import CaseScheduleTab from "../Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "../Components/Case/Tabs/CaseSummaryTab"
import CaseDrillingScheduleTab from "../Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseCO2Tab from "../Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import { GetCaseWithAssetsService } from "../Services/CaseWithAssetsService"
import { useAppContext } from "../Context/AppContext"
import { useModalContext } from "../Context/ModalContext"

const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const Wrapper = styled.div`
    flex: 1;
    display: flex;
    flex-direction: column;
`

const StyledList = styled(List)`
    border-bottom: 1px solid LightGray;
   
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
    padding-left: 10px;
`
const StyledTabs = styled(Tabs)`
    flex: 1;
    display: flex;
    flex-direction: column;
`
const StyledTabPanel = styled(Panel)`
    margin: 20px;
`
const HeaderWrapper = styled.div`
    padding-top: 0px;
    border-top: 1px solid LightGray;
`

const TabContentWrapper = styled(Panels)`
    flex: 1;
    overflow: hidden;
`
const CaseButtonsWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    margin-left: auto;
    gap: 10px;
`

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const MoreButton = styled(Button)`
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin: 20px;
    
`
const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};

`

const CaseView = () => {
    const {
        project,
        setProject,
    } = useAppContext()

    const {
        wellProject,
        setWellProject,
        exploration,
        setExploration,
        setTechnicalModalIsOpen,
    } = useModalContext()
    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const { currentContext } = useModuleCurrentContext()

    const [caseItem, setCase] = useState<Components.Schemas.CaseDto>()
    const [activeTab, setActiveTab] = useState<number>(0)

    const [drainageStrategy, setDrainageStrategy] = useState<Components.Schemas.DrainageStrategyDto>()
    const [surf, setSurf] = useState<Components.Schemas.SurfDto>()
    const [topside, setTopside] = useState<Components.Schemas.TopsideDto>()
    const [substructure, setSubstructure] = useState<Components.Schemas.SubstructureDto>()
    const [transport, setTransport] = useState<Components.Schemas.TransportDto>()

    const [wells, setWells] = useState<Components.Schemas.WellDto[]>()
    const [wellProjectWells, setWellProjectWells] = useState<Components.Schemas.WellProjectWellDto[]>()
    const [explorationWells, setExplorationWells] = useState<Components.Schemas.ExplorationWellDto[]>()

    const [totalFeasibilityAndConceptStudies,
        setTotalFeasibilityAndConceptStudies] = useState<Components.Schemas.TotalFeasibilityAndConceptStudiesDto>()

    const [totalFEEDStudies, setTotalFEEDStudies] = useState<Components.Schemas.TotalFEEDStudiesDto>()

    const [totalOtherStudies, setTotalOtherStudies] = useState<Components.Schemas.TotalOtherStudiesDto>()

    const [offshoreFacilitiesOperationsCostProfile,
        setOffshoreFacilitiesOperationsCostProfile] = useState<Components.Schemas.OffshoreFacilitiesOperationsCostProfileDto>()

    const [wellInterventionCostProfile, setWellInterventionCostProfile] = useState<Components.Schemas.WellInterventionCostProfileDto>()

    const [historicCostCostProfile,
        setHistoricCostCostProfile] = useState<Components.Schemas.HistoricCostCostProfileDto>()

    const [additionalOPEXCostProfile,
        setAdditionalOPEXCostProfile] = useState<Components.Schemas.AdditionalOPEXCostProfileDto>()

    const [cessationWellsCost, setCessationWellsCost] = useState<Components.Schemas.CessationWellsCostDto>()
    const [cessationOffshoreFacilitiesCost,
        setCessationOffshoreFacilitiesCost] = useState<Components.Schemas.CessationOffshoreFacilitiesCostDto>()

    const [gAndGAdminCost, setGAndGAdminCost] = useState<Components.Schemas.GAndGAdminCostDto>()

    const [co2Emissions, setCo2Emissions] = useState<Components.Schemas.Co2EmissionsDto>()

    const [netSalesGas, setNetSalesGas] = useState<Components.Schemas.NetSalesGasDto>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<Components.Schemas.FuelFlaringAndLossesDto>()
    const [importedElectricity, setImportedElectricity] = useState<Components.Schemas.ImportedElectricityDto>()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const navigate = useNavigate()
    const location = useLocation()

    const [isLoading, setIsLoading] = useState<boolean>()
    const [isSaving, setIsSaving] = useState<boolean>()
    const [updateFromServer, setUpdateFromServer] = useState<boolean>(true)

    useEffect(() => {
        (async () => {
            try {
                setUpdateFromServer(true)
                setIsLoading(true)
                const projectId = unwrapProjectId(currentContext?.externalId)
                const projectResult = await (await GetProjectService()).getProject(projectId)
                setProject(projectResult) // should we be setting project here?
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${currentContext?.externalId}`, error)
            }
        })()
    }, [currentContext?.externalId, caseId, fusionContextId])

    useEffect(() => {
        if (project && updateFromServer) {
            const caseResult = project.cases.find((o) => o.id === caseId)
            if (!caseResult) {
                if (location.pathname.indexOf("/case") > -1) {
                    const projectUrl = location.pathname.split("/case")[0]
                    navigate(projectUrl)
                }
            }
            setCase(caseResult)

            const drainageStrategyResult = project?.drainageStrategies
                .find((drain) => drain.id === caseResult?.drainageStrategyLink)
            setDrainageStrategy(
                drainageStrategyResult,
            )

            const explorationResult = project
                ?.explorations.find((exp) => exp.id === caseResult?.explorationLink)
            setExploration(explorationResult)

            const wellProjectResult = project
                ?.wellProjects.find((wp) => wp.id === caseResult?.wellProjectLink)
            setWellProject(wellProjectResult)

            const surfResult = project?.surfs.find((sur) => sur.id === caseResult?.surfLink)
            setSurf(surfResult)

            const topsideResult = project?.topsides.find((top) => top.id === caseResult?.topsideLink)
            setTopside(topsideResult)

            const substructureResult = project?.substructures.find((sub) => sub.id === caseResult?.substructureLink)
            setSubstructure(substructureResult)

            const transportResult = project?.transports.find((tran) => tran.id === caseResult?.transportLink)
            setTransport(transportResult)

            setWells(project.wells)

            setWellProjectWells(wellProjectResult?.wellProjectWells ?? [])

            setExplorationWells(explorationResult?.explorationWells ?? [])

            setUpdateFromServer(false)
            setIsLoading(false)
        } else if (project) {
            const caseResult = project.cases.find((o) => o.id === caseId)

            const surfResult = project.surfs.find((sur) => sur.id === caseResult?.surfLink)
            setSurf(surfResult)

            const topsideResult = project.topsides.find((top) => top.id === caseResult?.topsideLink)
            setTopside(topsideResult)

            const substructureResult = project.substructures.find((sub) => sub.id === caseResult?.substructureLink)
            setSubstructure(substructureResult)

            const transportResult = project.transports.find((tran) => tran.id === caseResult?.transportLink)
            setTransport(transportResult)

            setWells(project.wells)
        }
    }, [project])

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

    const handleSave = async () => {
        const dto: Components.Schemas.CaseWithAssetsWrapperDto = {
            caseDto: caseItem,
            drainageStrategyDto: drainageStrategy,
            wellProjectDto: wellProject,
            explorationDto: exploration,
            surfDto: surf,
            substructureDto: substructure,
            transportDto: transport,
            topsideDto: topside,
            explorationWellDto: explorationWells,
            wellProjectWellDtos: wellProjectWells,
        }

        setIsSaving(true)
        setUpdateFromServer(true)

        try {
            const result = await (await GetCaseWithAssetsService()).update(project.id, caseId!, dto)
            const projectResult = { ...result.projectDto }
            setProject(projectResult)

            const setIfNotNull = (data: any, setState: any) => {
                if (data !== null && data !== undefined) { setState(data) }
            }

            setIfNotNull(result.generatedProfilesDto?.studyCostProfileWrapperDto?.totalFeasibilityAndConceptStudiesDto, setTotalFeasibilityAndConceptStudies)
            setIfNotNull(result.generatedProfilesDto?.studyCostProfileWrapperDto?.totalFEEDStudiesDto, setTotalFEEDStudies)
            setIfNotNull(result.generatedProfilesDto?.studyCostProfileWrapperDto?.totalOtherStudiesDto, setTotalOtherStudies)
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.offshoreFacilitiesOperationsCostProfileDto, setOffshoreFacilitiesOperationsCostProfile)
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.wellInterventionCostProfileDto, setWellInterventionCostProfile)
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.historicCostCostProfileDto, setHistoricCostCostProfile)
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.additionalOPEXCostProfileDto, setAdditionalOPEXCostProfile)
            setIfNotNull(result.generatedProfilesDto?.cessationCostWrapperDto?.cessationWellsCostDto, setCessationWellsCost)
            setIfNotNull(result.generatedProfilesDto?.cessationCostWrapperDto?.cessationOffshoreFacilitiesCostDto, setCessationOffshoreFacilitiesCost)
            setIfNotNull(result.generatedProfilesDto?.gAndGAdminCostDto, setGAndGAdminCost)
            setIfNotNull(result.generatedProfilesDto?.co2EmissionsDto, setCo2Emissions)
            setIfNotNull(result.generatedProfilesDto?.fuelFlaringAndLossesDto, setFuelFlaringAndLosses)
            setIfNotNull(result.generatedProfilesDto?.netSalesGasDto, setNetSalesGas)
            setIfNotNull(result.generatedProfilesDto?.importedElectricityDto, setImportedElectricity)

            setIsSaving(false)
        } catch (e) {
            setIsSaving(false)
            console.error("Error when saving case and assets: ", e)
        }
    }

    const withReferenceCase = () => {
        if (project.referenceCaseId === caseItem.id) return bookmark_filled

        return undefined
    }

    return (
        <Wrapper>
            <HeaderWrapper>
                <RowWrapper>
                    <Button
                        onClick={() => navigate(projectPath(currentContext?.id!))}
                        variant="ghost_icon"
                    >
                        <Icon data={arrow_back} />
                    </Button>
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
                            {!isSaving ? <Button onClick={handleSave}>Save case</Button> : (
                                <Button>
                                    <Progress.Dots />
                                </Button>
                            )}
                            <Button
                                onClick={() => setTechnicalModalIsOpen(true)}
                                variant="outlined"
                            >
                                Edit technical input
                            </Button>
                            <MoreButton
                                variant="ghost_icon"
                                aria-label="case menu"
                                ref={setMenuAnchorEl}
                                onClick={() => (isMenuOpen ? setIsMenuOpen(false) : setIsMenuOpen(true))}
                            >
                                <Icon data={more_vertical} />
                            </MoreButton>
                        </CaseButtonsWrapper>
                    </ColumnWrapper>
                </RowWrapper>
                <CaseDropMenu
                    isMenuOpen={isMenuOpen}
                    setIsMenuOpen={setIsMenuOpen}
                    menuAnchorEl={menuAnchorEl}
                    caseItem={caseItem}
                />
            </HeaderWrapper>
            <StyledTabs activeTab={activeTab} onChange={setActiveTab} scrollable>
                <StyledList>
                    <Tab>Description</Tab>
                    <Tab>Production Profiles</Tab>
                    <Tab>Schedule</Tab>
                    <Tab>Drilling Schedule</Tab>
                    <Tab>Facilities</Tab>
                    <Tab>Cost</Tab>
                    <Tab>CO2 Emissions</Tab>
                    <Tab>Summary</Tab>
                </StyledList>
                <TabContentWrapper>
                    <StyledTabPanel>
                        <CaseDescriptionTab
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
                            caseItem={{ ...caseItem }}
                            setCase={setCase}
                            activeTab={activeTab}
                        />
                    </StyledTabPanel>
                    <StyledTabPanel>
                        <CaseDrillingScheduleTab
                            project={project}
                            caseItem={caseItem}
                            explorationWells={explorationWells}
                            setExplorationWells={setExplorationWells}
                            wellProjectWells={wellProjectWells}
                            setWellProjectWells={setWellProjectWells}
                            wells={wells}
                            activeTab={activeTab}
                            exploration={exploration}
                            wellProject={wellProject}
                        />
                    </StyledTabPanel>
                    <StyledTabPanel>
                        <CaseFacilitiesTab
                            project={project}
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
                            exploration={exploration}
                            setExploration={setExploration}
                            wellProject={wellProject}
                            setWellProject={setWellProject}
                            caseItem={caseItem}
                            setCase={setCase}
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
                            totalOtherStudies={totalOtherStudies}
                            offshoreFacilitiesOperationsCostProfile={offshoreFacilitiesOperationsCostProfile}
                            setOffshoreFacilitiesOperationsCostProfile={setOffshoreFacilitiesOperationsCostProfile}
                            wellInterventionCostProfile={wellInterventionCostProfile}
                            setWellInterventionCostProfile={setWellInterventionCostProfile}
                            historicCostCostProfile={historicCostCostProfile}
                            additionalOPEXCostProfile={additionalOPEXCostProfile}
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
                </TabContentWrapper>
            </StyledTabs>
        </Wrapper>
    )
}

export default CaseView
