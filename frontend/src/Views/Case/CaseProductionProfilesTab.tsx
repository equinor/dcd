import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
} from "react"
import styled from "styled-components"

import {
    Button, NativeSelect, Progress, Typography,
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
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import { GetGenerateProfileService } from "../../Services/GenerateProfileService"
import { ImportedElectricity } from "../../models/assets/drainagestrategy/ImportedElectricity"

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
const InputWrapper = styled.div`
    margin-right: 20px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
    drainageStrategy: DrainageStrategy,
    setDrainageStrategy: Dispatch<SetStateAction<DrainageStrategy | undefined>>,
    activeTab: number
}

function CaseProductionProfilesTab({
    project, setProject,
    caseItem, setCase,
    drainageStrategy, setDrainageStrategy,
    activeTab,
}: Props) {
    const [gas, setGas] = useState<ProductionProfileGas>()
    const [oil, setOil] = useState<ProductionProfileOil>()
    const [water, setWater] = useState<ProductionProfileWater>()
    const [nGL, setNGL] = useState<ProductionProfileNGL>()
    const [waterInjection, setWaterInjection] = useState<ProductionProfileWaterInjection>()

    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [importedElectricity, setImportedElectricity] = useState<ImportedElectricity>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()

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

    const handleCaseFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Case = { ...caseItem }
        const newfacilitiesAvailability = Math.min(Math.max(Number(e.currentTarget.value), 0), 100)
        newCase.facilitiesAvailability = newfacilitiesAvailability / 100
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

    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil production",
            unit: `${project?.physUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setOil,
            profile: oil,
        },
        {
            profileName: "Gas production",
            unit: `${project?.physUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            set: setGas,
            profile: gas,
        },
        {
            profileName: "Water production",
            unit: `${project?.physUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setWater,
            profile: water,
        },
        {
            profileName: "Water injection",
            unit: `${project?.physUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setWaterInjection,
            profile: waterInjection,
        },
        {
            profileName: "Fuel flaring and losses",
            unit: `${project?.physUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            profile: fuelFlaringAndLosses,
        },
        {
            profileName: "Net sales gas",
            unit: `${project?.physUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            profile: netSalesGas,
        },
        {
            profileName: "Imported electricity",
            unit: "GWh",
            profile: importedElectricity,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 1) {
                    const fuelFlaringProfile = (await GetGenerateProfileService())
                        .generateFuelFlaringLossesProfile(caseItem.id)
                    const netSaleProfile = (await GetGenerateProfileService()).generateNetSaleProfile(caseItem.id)
                    const importedElectricityProfile = (await GetGenerateProfileService())
                        .generateImportedElectricityProfile(caseItem.id)
                    setFuelFlaringAndLosses(await fuelFlaringProfile)
                    setNetSalesGas(await netSaleProfile)
                    setImportedElectricity(await importedElectricityProfile)

                    SetTableYearsFromProfiles([drainageStrategy.netSalesGas, drainageStrategy.fuelFlaringAndLosses,
                        drainageStrategy.productionProfileGas, drainageStrategy.productionProfileOil,
                        drainageStrategy.productionProfileWater, drainageStrategy.productionProfileNGL,
                        drainageStrategy.productionProfileWaterInjection,
                    ], caseItem.DG4Date.getFullYear(), setStartYear, setEndYear, setTableYears)
                    setGas(drainageStrategy.productionProfileGas)
                    setOil(drainageStrategy.productionProfileOil)
                    setWater(drainageStrategy.productionProfileWater)
                    setNGL(drainageStrategy.productionProfileNGL)
                    setWaterInjection(drainageStrategy.productionProfileWaterInjection)
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

    useEffect(() => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        if (newDrainageStrategy.productionProfileOil && !oil) { return }
        newDrainageStrategy.productionProfileOil = oil
        setDrainageStrategy(newDrainageStrategy)
    }, [oil])

    useEffect(() => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        if (newDrainageStrategy.productionProfileGas && !gas) { return }
        newDrainageStrategy.productionProfileGas = gas
        setDrainageStrategy(newDrainageStrategy)
    }, [gas])

    useEffect(() => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        if (newDrainageStrategy.productionProfileWater && !water) { return }
        newDrainageStrategy.productionProfileWater = water
        setDrainageStrategy(newDrainageStrategy)
    }, [water])

    useEffect(() => {
        const newDrainageStrategy: DrainageStrategy = { ...drainageStrategy }
        if (newDrainageStrategy.productionProfileWaterInjection && !waterInjection) { return }
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        setDrainageStrategy(newDrainageStrategy)
    }, [waterInjection])

    if (activeTab !== 1) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Production profiles</PageTitle>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={handleCaseFacilitiesAvailabilityChange}
                            defaultValue={caseItem.facilitiesAvailability * 100}
                            integer={false}
                            label="Facilities availability"
                            unit="%"
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
                    <InputWrapper>
                        <NativeSelectField
                            id="productionStrategy"
                            label="Production strategy overview"
                            onChange={() => { }}
                            disabled
                            value={caseItem.productionStrategyOverview}
                        >
                            <option key={0} value={0}>Depletion</option>
                            <option key={1} value={1}>Water injection</option>
                            <option key={2} value={2}>Gas injection</option>
                            <option key={3} value={3}>WAG</option>
                            <option key={4} value={4}>Mixed</option>
                        </NativeSelectField>
                    </InputWrapper>
                    <InputWrapper>
                        <NativeSelectField
                            id="artificialLift"
                            label="Artificial lift"
                            onChange={() => { }}
                            disabled
                            value={caseItem.artificialLift}
                        >
                            <option key="0" value={0}>No lift</option>
                            <option key="1" value={1}>Gas lift</option>
                            <option key="2" value={2}>Electrical submerged pumps</option>
                            <option key="3" value={3}>Subsea booster pumps</option>
                        </NativeSelectField>
                    </InputWrapper>
                </RowWrapper>
                <RowWrapper>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => { }}
                            defaultValue={caseItem.producerCount}
                            integer
                            disabled
                            label="Oil producer wells"
                        />
                    </NumberInputField>
                    <NumberInputField>
                        <CaseNumberInput
                            onChange={() => { }}
                            defaultValue={caseItem.waterInjectorCount}
                            integer
                            disabled
                            label="Water injector count"
                        />
                    </NumberInputField>
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={caseItem.gasInjectorCount}
                        integer
                        disabled
                        label="Gas injector count"
                    />
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
                            defaultValue={startYear}
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
                            defaultValue={endYear}
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
            <CaseTabTable
                caseItem={caseItem}
                project={project}
                setCase={setCase}
                setProject={setProject}
                timeSeriesData={timeSeriesData}
                dg4Year={caseItem.DG4Date.getFullYear()}
                tableYears={tableYears}
                tableName="Production profiles"
            />
        </>
    )
}

export default CaseProductionProfilesTab
