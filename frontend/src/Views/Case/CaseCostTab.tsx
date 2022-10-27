import {
    ChangeEventHandler, Dispatch, SetStateAction, useEffect, useRef, useState,
} from "react"
import styled from "styled-components"

import { Button, NativeSelect, Typography } from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { DrainageStrategy } from "../../models/assets/drainagestrategy/DrainageStrategy"
import CaseTabTable from "./CaseTabTable"
import { GetCaseService } from "../../Services/CaseService"
import { ITimeSeries } from "../../models/ITimeSeries"
import { StudyCostProfile } from "../../models/case/StudyCostProfile"
import { OpexCostProfile } from "../../models/case/OpexCostProfile"
import { CaseCessationCostProfile } from "../../models/case/CaseCessationCostProfile"
import { SeismicAcquisitionAndProcessing } from "../../models/assets/exploration/SeismicAcquisitionAndProcessing"
import { CountryOfficeCost } from "../../models/assets/exploration/CountryOfficeCost"
import { GAndGAdminCost } from "../../models/assets/exploration/GAndGAdminCost"
import { Exploration } from "../../models/assets/exploration/Exploration"
import { GetExplorationService } from "../../Services/ExplorationService"
import { Surf } from "../../models/assets/surf/Surf"
import { GetSurfService } from "../../Services/SurfService"
import { ExplorationCostProfile } from "../../models/assets/exploration/ExplorationCostProfile"
import { WellProjectCostProfile } from "../../models/assets/wellproject/WellProjectCostProfile"
import { WellProject } from "../../models/assets/wellproject/WellProject"
import { Substructure } from "../../models/assets/substructure/Substructure"
import { Topside } from "../../models/assets/topside/Topside"
import { Transport } from "../../models/assets/transport/Transport"
import { TopsideCostProfile } from "../../models/assets/topside/TopsideCostProfile"
import { SurfCostProfile } from "../../models/assets/surf/SurfCostProfile"
import { SubstructureCostProfile } from "../../models/assets/substructure/SubstructureCostProfile"
import { TransportCostProfile } from "../../models/assets/transport/TransportCostProfile"
import { GetTopsideService } from "../../Services/TopsideService"
import { GetSubstructureService } from "../../Services/SubstructureService"
import { GetTransportService } from "../../Services/TransportService"
import { GetWellProjectService } from "../../Services/WellProjectService"
import { GetGenerateProfileService } from "../../Services/GenerateProfileService"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const NativeSelectField = styled(NativeSelect)`
    width: 200px;
    padding-right: 20px;
`
const NumberInputField = styled.div`
    padding-right: 20px;
`

