import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import { DrainageStrategy } from "../../models/assets/drainagestrategy/DrainageStrategy"
import { GetDrainageStrategyService } from "../../Services/DrainageStrategyService"
import CaseTabTable from "./CaseTabTable"
import { NetSalesGas } from "../../models/assets/drainagestrategy/NetSalesGas"
import { FuelFlaringAndLosses } from "../../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { ProductionProfileGas } from "../../models/assets/drainagestrategy/ProductionProfileGas"
import { ProductionProfileOil } from "../../models/assets/drainagestrategy/ProductionProfileOil"
import { ProductionProfileWater } from "../../models/assets/drainagestrategy/ProductionProfileWater"
import { ProductionProfileNGL } from "../../models/assets/drainagestrategy/ProductionProfileNGL"
import { ProductionProfileWaterInjection } from "../../models/assets/drainagestrategy/ProductionProfileWaterInjection"
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
    setSubstrucutre: Dispatch<SetStateAction<Substructure | undefined>>,
    transport: Transport,
    setTransport: Dispatch<SetStateAction<Transport | undefined>>,
    exploration: Exploration,
    setExploration: Dispatch<SetStateAction<Exploration | undefined>>,
    wellProject: WellProject,
    setWellProject: Dispatch<SetStateAction<WellProject | undefined>>,
    drainageStrategy: DrainageStrategy
}

function CaseSummaryTab({
    project, setProject,
    caseItem, setCase,
    exploration, setExploration,
    wellProject, setWellProject,
    topside, setTopside,
    surf, setSurf,
    substructure, setSubstrucutre,
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

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const getTimeSeriesLastYear = (timeSeries: ITimeSeries | undefined): number | undefined => {
        if (timeSeries && timeSeries.startYear && timeSeries.values) {
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
                const study = await (await GetCaseService()).generateStudyCost(caseItem.id)
                setStudyCost(study)
                const opex = await (await GetCaseService()).generateOpexCost(caseItem.id)
                setOpexCost(opex)
                const cessation = await (await GetCaseService()).generateCessationCost(caseItem.id)
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

                setTableYearsFromProfiles([study, opex, cessation,
                ])
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [])

    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()
    const [gas, setGas] = useState<ProductionProfileGas>()
    const [oil, setOil] = useState<ProductionProfileOil>()
    const [water, setWater] = useState<ProductionProfileWater>()
    const [nGL, setNGL] = useState<ProductionProfileNGL>()
    const [waterInjection, setWaterInjection] = useState<ProductionProfileWaterInjection>()

    // const updateAndSetExploration = (drainage: DrainageStrategy) => {
    //     const newDrainageStrategy: DrainageStrategy = { ...drainage }
    //     newDrainageStrategy.netSalesGas = netSalesGas
    //     newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
    //     newDrainageStrategy.productionProfileGas = gas
    //     newDrainageStrategy.productionProfileOil = oil
    //     newDrainageStrategy.productionProfileWater = water
    //     newDrainageStrategy.productionProfileNGL = nGL
    //     newDrainageStrategy.productionProfileWaterInjection = waterInjection
    //     setDrainageStrategy(newDrainageStrategy)
    // }

    const handleCaseNPVChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        newCase.npv = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleCaseBreakEvenChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = Case.Copy(caseItem)
        newCase.breakEven = Number(e.currentTarget.value)
        setCase(newCase)
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
            profileName: "Topside cost", unit: "MNOK", profile: topsideCost, set: setTopsideCost,
        },
        {
            profileName: "SURF cost", unit: "MNOK", profile: surfCost, set: setSurfCost,
        },
        {
            profileName: "Substructure cost", unit: "MNOK", profile: substructureCost, set: setSubstructureCost,
        },
        {
            profileName: "Transport cost", unit: "MNOK", profile: transportCost, set: setTransportCost,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    // useEffect(() => {
    //     setNetSalesGas(drainageStrategy.netSalesGas)
    //     setFuelFlaringAndLosses(drainageStrategy.fuelFlaringAndLosses)
    //     setGas(drainageStrategy.productionProfileGas)
    //     setOil(drainageStrategy.productionProfileOil)
    //     setWater(drainageStrategy.productionProfileWater)
    //     setNGL(drainageStrategy.productionProfileNGL)
    //     setWaterInjection(drainageStrategy.productionProfileWaterInjection)
    // }, [])

    const handleSave = async () => {
        // if (drainageStrategy) {
        //     const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        //     newDrainageStrategy.netSalesGas = netSalesGas
        //     newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
        //     newDrainageStrategy.productionProfileGas = gas
        //     newDrainageStrategy.productionProfileOil = oil
        //     newDrainageStrategy.productionProfileWater = water
        //     newDrainageStrategy.productionProfileNGL = nGL
        //     newDrainageStrategy.productionProfileWaterInjection = waterInjection
        //     const result = await (await GetDrainageStrategyService()).newUpdate(newDrainageStrategy)
        //     setDrainageStrategy(result)
        // }
        const updatedSurfResult = await (await GetSurfService()).newUpdate(surf)
        setSurf(updatedSurfResult)
        const updateedCaseResult = await (await GetCaseService()).update(caseItem)
        setCase(updateedCaseResult)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Summary</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseNPVChange}
                            value={caseItem.npv}
                            integer={false}
                            label="NPV before tax"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseBreakEvenChange}
                            value={caseItem.breakEven}
                            integer={false}
                            label="B/E before tax"
                        />
                    </NumberInputField>
                </RowWrapper>
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
                />
            </TableWrapper>
        </>
    )
}

export default CaseSummaryTab
