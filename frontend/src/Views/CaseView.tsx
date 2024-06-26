import { Tabs } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useLocation, useNavigate, useParams } from "react-router-dom"
import Grid from "@mui/material/Grid"
import styled from "styled-components"
import CaseDescriptionTab from "../Components/Case/Tabs/CaseDescriptionTab"
import CaseCostTab from "../Components/Case/Tabs/CaseCost/CaseCostTab"
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
import { mergeTimeseriesList } from "../Utils/common"
import { ITimeSeries } from "../Models/ITimeSeries"

const {
    List, Tab, Panels, Panel,
} = Tabs

const CaseView = () => {
    const { caseId, tab } = useParams()

    const {
        setIsSaving,
        isLoading,
        setIsLoading,
        updateFromServer,
        setUpdateFromServer,
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

        setTotalDrillingCost,

        setSurfCost,
        setSubstructureCost,
        setTransportCost,

        setOffshoreFacilitiesCost,

        setTotalExplorationCost,

        setOffshoreOpexPlussWellIntervention,
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

    const [, setCessationWellsCost] = useState<Components.Schemas.CessationWellsCostDto>()

    const [co2Emissions, setCo2Emissions] = useState<Components.Schemas.Co2EmissionsDto>()

    const [netSalesGas, setNetSalesGas] = useState<Components.Schemas.NetSalesGasDto>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<Components.Schemas.FuelFlaringAndLossesDto>()
    const [importedElectricity, setImportedElectricity] = useState<Components.Schemas.ImportedElectricityDto>()

    const navigate = useNavigate()
    const location = useLocation()

    const tabNames = [
        "Description",
        "Production Profiles",
        "Schedule",
        "Drilling Schedule",
        "Facilities",
        "Cost",
        "CO2 Emissions",
        "Summary",
    ]

    // when the URL changes, set the active tab to the tab name in the URL
    useEffect(() => {
        if (tab) {
            const tabIndex = tabNames.indexOf(tab)
            if (activeTabCase !== tabIndex) {
                setActiveTabCase(tabIndex)
            }
        }
    }, [])

    // when user navigates to a different tab, add the tab name to the URL
    useEffect(() => {
        if (activeTabCase !== undefined) {
            const tabName = tabNames[activeTabCase]
            const projectUrl = location.pathname.split("/case")[0]
            navigate(`${projectUrl}/case/${caseId}/${tabName}`)
        }
    }, [activeTabCase])

    const handleOffshoreOpexPlussWellIntervention = () => {
        setOffshoreOpexPlussWellIntervention(
            mergeTimeseriesList([
                (projectCase.wellInterventionCostProfileOverride?.override === true
                    ? projectCase.wellInterventionCostProfileOverride
                    : projectCase.wellInterventionCostProfile),
                (projectCase.offshoreFacilitiesOperationsCostProfileOverride?.override === true
                    ? projectCase.offshoreFacilitiesOperationsCostProfileOverride
                    : projectCase.offshoreFacilitiesOperationsCostProfile),
            ]),
        )
    }

    const handleTotalExplorationCost = () => {
        if (exploration) {
            setTotalExplorationCost(mergeTimeseriesList([
                exploration.explorationWellCostProfile,
                exploration.appraisalWellCostProfile,
                exploration.sidetrackCostProfile,
                exploration.seismicAcquisitionAndProcessing,
                exploration.countryOfficeCost,
                // gAndGAdminCost // Missing implementation, uncomment when gAndGAdminCost is fixed
            ]))
        }
    }

    const handleOffshoreFacilitiesCost = () => {
        const surfCostProfile = surf?.costProfileOverride?.override
            ? surf.costProfileOverride : surf?.costProfile
        setSurfCost(surfCostProfile)

        const substructureCostProfile = substructure?.costProfileOverride?.override
            ? substructure.costProfileOverride : substructure?.costProfile
        setSubstructureCost(substructureCostProfile)

        const transportCostProfile = transport?.costProfileOverride?.override
            ? transport.costProfileOverride : transport?.costProfile
        setTransportCost(transportCostProfile)

        setOffshoreFacilitiesCost(mergeTimeseriesList([
            surfCostProfile,
            substructureCostProfile,
            transportCostProfile,
        ]))
    }

    const handleDrilling = () => {
        const oilProducerCostProfile = wellProject?.oilProducerCostProfileOverride?.override
            ? wellProject.oilProducerCostProfileOverride
            : wellProject?.oilProducerCostProfile

        const gasProducerCostProfile = wellProject?.gasProducerCostProfileOverride?.override
            ? wellProject.gasProducerCostProfileOverride
            : wellProject?.gasProducerCostProfile

        const waterInjectorCostProfile = wellProject?.waterInjectorCostProfileOverride?.override
            ? wellProject.waterInjectorCostProfileOverride
            : wellProject?.waterInjectorCostProfile

        const gasInjectorCostProfile = wellProject?.gasInjectorCostProfileOverride?.override
            ? wellProject.gasInjectorCostProfileOverride
            : wellProject?.gasInjectorCostProfile

        const startYears = [
            oilProducerCostProfile,
            gasProducerCostProfile,
            waterInjectorCostProfile,
            gasInjectorCostProfile,
        ].map((series) => series?.startYear).filter((startYear) => startYear !== undefined) as number[]

        const minStartYear = startYears.length > 0 ? Math.min(...startYears) : 2020

        let drillingCostSeriesList: (ITimeSeries | undefined)[] = [
            oilProducerCostProfile,
            gasProducerCostProfile,
            waterInjectorCostProfile,
            gasInjectorCostProfile,
        ]

        const rigUpgradingCost = project.developmentOperationalWellCosts.rigUpgrading
        const rigMobDemobCost = project.developmentOperationalWellCosts.rigMobDemob
        const sumOfRigAndMobDemob = rigUpgradingCost + rigMobDemobCost

        if (sumOfRigAndMobDemob > 0) {
            interface ITimeSeriesWithCostProfile extends ITimeSeries {
                developmentRigUpgradingAndMobDemobCostProfile?: number[] | null;
            }

            const timeSeriesWithCostProfile: ITimeSeriesWithCostProfile = {
                id: "developmentRigUpgradingAndMobDemob",
                startYear: minStartYear,
                name: "Development Rig Upgrading and Mob/Demob Costs",
                values: [sumOfRigAndMobDemob],
                sum: sumOfRigAndMobDemob,
            }

            if (
                drillingCostSeriesList.every((series) => !series || !series.values || series.values.length === 0)
                && timeSeriesWithCostProfile?.values && timeSeriesWithCostProfile.values.length > 0
            ) {
                drillingCostSeriesList = [timeSeriesWithCostProfile]
            }
            if (!drillingCostSeriesList.includes(timeSeriesWithCostProfile)) {
                drillingCostSeriesList.push(timeSeriesWithCostProfile)
            }
        }
        setTotalDrillingCost(mergeTimeseriesList(drillingCostSeriesList))
    }

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

            handleOffshoreOpexPlussWellIntervention()
            handleTotalExplorationCost()
            handleOffshoreFacilitiesCost()
            handleDrilling()

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
        if (saveProjectCase) { handleCaseSave() }
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
                        {tabNames.map((tabName) => <Tab key={tabName}>{tabName}</Tab>)}
                    </List>
                    <Panels>
                        <Panel>
                            <CaseDescriptionTab />
                        </Panel>
                        <Panel>
                            <CaseProductionProfilesTab />
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
                            <CaseFacilitiesTab />
                        </Panel>
                        <Panel>
                            <CaseCostTab />
                        </Panel>
                        <Panel>
                            <CaseCO2Tab />
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