const TableYearWrapper = styled.div`
    align-items: flex-end;
    display: flex;
    flex-direction: row;
    align-content: right;
    margin-left: auto;
    margin-bottom: 20px;
`
const YearInputWrapper = styled.div`
    width: 80px;
    padding-right: 10px;
`
const YearDashWrapper = styled.div`
    padding-right: 5px;
`
const TableWrapper = styled.div`
    margin-bottom: 50px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    topside: Topside,
    setTopside: Dispatch<SetStateAction<Topside | undefined>>,
    surf: Surf,
    setSurf: Dispatch<SetStateAction<Surf | undefined>>,
    substructure: Substructure,
    setSubstructure: Dispatch<SetStateAction<Substructure | undefined>>,
    transport: Transport,
    setTransport: Dispatch<SetStateAction<Transport | undefined>>,
    exploration: Exploration,
    setExploration: Dispatch<SetStateAction<Exploration | undefined>>,
    wellProject: WellProject,
    setWellProject: Dispatch<SetStateAction<WellProject | undefined>>,
    drainageStrategy: DrainageStrategy
}

function CaseCostTab({
    project, setProject,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstructure,
    transport, setTransport,
    drainageStrategy,
}: Props) {
    // OPEX
    const [studyCost, setStudyCost] = useState<StudyCostProfile>()
    const [opexCost, setOpexCost] = useState<OpexCostProfile>()
    const [cessationCost, setCessationCost] = useState<CaseCessationCostProfile>()

    // CAPEX
    const [topsideCost, setTopsideCost] = useState<TopsideCostProfile>()
    const [surfCost, setSurfCost] = useState<SurfCostProfile>()
    const [substructureCost, setSubstructureCost] = useState<SubstructureCostProfile>()
    const [transportCost, setTransportCost] = useState<TransportCostProfile>()

    // Development
    const [wellProjectCost, setWellProjectCost] = useState<WellProjectCostProfile>()

    // Exploration
    const [explorationCost, setExplorationCost] = useState<ExplorationCostProfile>()
    const [seismicAcqAndProcCost, setseismicAcqAndProcCost] = useState<SeismicAcquisitionAndProcessing>()
    const [countryOfficeCost, setCountryOfficeCost] = useState<CountryOfficeCost>()
    const [gAndGAdminCost, setGAndGAdminCost] = useState<GAndGAdminCost>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2100)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const opexGridRef = useRef(null)
    const capexGridRef = useRef(null)
    const developmentWellsGridRef = useRef(null)
    const explorationWellsGridRef = useRef(null)

    const getTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
        if (timeSeries && timeSeries.startYear !== undefined && timeSeries.values) {
            return timeSeries.startYear + timeSeries.values.length - 1
        } return undefined
    }

    const setTableYearsFromProfiles = (profiles: (ITimeSeries | undefined)[]) => {
        let firstYear = Number.MAX_SAFE_INTEGER
        let lastYear = Number.MIN_SAFE_INTEGER
        profiles.forEach((p) => {
            if (p && p.startYear !== undefined && p.startYear < firstYear) {
                firstYear = p.startYear
            }
            const profileLastYear = getTimeSeriesLastYear(p)
            if (profileLastYear !== undefined && profileLastYear > lastYear) {
                lastYear = profileLastYear
            }
        })
        if (firstYear < Number.MAX_SAFE_INTEGER && lastYear > Number.MIN_SAFE_INTEGER) {
            setStartYear(firstYear + caseItem.DG4Date.getFullYear())
            setEndYear(lastYear + caseItem.DG4Date.getFullYear())
            setTableYears([firstYear + caseItem.DG4Date.getFullYear(), lastYear + caseItem.DG4Date.getFullYear()])
        }
    }

    useEffect(() => {
        (async () => {
            try {
                // OPEX
                const study = await (await GetGenerateProfileService()).generateStudyCost(caseItem.id)
                setStudyCost(study)
                const opex = await (await GetGenerateProfileService()).generateOpexCost(caseItem.id)
                setOpexCost(opex)
                const cessation = await (await GetGenerateProfileService()).generateCessationCost(caseItem.id)
                setCessationCost(cessation)

                // CAPEX
                const topsideCostProfile = topside.costProfile
                setTopsideCost(topsideCostProfile)
                const surfCostProfile = surf.costProfile
                setSurfCost(surfCostProfile)
                const substructureCostProfile = substructure.costProfile
                setSubstructureCost(substructureCostProfile)
                const transportCostProfile = transport.costProfile
                setTransportCost(transportCostProfile)

                // Development
                const wellProjectCostProfile = wellProject.costProfile
                setWellProjectCost(wellProjectCostProfile)

                // Exploration
                const explorationCostProfile = exploration.costProfile
                setExplorationCost(explorationCostProfile)
                const seismicAcqAndProc = exploration.seismicAcquisitionAndProcessing
                setseismicAcqAndProcCost(seismicAcqAndProc)
                const countryOffice = exploration.countryOfficeCost
                setCountryOfficeCost(countryOffice)
                const gAndGAdmin = await (await GetGenerateProfileService()).generateGAndGAdminCost(caseItem.id)
                setGAndGAdminCost(gAndGAdmin)

                setTableYearsFromProfiles([study, opex, cessation,
                    surfCostProfile, topsideCostProfile, substructureCostProfile, transportCostProfile,
                    wellProjectCostProfile,
                    explorationCostProfile, seismicAcqAndProc, countryOffice, gAndGAdmin,
                ])
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [])

    const updatedAndSetSurf = (surfItem: Surf) => {
        const newSurf: Surf = { ...surfItem }
        newSurf.costProfile = surfCost
        setSurf(newSurf)
    }

    const handleCaseFeasibilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        const newCapexFactorFeasibilityStudies = Number(e.currentTarget.value)
        newCase.capexFactorFeasibilityStudies = newCapexFactorFeasibilityStudies / 100
        setCase(newCase)
    }

    const handleCaseFEEDChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        const newCapexFactorFEEDStudies = Number(e.currentTarget.value)
        newCase.capexFactorFEEDStudies = newCapexFactorFEEDStudies / 100
        setCase(newCase)
    }

    const handleSurfMaturityChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1, 2, 3].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newMaturity: Components.Schemas.Maturity = Number(e.currentTarget.value) as Components.Schemas.Maturity
            const newSurf: Surf = { ...surf }
            newSurf.maturity = newMaturity
            updatedAndSetSurf(newSurf)
        }
    }

    const handleStartYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newStartYear = Number(e.currentTarget.value)
        if (newStartYear < 2010) {
            setStartYear(2010)
            return
        }
        setStartYear(newStartYear)
    }

    const handleEndYearChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newEndYear = Number(e.currentTarget.value)
        if (newEndYear > 2100) {
            setEndYear(2100)
            return
        }
        setEndYear(newEndYear)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
    }

    const opexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Study cost", unit: "MNOK", profile: studyCost,
        },
        {
            profileName: "OPEX cost", unit: "MNOK", profile: opexCost,
        },
        {
            profileName: "Cessation cost", unit: "MNOK", profile: cessationCost,
        },
    ]

    const capexTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Subsea production system", unit: "MNOK", profile: surfCost, set: setSurfCost,
        },
        {
            profileName: "Topside", unit: "MNOK", profile: topsideCost, set: setTopsideCost,
        },
        {
            profileName: "Substructure", unit: "MNOK", profile: substructureCost, set: setSubstructureCost,
        },
        {
            profileName: "Transport system", unit: "MNOK", profile: transportCost, set: setTransportCost,
        },
    ]

    const developmentTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Development cost", unit: "MNOK", profile: wellProjectCost, set: setWellProjectCost,
        },
    ]

    const explorationTimeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "G&G and admin costs", unit: "MNOK", profile: gAndGAdminCost,
        },
        {
            profileName: "Seismic acquisition and processing",
            unit: "MNOK",
            profile: seismicAcqAndProcCost,
            set: setseismicAcqAndProcCost,
        },
        {
            profileName: "Country office cost", unit: "MNOK", profile: countryOfficeCost, set: setCountryOfficeCost,
        },
        {
            profileName: "Exploration cost", unit: "MNOK", profile: explorationCost, set: setExplorationCost,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    useEffect(() => {
        const newSurf: Surf = { ...surf }
        newSurf.costProfile = surfCost
        setSurf(newSurf)
    }, [surfCost])

    useEffect(() => {
        const newTopside: Topside = { ...topside }
        newTopside.costProfile = topsideCost
        setTopside(newTopside)
    }, [topsideCost])

    useEffect(() => {
        const newSubstructure: Substructure = { ...substructure }
        newSubstructure.costProfile = substructureCost
        setSubstructure(newSubstructure)
    }, [substructureCost])

    useEffect(() => {
        const newTransport: Transport = { ...transport }
        newTransport.costProfile = transportCost
        setTransport(newTransport)
    }, [transportCost])

    useEffect(() => {
        const newWellProject: WellProject = { ...wellProject }
        newWellProject.costProfile = wellProjectCost
        setWellProject(newWellProject)
    }, [wellProjectCost])

    useEffect(() => {
        const newExploration: Exploration = { ...exploration }
        newExploration.costProfile = explorationCost
        newExploration.seismicAcquisitionAndProcessing = seismicAcqAndProcCost
        newExploration.countryOfficeCost = countryOfficeCost
        setExploration(newExploration)
    }, [explorationCost, seismicAcqAndProcCost, countryOfficeCost])

    const handleSave = async () => {
        const updatedSurfResult = await (await GetSurfService()).newUpdate(surf)
        setSurf(updatedSurfResult)
        const updatedTopsideResult = await (await GetTopsideService()).newUpdate(topside)
        setTopside(updatedTopsideResult)
        const updatedSubstructureResult = await (await GetSubstructureService()).newUpdate(substructure)
        setSubstructure(updatedSubstructureResult)
        const updatedTransportResult = await (await GetTransportService()).newUpdate(transport)
        setTransport(updatedTransportResult)
        const updatedWellProjectResult = await (await GetWellProjectService()).newUpdate(wellProject)
        setWellProject(updatedWellProjectResult)
        const updatedExplorationResult = await (await GetExplorationService()).newUpdate(exploration)
        setExploration(updatedExplorationResult)
        const updateedCaseResult = await (await GetCaseService()).update(caseItem)
        setCase(updateedCaseResult)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Cost</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseFeasibilityChange}
                            value={caseItem.capexFactorFeasibilityStudies * 100}
                            integer={false}
                            label="CAPEX factor feasibility studies (%)"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseFEEDChange}
                            value={caseItem.capexFactorFEEDStudies * 100}
                            integer={false}
                            label="CAPEX factor FEED studies (%)"
                        />
                    </NumberInputField>
                    <NativeSelectField
                        id="maturity"
                        label="Maturity"
                        onChange={handleSurfMaturityChange}
                        value={surf.maturity}
                    >
                        <option key="0" value={0}>A</option>
                        <option key="1" value={1}>B</option>
                        <option key="2" value={2}>C</option>
                        <option key="3" value={3}>D</option>
                    </NativeSelectField>
                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <TableYearWrapper>
                    <NativeSelectField
                        id="currency"
                        label="Currency"
                        onChange={() => { }}
                        value={project.currency}
                        disabled
                    >
                        <option key="1" value={1}>MNOK</option>
                        <option key="2" value={2}>MUSD</option>
                    </NativeSelectField>
                    <YearInputWrapper>
                        <CaseNumberInput
                            onChange={handleStartYearChange}
                            value={startYear}
                            integer
                            label="Start year"
                        />
                    </YearInputWrapper>
                    <YearDashWrapper>
                        <Typography variant="h2">-</Typography>
                    </YearDashWrapper>
                    <YearInputWrapper>
                        <CaseNumberInput
                            onChange={handleEndYearChange}
                            value={endYear}
                            integer
                            label="End year"
                        />
                    </YearInputWrapper>
                    <Button
                        onClick={handleTableYearsClick}
                    >
                        Apply
                    </Button>
                </TableYearWrapper>
            </ColumnWrapper>
            <TableWrapper>
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={opexTimeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="OPEX"
                    gridRef={opexGridRef}
                    alignedGridsRef={[capexGridRef, developmentWellsGridRef, explorationWellsGridRef]}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={capexTimeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="CAPEX"
                    gridRef={capexGridRef}
                    alignedGridsRef={[opexGridRef, developmentWellsGridRef, explorationWellsGridRef]}
                />
            </TableWrapper>
            <TableWrapper>
                <CaseTabTable
                    caseItem={caseItem}
                    project={project}
                    setCase={setCase}
                    setProject={setProject}
                    timeSeriesData={developmentTimeSeriesData}
                    dg4Year={caseItem.DG4Date.getFullYear()}
                    tableYears={tableYears}
                    tableName="Development well costs"
                    gridRef={developmentWellsGridRef}
                    alignedGridsRef={[opexGridRef, capexGridRef, explorationWellsGridRef]}
                />
            </TableWrapper>
            <CaseTabTable
                caseItem={caseItem}
                project={project}
                setCase={setCase}
                setProject={setProject}
                timeSeriesData={explorationTimeSeriesData}
                dg4Year={caseItem.DG4Date.getFullYear()}
                tableYears={tableYears}
                tableName="Exploration well costs"
                gridRef={explorationWellsGridRef}
                alignedGridsRef={[opexGridRef, capexGridRef, developmentWellsGridRef]}
            />
        </>
    )
}

export default CaseCostTab
