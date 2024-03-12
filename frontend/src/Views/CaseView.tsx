import { Tabs } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import CaseDescriptionTab from "../Components/Case/Tabs/CaseDescriptionTab"
import CaseCostTab from "../Components/Case/Tabs/CaseCostTab"
import CaseFacilitiesTab from "../Components/Case/Tabs/CaseFacilitiesTab"
import CaseProductionProfilesTab from "../Components/Case/Tabs/CaseProductionProfilesTab"
import CaseScheduleTab from "../Components/Case/Tabs/CaseScheduleTab"
import CaseSummaryTab from "../Components/Case/Tabs/CaseSummaryTab"
import CaseDrillingScheduleTab from "../Components/Case/Tabs/CaseDrillingSchedule/CaseDrillingScheduleTab"
import CaseCO2Tab from "../Components/Case/Tabs/Co2Emissions/CaseCO2Tab"
import { GetCaseWithAssetsService } from "../Services/CaseWithAssetsService"
import { useProjectContext } from "../Context/ProjectContext"
import { useModalContext } from "../Context/ModalContext"
import { useCaseContext } from "../Context/CaseContext"
import { useAppContext } from "../Context/AppContext"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CaseView = () => {
    const {
        setIsSaving, isLoading, setIsLoading, updateFromServer, setUpdateFromServer,
    } = useAppContext()

    const {
        project,
        setProject,
    } = useProjectContext()

    const {
        projectCase,
        setProjectCase,
        saveProjectCase,
        setSaveProjectCase,
        activeTabCase,
        setActiveTabCase,

        // Study cost
        totalFeasibilityAndConceptStudies,
        setTotalFeasibilityAndConceptStudies,
        totalFeasibilityAndConceptStudiesOverride,
        setTotalFeasibilityAndConceptStudiesOverride,
        totalFEEDStudies,
        setTotalFEEDStudies,
        totalFEEDStudiesOverride,
        setTotalFEEDStudiesOverride,
        totalOtherStudies,
        setTotalOtherStudies,

        topside, setTopside,
        topsideCost, setTopsideCost,
        surf, setSurf,
        surfCost, setSurfCost,
        substructure, setSubstructure,
        substructureCost, setSubstructureCost,
        transport, setTransport,
        transportCost, setTransportCost,
        drainageStrategy, setDrainageStrategy,

        wellProjectWells, setWellProjectWells,
        explorationWells, setExplorationWells,

        // Exploration
        totalExplorationCost,
        setTotalExplorationCost,
        explorationWellCostProfile,
        setExplorationWellCostProfile,
        gAndGAdminCost,
        setGAndGAdminCost,
        seismicAcquisitionAndProcessing,
        setSeismicAcquisitionAndProcessing,
        explorationSidetrackCost,
        setExplorationSidetrackCost,
        explorationAppraisalWellCost,
        setExplorationAppraisalWellCost,
        countryOfficeCost,
        setCountryOfficeCost,
    } = useCaseContext()

    if (!projectCase) return (null)

    const {
        wellProject,
        setWellProject,
        exploration,
        setExploration,
    } = useModalContext()

    const [wells, setWells] = useState<Components.Schemas.WellDto[]>()

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

    const [co2Emissions, setCo2Emissions] = useState<Components.Schemas.Co2EmissionsDto>()

    const [netSalesGas, setNetSalesGas] = useState<Components.Schemas.NetSalesGasDto>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<Components.Schemas.FuelFlaringAndLossesDto>()
    const [importedElectricity, setImportedElectricity] = useState<Components.Schemas.ImportedElectricityDto>()

    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        if (project && updateFromServer) {
            const caseResult = project.cases.find((o) => o.id === projectCase?.id)
            if (!caseResult) {
                if (location.pathname.indexOf("/case") > -1) {
                    const projectUrl = location.pathname.split("/case")[0]
                    navigate(projectUrl)
                }
            }

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
            const surfResult = project.surfs.find((sur) => sur.id === projectCase?.surfLink)
            setSurf(surfResult)

            const topsideResult = project.topsides.find((top) => top.id === projectCase?.topsideLink)
            setTopside(topsideResult)

            const substructureResult = project.substructures.find((sub) => sub.id === projectCase?.substructureLink)
            setSubstructure(substructureResult)

            const transportResult = project.transports.find((tran) => tran.id === projectCase?.transportLink)
            setTransport(transportResult)

            setWells(project.wells)
        }
    }, [project])

    const handleCaseSave = async () => {
        const dto: Components.Schemas.CaseWithAssetsWrapperDto = {
            caseDto: projectCase,
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
            const result = await (await GetCaseWithAssetsService()).update(project?.id!, projectCase?.id!, dto)
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

            setProjectCase(result.projectDto.cases.find((c) => c.name === dto.caseDto?.name))

            setIsSaving(false)
        } catch (e) {
            setIsSaving(false)
            console.error("Error when saving case and assets: ", e)
        }
        setSaveProjectCase(false)
    }

    useEffect(() => {
        saveProjectCase && handleCaseSave()
    }, [saveProjectCase])

    if (isLoading
        || !project
        || !projectCase
        || !drainageStrategy
        || !exploration
        || !wellProject
        || !surf
        || !topside
        || !substructure
        || !transport
        || !explorationWells
        || !wellProjectWells) {
        return (<></>)
    }

    return (
        <Grid container spacing={1} alignSelf="flex-start">
            <Grid item xs={12}>
                <Tabs activeTab={activeTabCase} onChange={setActiveTabCase} scrollable>
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
                        <Panel>
                            <CaseDescriptionTab />
                        </Panel>
                        <Panel>
                            <CaseProductionProfilesTab
                                drainageStrategy={drainageStrategy}
                                setDrainageStrategy={setDrainageStrategy}
                                fuelFlaringAndLosses={fuelFlaringAndLosses}
                                setFuelFlaringAndLosses={setFuelFlaringAndLosses}
                                netSalesGas={netSalesGas}
                                setNetSalesGas={setNetSalesGas}
                                importedElectricity={importedElectricity}
                                setImportedElectricity={setImportedElectricity}
                            />
                        </Panel>
                        <Panel>
                            <CaseScheduleTab />
                        </Panel>
                        <Panel>
                            <CaseDrillingScheduleTab
                                explorationWells={explorationWells}
                                setExplorationWells={setExplorationWells}
                                wellProjectWells={wellProjectWells}
                                setWellProjectWells={setWellProjectWells}
                                wells={wells}
                                exploration={exploration}
                                wellProject={wellProject}
                            />
                        </Panel>
                        <Panel>
                            <CaseFacilitiesTab
                                topside={topside}
                                setTopside={setTopside}
                                surf={surf}
                                setSurf={setSurf}
                                substructure={substructure}
                                setSubstrucutre={setSubstructure}
                                transport={transport}
                                setTransport={setTransport}
                            />
                        </Panel>
                        <Panel>
                            <CaseCostTab
                                wellProject={wellProject}
                                setWellProject={setWellProject}
                                offshoreFacilitiesOperationsCostProfile={offshoreFacilitiesOperationsCostProfile}
                                setOffshoreFacilitiesOperationsCostProfile={setOffshoreFacilitiesOperationsCostProfile}
                                wellInterventionCostProfile={wellInterventionCostProfile}
                                setWellInterventionCostProfile={setWellInterventionCostProfile}
                                additionalOPEXCostProfile={additionalOPEXCostProfile}
                                cessationWellsCost={cessationWellsCost}
                                setCessationWellsCost={setCessationWellsCost}
                                cessationOffshoreFacilitiesCost={cessationOffshoreFacilitiesCost}
                                setCessationOffshoreFacilitiesCost={setCessationOffshoreFacilitiesCost}
                            />
                        </Panel>
                        <Panel>
                            <CaseCO2Tab
                                topside={topside}
                                setTopside={setTopside}
                                drainageStrategy={drainageStrategy}
                                setDrainageStrategy={setDrainageStrategy}
                                co2Emissions={co2Emissions}
                                setCo2Emissions={setCo2Emissions}
                            />
                        </Panel>
                        <Panel>
                            <CaseSummaryTab />
                        </Panel>
                    </Panels>
                </Tabs>
            </Grid>
        </Grid>
    )
}

export default CaseView
