import { Tabs } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useLocation, useNavigate } from "react-router-dom"
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
import styled from "styled-components"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CasePanel = styled(Panel)`
    height: calc(100vh - 210px);
    overflow: auto;
`

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

        // CAPEX
        setCessationOffshoreFacilitiesCost,
        setCessationOnshoreFacilitiesCostProfile,

        // Study cost
        setTotalFeasibilityAndConceptStudies,
        setTotalFEEDStudies,
        setTotalOtherStudies,

        topside,
        setTopside,
        surf,
        setSurf,
        substructure,
        setSubstructure,
        transport,
        setTransport,
        drainageStrategy,
        setDrainageStrategy,

        wellProjectWells,
        setWellProjectWells,
        explorationWells,
        setExplorationWells,

        // OPEX
        setHistoricCostCostProfile,
        setWellInterventionCostProfile,
        setOffshoreFacilitiesOperationsCostProfile,
        setOnshoreRelatedOPEXCostProfile,
        setAdditionalOPEXCostProfile,

    } = useCaseContext()

    if (!projectCase || !project) {
        return (<>Loading...</>)
    }

    const {
        wellProject,
        setWellProject,
        exploration,
        setExploration,
    } = useModalContext()

    const [wells, setWells] = useState<Components.Schemas.WellDto[]>()

    const [cessationWellsCost, setCessationWellsCost] = useState<Components.Schemas.CessationWellsCostDto>()

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
            const result = await (await GetCaseWithAssetsService()).update(project.id, projectCase.id, dto)
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
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.onshoreRelatedOPEXCostProfileDto, setOnshoreRelatedOPEXCostProfile)
            setIfNotNull(result.generatedProfilesDto?.opexCostProfileWrapperDto?.additionalOPEXCostProfileDto, setAdditionalOPEXCostProfile)
            setIfNotNull(result.generatedProfilesDto?.cessationCostWrapperDto?.cessationWellsCostDto, setCessationWellsCost)
            setIfNotNull(result.generatedProfilesDto?.cessationCostWrapperDto?.cessationOffshoreFacilitiesCostDto, setCessationOffshoreFacilitiesCost)
            setIfNotNull(result.generatedProfilesDto?.cessationCostWrapperDto?.cessationOnshoreFacilitiesCostProfileDto, setCessationOnshoreFacilitiesCostProfile)
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

    if (isLoading) {
        return (<>Loading...</>)
    }

    if (!drainageStrategy
        || !exploration
        || !wellProject
        || !surf
        || !topside
        || !substructure
        || !transport
        || !explorationWells
        || !wellProjectWells) {
        return (<>One or more assets missing</>)
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
                        <CasePanel>
                            <CaseDescriptionTab />
                        </CasePanel>
                        <CasePanel>
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
                        </CasePanel>
                        <CasePanel>
                            <CaseScheduleTab />
                        </CasePanel>
                        <CasePanel>
                            <CaseDrillingScheduleTab
                                explorationWells={explorationWells}
                                setExplorationWells={setExplorationWells}
                                wellProjectWells={wellProjectWells}
                                setWellProjectWells={setWellProjectWells}
                                wells={wells}
                                exploration={exploration}
                                wellProject={wellProject}
                            />
                        </CasePanel>
                        <CasePanel>
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
                        </CasePanel>
                        <Panel>
                            <CaseCostTab />
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
                        <CasePanel>
                            <CaseSummaryTab />
                        </CasePanel>
                    </Panels>
                </Tabs>
            </Grid>
        </Grid>
    )
}

export default CaseView
