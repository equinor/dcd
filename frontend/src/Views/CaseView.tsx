import {
    Button, Icon, Menu, Progress, Tabs, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useLocation, useParams } from "react-router-dom"
import { useModuleCurrentContext } from "@equinor/fusion-framework-react-module-context"
import { useNavigate } from 'react-router-dom'
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
import EditTechnicalInputModal from "../Components/EditTechnicalInput/EditTechnicalInputModal"
import CaseCostTab from "../Components/Case/Tabs/CaseCostTab"
import CaseFacilitiesTab from "../Components/Case/Tabs/CaseFacilitiesTab"
import CaseProductionProfilesTab from "../Components/Case/Tabs/CaseProductionProfilesTab"
import CaseScheduleTab from "../Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "../Components/Case/Tabs/CaseSummaryTab"
import CaseDrillingScheduleTab from "../Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseCO2Tab from "../Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import { GetCaseWithAssetsService } from "../Services/CaseWithAssetsService"
import { useAppContext } from "../Context/AppContext"


const { Panel } = Tabs
const { List, Tab, Panels } = Tabs

const CaseViewDiv = styled.div`
    display: flex;
    flex-direction: column;
`

const PageTitle = styled(Typography)`
    flex-grow: 1;
    padding-left: 10px;
`

const DividerLine = styled.div`
`
const StyledTabPanel = styled(Panel)`
    padding-top: 5px;
    margin: 20px;
`
const HeaderWrapper = styled.div`
    background-color: white;
    width: calc(100% - 270px);
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
    margin: 0 0 78px 20px;
    align-items: center;
`
const MenuIcon = styled(Icon)`
    color: ${tokens.colors.text.static_icons__secondary.rgba};
    margin-right: 0.5rem;
    margin-bottom: -0.2rem;
`

