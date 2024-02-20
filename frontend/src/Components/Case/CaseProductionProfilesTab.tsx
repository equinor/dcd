import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
    useRef,
} from "react"
import styled from "styled-components"
import {
    Button, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import InputContainer from "../Input/InputContainer"
import CaseNumberInput from "./CaseNumberInput"
import FilterContainer from "../Input/FilterContainer"
import CaseTabTable from "./CaseTabTable"
import { ITimeSeries } from "../../Models/ITimeSeries"
import { SetTableYearsFromProfiles } from "./CaseTabTableHelper"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../AgGrid/AgChartsTimeseries"
import { ITimeSeriesOverride } from "../../Models/ITimeSeriesOverride"

const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const ApplyButtonWrapper = styled.div`
    display: flex;
    width: 100%;
    justify-content: center;
    align-items: flex-end;
`
interface Props {
    project: Components.Schemas.ProjectDto,
    caseItem: Components.Schemas.CaseDto,
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    drainageStrategy: Components.Schemas.DrainageStrategyDto,
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyDto | undefined>>,
    activeTab: number

    netSalesGas: Components.Schemas.NetSalesGasDto | undefined,
    setNetSalesGas: Dispatch<SetStateAction<Components.Schemas.NetSalesGasDto | undefined>>,

    fuelFlaringAndLosses: Components.Schemas.FuelFlaringAndLossesDto | undefined,
    setFuelFlaringAndLosses: Dispatch<SetStateAction<Components.Schemas.FuelFlaringAndLossesDto | undefined>>,

    importedElectricity: Components.Schemas.ImportedElectricityDto | undefined,
    setImportedElectricity: Dispatch<SetStateAction<Components.Schemas.ImportedElectricityDto | undefined>>,
}

const CaseProductionProfilesTab = ({
    project,
    caseItem, setCase,
    drainageStrategy, setDrainageStrategy,
    activeTab,
    netSalesGas, setNetSalesGas,
    fuelFlaringAndLosses, setFuelFlaringAndLosses,
    importedElectricity, setImportedElectricity,
}: Props) => {
    const [gas, setGas] = useState<Components.Schemas.ProductionProfileGasDto>()
    const [oil, setOil] = useState<Components.Schemas.ProductionProfileOilDto>()
    const [water, setWater] = useState<Components.Schemas.ProductionProfileWaterDto>()
    const [nGL, setNGL] = useState<Components.Schemas.ProductionProfileNGLDto>()
    const [waterInjection, setWaterInjection] = useState<Components.Schemas.ProductionProfileWaterInjectionDto>()

    const [netSalesGasOverride, setNetSalesGasOverride] = useState<Components.Schemas.NetSalesGasOverrideDto>()
    const [fuelFlaringAndLossesOverride, setFuelFlaringAndLossesOverride] = useState<Components.Schemas.FuelFlaringAndLossesOverrideDto>()
    const [importedElectricityOverride, setImportedElectricityOverride] = useState<Components.Schemas.ImportedElectricityOverrideDto>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const gridRef = useRef<any>(null)

    const updateAndSetDraiangeStrategy = (drainage: Components.Schemas.DrainageStrategyDto) => {
        if (drainageStrategy === undefined) { return }
        if (netSalesGas === undefined
            || fuelFlaringAndLosses === undefined
            || gas === undefined
            || oil === undefined
            || water === undefined
            || nGL === undefined
            || waterInjection === undefined
            || importedElectricityOverride === undefined
            || netSalesGasOverride === undefined
            || fuelFlaringAndLossesOverride === undefined) {
            return
        }
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainage }
        newDrainageStrategy.netSalesGas = netSalesGas
        newDrainageStrategy.netSalesGasOverride = netSalesGasOverride
        newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
        newDrainageStrategy.fuelFlaringAndLossesOverride = fuelFlaringAndLossesOverride
        newDrainageStrategy.productionProfileGas = gas
        newDrainageStrategy.productionProfileOil = oil
        newDrainageStrategy.productionProfileWater = water
        newDrainageStrategy.productionProfileNGL = nGL
        newDrainageStrategy.productionProfileWaterInjection = waterInjection

        newDrainageStrategy.importedElectricityOverride = importedElectricityOverride
        setDrainageStrategy(newDrainageStrategy)
    }

    const handleCaseFacilitiesAvailabilityChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newfacilitiesAvailability = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        setCase(newCase)
    }

    const handleDrainageStrategyGasSolutionChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newGasSolution: Components.Schemas.GasSolution = Number(e.currentTarget.value) as Components.Schemas.GasSolution
            const newDrainageStrategy = { ...drainageStrategy }
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
        overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
        profile: ITimeSeries | undefined
        overrideProfile?: ITimeSeries | undefined
        overridable?: boolean
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
            profileName: "Fuel, flaring and losses",
            unit: `${project?.physUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            profile: fuelFlaringAndLosses,
            overridable: true,
            overrideProfile: fuelFlaringAndLossesOverride,
            overrideProfileSet: setFuelFlaringAndLossesOverride,
        },
        {
            profileName: "Net sales gas",
            unit: `${project?.physUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            profile: netSalesGas,
            overridable: true,
            overrideProfile: netSalesGasOverride,
            overrideProfileSet: setNetSalesGasOverride,
        },
        {
            profileName: "Imported electricity",
            unit: "GWh",
            profile: importedElectricity,
            overridable: true,
            overrideProfile: importedElectricityOverride,
            overrideProfileSet: setImportedElectricityOverride,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const productionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseItem.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oil, i, startYear, new Date(caseItem.dG4Date).getFullYear()),
                gasProduction: setValueToCorrespondingYear(gas, i, startYear, new Date(caseItem.dG4Date).getFullYear()),
                waterProduction: setValueToCorrespondingYear(water, i, startYear, new Date(caseItem.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    const injectionProfilesChartData = () => {
        const dataArray: object[] = []
        if (caseItem.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                waterInjection:
                    setValueToCorrespondingYear(waterInjection, i, startYear, new Date(caseItem.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    useEffect(() => {
        (async () => {
            try {
                if (activeTab === 1 && caseItem.dG4Date !== undefined) {
                    setFuelFlaringAndLosses(drainageStrategy.fuelFlaringAndLosses)
                    setNetSalesGas(drainageStrategy.netSalesGas)
                    setImportedElectricity(drainageStrategy.importedElectricity)

                    SetTableYearsFromProfiles([drainageStrategy.netSalesGas, drainageStrategy.fuelFlaringAndLosses,
                    drainageStrategy.netSalesGasOverride, drainageStrategy.fuelFlaringAndLossesOverride,
                    drainageStrategy.productionProfileGas, drainageStrategy.productionProfileOil,
                    drainageStrategy.productionProfileWater, drainageStrategy.productionProfileNGL,
                    drainageStrategy.productionProfileWaterInjection, drainageStrategy.importedElectricityOverride,
                    drainageStrategy.co2EmissionsOverride,
                    ], new Date(caseItem.dG4Date).getFullYear(), setStartYear, setEndYear, setTableYears)
                    setGas(drainageStrategy.productionProfileGas)
                    setOil(drainageStrategy.productionProfileOil)
                    setWater(drainageStrategy.productionProfileWater)
                    setNGL(drainageStrategy.productionProfileNGL)
                    setWaterInjection(drainageStrategy.productionProfileWaterInjection)

                    setImportedElectricityOverride(drainageStrategy.importedElectricityOverride)
                    setNetSalesGasOverride(drainageStrategy.netSalesGasOverride)
                    setFuelFlaringAndLossesOverride(drainageStrategy.fuelFlaringAndLossesOverride)
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTab])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!oil) { return }
        newDrainageStrategy.productionProfileOil = oil
        setDrainageStrategy(newDrainageStrategy)
    }, [oil])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!gas) { return }
        newDrainageStrategy.productionProfileGas = gas
        setDrainageStrategy(newDrainageStrategy)
    }, [gas])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!water) { return }
        newDrainageStrategy.productionProfileWater = water
        setDrainageStrategy(newDrainageStrategy)
    }, [water])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!waterInjection) { return }
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        setDrainageStrategy(newDrainageStrategy)
    }, [waterInjection])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!importedElectricityOverride) { return }
        newDrainageStrategy.importedElectricityOverride = importedElectricityOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [importedElectricityOverride])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!fuelFlaringAndLossesOverride) { return }
        newDrainageStrategy.fuelFlaringAndLossesOverride = fuelFlaringAndLossesOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [fuelFlaringAndLossesOverride])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyDto = { ...drainageStrategy }
        if (!netSalesGasOverride) { return }
        newDrainageStrategy.netSalesGasOverride = netSalesGasOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [netSalesGasOverride])

    useEffect(() => {
        if (gridRef.current && gridRef.current.api && gridRef.current.api.refreshCells) {
            gridRef.current.api.refreshCells()
        }
    }, [fuelFlaringAndLosses, netSalesGas, importedElectricity])

    if (activeTab !== 1) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Production profiles</PageTitle>
            </TopWrapper>
            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>
                <CaseNumberInput
                    onChange={handleCaseFacilitiesAvailabilityChange}
                    defaultValue={caseItem.facilitiesAvailability
                        !== undefined ? caseItem.facilitiesAvailability * 100 : undefined}
                    integer={false}
                    label="Facilities availability"
                    unit="%"
                />
                <NativeSelect
                    id="gasSolution"
                    label="Gas solution"
                    onChange={handleDrainageStrategyGasSolutionChange}
                    value={drainageStrategy?.gasSolution}
                >
                    <option key={0} value={0}>Export</option>
                    <option key={1} value={1}>Injection</option>
                </NativeSelect>
                <NativeSelect
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
                </NativeSelect>
                <NativeSelect
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
                </NativeSelect>
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={caseItem.producerCount}
                    integer
                    disabled
                    label="Oil producer wells"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={caseItem.waterInjectorCount}
                    integer
                    disabled
                    label="Water injector count"
                />
                <CaseNumberInput
                    onChange={() => { }}
                    defaultValue={caseItem.gasInjectorCount}
                    integer
                    disabled
                    label="Gas injector count"
                />

            </InputContainer>
            <FilterContainer>
                <NativeSelect
                    id="unit"
                    label="Units"
                    onChange={() => { }}
                    value={project.physUnit}
                    disabled
                >
                    <option key={0} value={0}>SI</option>
                    <option key={1} value={1}>Oil field</option>
                </NativeSelect>

                <CaseNumberInput
                    onChange={handleStartYearChange}
                    defaultValue={startYear}
                    integer
                    label="Start year"
                />
                <CaseNumberInput
                    onChange={handleEndYearChange}
                    defaultValue={endYear}
                    integer
                    label="End year"
                />
                <Button onClick={handleTableYearsClick}>  Apply </Button>
            </FilterContainer>
            <AgChartsTimeseries
                data={productionProfilesChartData()}
                chartTitle="Production profiles"
                barColors={["#243746", "#EB0037", "#A8CED1"]}
                barProfiles={["oilProduction", "gasProduction", "waterProduction"]}
                barNames={[
                    "Oil production (MSm3)",
                    "Gas production (GSm3)",
                    "Water production (MSm3)",
                ]}
            />
            {
                (waterInjection?.values && waterInjection.values?.length > 0)
                && (
                    <AgChartsTimeseries
                        data={injectionProfilesChartData()}
                        chartTitle="Injection profiles"
                        barColors={["#A8CED1"]}
                        barProfiles={["waterInjection"]}
                        barNames={["Water injection"]}
                        unit="MSm3"
                    />
                )
            }
            <CaseTabTable
                timeSeriesData={timeSeriesData}
                dg4Year={caseItem.dG4Date ? new Date(caseItem.dG4Date).getFullYear() : 2030}
                tableYears={tableYears}
                tableName="Production profiles"
                includeFooter={false}
                gridRef={gridRef}
            />
        </>
    )
}

export default CaseProductionProfilesTab
