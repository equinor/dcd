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
import CaseProductionProfilesTabTable from "./CaseProductionProfilesTabTable"
import { NetSalesGas } from "../../models/assets/drainagestrategy/NetSalesGas"
import { FuelFlaringAndLosses } from "../../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { ProductionProfileGas } from "../../models/assets/drainagestrategy/ProductionProfileGas"
import { ProductionProfileOil } from "../../models/assets/drainagestrategy/ProductionProfileOil"
import { ProductionProfileWater } from "../../models/assets/drainagestrategy/ProductionProfileWater"
import { ProductionProfileNGL } from "../../models/assets/drainagestrategy/ProductionProfileNGL"
import { ProductionProfileWaterInjection } from "../../models/assets/drainagestrategy/ProductionProfileWaterInjection"
import { GetCaseService } from "../../Services/CaseService"
import { ITimeSeries } from "../../models/ITimeSeries"

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

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    drainageStrategy: DrainageStrategy,
    setDrainageStrategy: Dispatch<SetStateAction<DrainageStrategy | undefined>>,
}

function CaseProductionProfilesTab({
    project, setProject,
    caseItem, setCase,
    drainageStrategy, setDrainageStrategy,
}: Props) {
    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()
    const [gas, setGas] = useState<ProductionProfileGas>()
    const [oil, setOil] = useState<ProductionProfileOil>()
    const [water, setWater] = useState<ProductionProfileWater>()
    const [nGL, setNGL] = useState<ProductionProfileNGL>()
    const [waterInjection, setWaterInjection] = useState<ProductionProfileWaterInjection>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const updateAndSetDraiangeStrategy = (drainage: DrainageStrategy) => {
        const newDrainageStrategy: DrainageStrategy = { ...drainage }
        newDrainageStrategy.netSalesGas = netSalesGas
        newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
        newDrainageStrategy.productionProfileGas = gas
        newDrainageStrategy.productionProfileOil = oil
        newDrainageStrategy.productionProfileWater = water
        newDrainageStrategy.productionProfileNGL = nGL
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        setDrainageStrategy(newDrainageStrategy)
    }

    const handleDrainageStrategyNGLYieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        newDrainageStrategy.nglYield = Number(e.currentTarget.value)
        updateAndSetDraiangeStrategy(newDrainageStrategy)
    }

    const handleCaseFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.facilitiesAvailability = Number(e.currentTarget.value)
        setCase(newCase)
    }

    const handleDrainageStrategyGasSolutinChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1) {
            // eslint-disable-next-line max-len
            const newGasSolution: Components.Schemas.GasSolution = Number(e.currentTarget.value) as Components.Schemas.GasSolution
            const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
            newDrainageStrategy.gasSolution = newGasSolution
            updateAndSetDraiangeStrategy(newDrainageStrategy)
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
        if (newEndYear > 2050) {
            setEndYear(2050)
            return
        }
        setEndYear(newEndYear)
    }

    interface ITimeSeriesData {
        profileName: string
        unit: string,
        set: Dispatch<SetStateAction<ITimeSeries | undefined>>,
        profile: ITimeSeries | undefined
    }

    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Net sales gas", unit: "GSm3/yr", set: setNetSalesGas, profile: netSalesGas,
        },
        {
            profileName: "Fuel flaring and losses",
            unit: "GSm3/yr",
            set: setFuelFlaringAndLosses,
            profile: fuelFlaringAndLosses,
        },
        {
            profileName: "Gas production", unit: "GSm3/yr", set: setGas, profile: gas,
        },
        {
            profileName: "Oil production", unit: "MSm3/yr", set: setOil, profile: oil,
        },
        {
            profileName: "Water production", unit: "????", set: setWater, profile: water,
        },
        {
            profileName: "Water injection", unit: "MSm3/yr", set: setWaterInjection, profile: waterInjection,
        },
    ]

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

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    useEffect(() => {
        setTableYearsFromProfiles([drainageStrategy.netSalesGas, drainageStrategy.fuelFlaringAndLosses,
        drainageStrategy.productionProfileGas, drainageStrategy.productionProfileOil,
        drainageStrategy.productionProfileWater, drainageStrategy.productionProfileNGL,
        drainageStrategy.productionProfileWaterInjection,
        ])
        setNetSalesGas(drainageStrategy.netSalesGas)
        setFuelFlaringAndLosses(drainageStrategy.fuelFlaringAndLosses)
        setGas(drainageStrategy.productionProfileGas)
        setOil(drainageStrategy.productionProfileOil)
        setWater(drainageStrategy.productionProfileWater)
        setNGL(drainageStrategy.productionProfileNGL)
        setWaterInjection(drainageStrategy.productionProfileWaterInjection)
    }, [])

    const handleSave = async () => {
        if (drainageStrategy) {
            const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
            newDrainageStrategy.netSalesGas = netSalesGas
            newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
            newDrainageStrategy.productionProfileGas = gas
            newDrainageStrategy.productionProfileOil = oil
            newDrainageStrategy.productionProfileWater = water
            newDrainageStrategy.productionProfileNGL = nGL
            newDrainageStrategy.productionProfileWaterInjection = waterInjection
            const result = await (await GetDrainageStrategyService()).newUpdate(newDrainageStrategy)
            setDrainageStrategy(result)
        }
        const updateCaseResult = await (await GetCaseService()).update(caseItem)
        setCase(updateCaseResult)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Production profiles</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseFacilitiesAvailabilityChange}
                            value={caseItem.facilitiesAvailability}
                            integer={false}
                            label="Facilities availability (%)"
                        />
                    </NumberInputField>
                    <NativeSelectField
                        id="gasSolution"
                        label="Gas solution"
                        onChange={handleDrainageStrategyGasSolutinChange}
                        value={drainageStrategy?.gasSolution}
                    >
                        <option key={0} value={0}>Export</option>
                        <option key={1} value={1}>Injection</option>
                    </NativeSelectField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleDrainageStrategyNGLYieldChange}
                            value={drainageStrategy?.nglYield}
                            integer={false}
                            label="NGL yield"
                        />
                    </NumberInputField>
                </RowWrapper>
            </ColumnWrapper>
            <ColumnWrapper>
                <TableYearWrapper>
                    <NativeSelectField
                        id="unit"
                        label="Units"
                        onChange={() => { }}
                        value={project.physUnit}
                        disabled
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
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
            <CaseProductionProfilesTabTable
                caseItem={caseItem}
                project={project}
                setCase={setCase}
                setProject={setProject}
                timeSeriesData={timeSeriesData}
                dg4Year={caseItem.DG4Date.getFullYear()}
                tableYears={tableYears}
            />
        </>
    )
}

export default CaseProductionProfilesTab