const CaseView = () => {
    const {
        project, setProject,
        caseItem, setCase,
        topside, setTopside,
        topsideCost, setTopsideCost,
        surf, setSurf, 
        surfCost, setSurfCost, 
        substructure, setSubstructure, 
        substructureCost, setSubstructureCost, 
        transport, setTransport,
        transportCost, setTransportCost,
        opexSum, setOpexSum,
        cessationOffshoreFacilitiesCost, setCessationOffshoreFacilitiesCost,
        feasibilityAndConceptStudiesCost, setFeasibilityAndConceptStudiesCost,
        feedStudiesCost, setFEEDStudiesCost, 
        activeTab, setActiveTab, 
        explorationCost, setExplorationCost, 
        drillingCost, setDrillingCost,
        totalStudyCost, setTotalStudyCost,
        productionAndSalesVolume,setProductionAndSalesVolume,
        oilCondensateProduction, setOilCondensateProduction, 
        nglProduction, setNGLProduction, 
        NetSalesGas, setNetSalesGas,
        cO2Emissions, setCO2Emissions, 
        importedElectricity, setImportedElectricity, 
        setStartYear,
        setEndYear,
        tableYears, setTableYears
    } = useAppContext();

    const [editTechnicalInputModalIsOpen, setEditTechnicalInputModalIsOpen] = useState<boolean>(false)

    const { fusionContextId, caseId } = useParams<Record<string, string | undefined>>()
    const { currentContext } = useModuleCurrentContext()
    const [drainageStrategy, setDrainageStrategy] = useState<Components.Schemas.DrainageStrategyDto>()
    const [exploration, setExploration] = useState<Components.Schemas.ExplorationDto>()
    const [wellProject, setWellProject] = useState<Components.Schemas.WellProjectDto>()

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

    const [gAndGAdminCost, setGAndGAdminCost] = useState<Components.Schemas.GAndGAdminCostDto>()


    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<Components.Schemas.FuelFlaringAndLossesDto>()

    const [isMenuOpen, setIsMenuOpen] = useState<boolean>(false)
    const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLButtonElement | null>(null)

    const toggleTechnicalInputModal = () => setEditTechnicalInputModalIsOpen(!editTechnicalInputModalIsOpen)

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
            if (result.generatedProfilesDto?.studyCostProfileWrapperDto !== null && result.generatedProfilesDto?.studyCostProfileWrapperDto !== undefined) {
                setFeasibilityAndConceptStudiesCost(result.generatedProfilesDto.studyCostProfileWrapperDto.totalFeasibilityAndConceptStudiesDto)
                setFEEDStudiesCost(result.generatedProfilesDto.studyCostProfileWrapperDto.totalFEEDStudiesDto)
            }
            if (result.generatedProfilesDto?.opexCostProfileWrapperDto !== null && result.generatedProfilesDto?.opexCostProfileWrapperDto !== undefined) {
                setOffshoreFacilitiesOperationsCostProfile(result.generatedProfilesDto.opexCostProfileWrapperDto?.offshoreFacilitiesOperationsCostProfileDto)
                setWellInterventionCostProfile(result.generatedProfilesDto.opexCostProfileWrapperDto?.wellInterventionCostProfileDto)
            }
            if (result.generatedProfilesDto?.cessationCostWrapperDto !== null && result.generatedProfilesDto?.cessationCostWrapperDto !== undefined) {
                setCessationWellsCost(result.generatedProfilesDto.cessationCostWrapperDto.cessationWellsCostDto)
                setCessationOffshoreFacilitiesCost(result.generatedProfilesDto.cessationCostWrapperDto.cessationOffshoreFacilitiesCostDto)
            }
            if (result.generatedProfilesDto?.gAndGAdminCostDto !== null && result.generatedProfilesDto?.gAndGAdminCostDto !== undefined) {
                setGAndGAdminCost(result.generatedProfilesDto.gAndGAdminCostDto)
            }
            if (result.generatedProfilesDto?.co2EmissionsDto !== null && result.generatedProfilesDto?.co2EmissionsDto !== undefined) {
                setCO2Emissions(result.generatedProfilesDto.co2EmissionsDto)
            }
            if (result.generatedProfilesDto?.fuelFlaringAndLossesDto !== null && result.generatedProfilesDto?.fuelFlaringAndLossesDto !== undefined) {
                setFuelFlaringAndLosses(result.generatedProfilesDto.fuelFlaringAndLossesDto)
            }
            if (result.generatedProfilesDto?.netSalesGasDto !== null && result.generatedProfilesDto?.netSalesGasDto !== undefined) {
                setNetSalesGas(result.generatedProfilesDto.netSalesGasDto)
            }
            if (result.generatedProfilesDto?.importedElectricityDto !== null && result.generatedProfilesDto?.importedElectricityDto !== undefined) {
                setImportedElectricity(result.generatedProfilesDto.importedElectricityDto)
            }
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
            setIfNotNull(result.generatedProfilesDto?.co2EmissionsDto, setCO2Emissions)
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
        <div>
            <HeaderWrapper>
                <RowWrapper>
                    <Button
                        onClick={() => navigate(projectPath(currentContext?.externalId!))}
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
                                onClick={() => toggleTechnicalInputModal()}
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
                                    netSalesGas={NetSalesGas}
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
                                    exploration={exploration}
                                    wellProject={wellProject}
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
                                    totalFeasibilityAndConceptStudies={feasibilityAndConceptStudiesCost}
                                    setTotalFeasibilityAndConceptStudies={setFeasibilityAndConceptStudiesCost}
                                    totalFEEDStudies={feedStudiesCost}
                                    setTotalFEEDStudies={setFEEDStudiesCost}
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
                                    co2Emissions={cO2Emissions}
                                    setCo2Emissions={setCO2Emissions}
                                />
                            </StyledTabPanel>
                            <StyledTabPanel>
                                
                                <CaseSummaryTab
                                    // project={project}
                                    // caseItem={caseItem}
                                    // setCase={setCase}
                                    // topside={topside}
                                    // surf={surf}
                                    // substructure={substructure}
                                    // transport={transport}
                                    // activeTab={activeTab}
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
                caseId={caseItem.id}
                setExploration={setExploration}
                setWellProject={setWellProject}
            />
        </div>
    )
}

export default CaseView
