import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
    useRef,
} from "react"
import { NativeSelect } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import SwitchableNumberInput from "../../Input/SwitchableNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../AgGrid/AgChartsTimeseries"
import { ITimeSeriesOverride } from "../../../Models/ITimeSeriesOverride"
import InputSwitcher from "../../Input/Components/InputSwitcher"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"
import DateRangePicker from "../../Input/TableDateRangePicker"
import SwitchableDropdownInput from "../../Input/SwitchableDropdownInput"

interface ITimeSeriesData {
    profileName: string
    unit: string,
    set?: Dispatch<SetStateAction<ITimeSeries | undefined>>,
    overrideProfileSet?: Dispatch<SetStateAction<ITimeSeriesOverride | undefined>>,
    profile: ITimeSeries | undefined
    overrideProfile?: ITimeSeries | undefined
    overridable?: boolean
}
interface Props {
    drainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto,
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyWithProfilesDto | undefined>>,

    netSalesGas: Components.Schemas.NetSalesGasDto | undefined,
    setNetSalesGas: Dispatch<SetStateAction<Components.Schemas.NetSalesGasDto | undefined>>,

    fuelFlaringAndLosses: Components.Schemas.FuelFlaringAndLossesDto | undefined,
    setFuelFlaringAndLosses: Dispatch<SetStateAction<Components.Schemas.FuelFlaringAndLossesDto | undefined>>,

    importedElectricity: Components.Schemas.ImportedElectricityDto | undefined,
    setImportedElectricity: Dispatch<SetStateAction<Components.Schemas.ImportedElectricityDto | undefined>>,
}

const CaseProductionProfilesTab = ({
    drainageStrategy, setDrainageStrategy,
    netSalesGas, setNetSalesGas,
    fuelFlaringAndLosses, setFuelFlaringAndLosses,
    importedElectricity, setImportedElectricity,
}: Props) => {
    const { project } = useProjectContext()
    const {
        projectCase, projectCaseEdited, setProjectCaseEdited, activeTabCase,
    } = useCaseContext()
    if (!projectCase) { return null }
    const [gas, setGas] = useState<Components.Schemas.ProductionProfileGasDto>()
    const [oil, setOil] = useState<Components.Schemas.ProductionProfileOilDto>()
    const [water, setWater] = useState<Components.Schemas.ProductionProfileWaterDto>()
    const [nGL, setNGL] = useState<Components.Schemas.ProductionProfileNGLDto>()
    const [waterInjection, setWaterInjection] = useState<Components.Schemas.ProductionProfileWaterInjectionDto>()

    const [netSalesGasOverride, setNetSalesGasOverride] = useState<Components.Schemas.NetSalesGasOverrideDto>()
    const [fuelFlaringAndLossesOverride, setFuelFlaringAndLossesOverride] = useState<Components.Schemas.FuelFlaringAndLossesOverrideDto>()
    const [importedElectricityOverride, setImportedElectricityOverride] = useState<Components.Schemas.ImportedElectricityOverrideDto>()

    const [deferredGas, setDeferredGas] = useState<Components.Schemas.DeferredGasProductionDto>()
    const [deferredOil, setDeferredOil] = useState<Components.Schemas.DeferredOilProductionDto>()

    const [startYear, setStartYear] = useState<number>(2020)
    const [endYear, setEndYear] = useState<number>(2030)
    const [tableYears, setTableYears] = useState<[number, number]>([2020, 2030])

    const profilesToHideWithoutValues = ["Deferred oil production", "Deferred gas production"]

    const productionStrategyOptions = {
        0: "Depletion",
        1: "Water injection",
        2: "Gas injection",
        3: "WAG",
        4: "Mixed",
    }

    const artificialLiftOptions = {
        0: "No lift",
        1: "Gas lift",
        2: "Electrical submerged pumps",
        3: "Subsea booster pumps",
    }

    const datePickerValue = (() => {
        if (project?.physicalUnit === 1) {
            return "Oil field"
        } if (project?.physicalUnit === 0) {
            return "SI"
        }
        return ""
    })()

    const gridRef = useRef<any>(null)

    const updateAndSetDraiangeStrategy = (drainage: Components.Schemas.DrainageStrategyWithProfilesDto) => {
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
            || fuelFlaringAndLossesOverride === undefined
            || deferredGas === undefined
            || deferredOil === undefined) {
            return
        }
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainage }
        newDrainageStrategy.netSalesGas = netSalesGas
        newDrainageStrategy.netSalesGasOverride = netSalesGasOverride
        newDrainageStrategy.fuelFlaringAndLosses = fuelFlaringAndLosses
        newDrainageStrategy.fuelFlaringAndLossesOverride = fuelFlaringAndLossesOverride
        newDrainageStrategy.productionProfileGas = gas
        newDrainageStrategy.productionProfileOil = oil
        newDrainageStrategy.productionProfileWater = water
        newDrainageStrategy.productionProfileNGL = nGL
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        newDrainageStrategy.deferredGasProduction = deferredGas
        newDrainageStrategy.deferredOilProduction = deferredOil

        newDrainageStrategy.importedElectricityOverride = importedElectricityOverride
        setDrainageStrategy(newDrainageStrategy)
    }

    const handleCaseFacilitiesAvailabilityChange = (value: number): void => {
        const newCase = { ...projectCaseEdited }
        const newfacilitiesAvailability = value > 0
            ? Math.min(Math.max(value, 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        setProjectCaseEdited(newCase as Components.Schemas.CaseDto)
    }

    const handleDrainageStrategyGasSolutionChange: ChangeEventHandler<HTMLSelectElement> = async (e) => {
        if ([0, 1].indexOf(Number(e.currentTarget.value)) !== -1) {
            const newGasSolution: Components.Schemas.GasSolution = Number(e.currentTarget.value) as Components.Schemas.GasSolution
            const newDrainageStrategy = { ...drainageStrategy }
            newDrainageStrategy.gasSolution = newGasSolution
            updateAndSetDraiangeStrategy(newDrainageStrategy)
        }
    }

    const gasSolutionOptions = {
        0: "Export",
        1: "Injection",
    }
    const timeSeriesData: ITimeSeriesData[] = [
        {
            profileName: "Oil production",
            unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setOil,
            profile: oil,
        },
        {
            profileName: "Gas production",
            unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            set: setGas,
            profile: gas,
        },
        {
            profileName: "Water production",
            unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setWater,
            profile: water,
        },
        {
            profileName: "Water injection",
            unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setWaterInjection,
            profile: waterInjection,
        },
        {
            profileName: "Fuel, flaring and losses",
            unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            profile: fuelFlaringAndLosses,
            overridable: true,
            overrideProfile: fuelFlaringAndLossesOverride,
            overrideProfileSet: setFuelFlaringAndLossesOverride,
        },
        {
            profileName: "Net sales gas",
            unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
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
        {
            profileName: "Deferred oil production",
            unit: `${project?.physicalUnit === 0 ? "MSm³/yr" : "mill bbls/yr"}`,
            set: setDeferredOil,
            profile: deferredOil,
        },
        {
            profileName: "Deferred gas production",
            unit: `${project?.physicalUnit === 0 ? "GSm³/yr" : "Bscf/yr"}`,
            set: setDeferredGas,
            profile: deferredGas,
        },
    ]

    const handleTableYearsClick = () => {
        setTableYears([startYear, endYear])
    }

    const productionProfilesChartData = () => {
        const dataArray: object[] = []
        if (projectCase?.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                oilProduction: setValueToCorrespondingYear(oil, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
                gasProduction: setValueToCorrespondingYear(gas, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
                waterProduction: setValueToCorrespondingYear(water, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    const injectionProfilesChartData = () => {
        const dataArray: object[] = []
        if (projectCase?.dG4Date === undefined) { return dataArray }
        for (let i = startYear; i <= endYear; i += 1) {
            dataArray.push({
                year: i,
                waterInjection:
                    setValueToCorrespondingYear(waterInjection, i, startYear, new Date(projectCase?.dG4Date).getFullYear()),
            })
        }
        return dataArray
    }

    useEffect(() => {
        (async () => {
            try {
                if (activeTabCase === 1 && projectCase?.dG4Date !== undefined) {
                    setFuelFlaringAndLosses(drainageStrategy.fuelFlaringAndLosses)
                    setNetSalesGas(drainageStrategy.netSalesGas)
                    setImportedElectricity(drainageStrategy.importedElectricity)

                    SetTableYearsFromProfiles([
                        drainageStrategy.netSalesGas,
                        drainageStrategy.fuelFlaringAndLosses,
                        drainageStrategy.netSalesGasOverride,
                        drainageStrategy.fuelFlaringAndLossesOverride,
                        drainageStrategy.productionProfileGas,
                        drainageStrategy.productionProfileOil,
                        drainageStrategy.productionProfileWater,
                        drainageStrategy.productionProfileNGL,
                        drainageStrategy.productionProfileWaterInjection,
                        drainageStrategy.importedElectricityOverride,
                        drainageStrategy.co2EmissionsOverride,
                        drainageStrategy.deferredGasProduction,
                        drainageStrategy.deferredOilProduction,
                    ], new Date(projectCase?.dG4Date).getFullYear(), setStartYear, setEndYear, setTableYears)
                    setGas(drainageStrategy.productionProfileGas)
                    setOil(drainageStrategy.productionProfileOil)
                    setWater(drainageStrategy.productionProfileWater)
                    setNGL(drainageStrategy.productionProfileNGL)
                    setWaterInjection(drainageStrategy.productionProfileWaterInjection)

                    setImportedElectricityOverride(drainageStrategy.importedElectricityOverride)
                    setNetSalesGasOverride(drainageStrategy.netSalesGasOverride)
                    setFuelFlaringAndLossesOverride(drainageStrategy.fuelFlaringAndLossesOverride)

                    setDeferredGas(drainageStrategy.deferredGasProduction)
                    setDeferredOil(drainageStrategy.deferredOilProduction)
                }
            } catch (error) {
                console.error("[CaseView] Error while generating cost profile", error)
            }
        })()
    }, [activeTabCase])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!oil) { return }
        newDrainageStrategy.productionProfileOil = oil
        setDrainageStrategy(newDrainageStrategy)
    }, [oil])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!gas) { return }
        newDrainageStrategy.productionProfileGas = gas
        setDrainageStrategy(newDrainageStrategy)
    }, [gas])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!water) { return }
        newDrainageStrategy.productionProfileWater = water
        setDrainageStrategy(newDrainageStrategy)
    }, [water])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!waterInjection) { return }
        newDrainageStrategy.productionProfileWaterInjection = waterInjection
        setDrainageStrategy(newDrainageStrategy)
    }, [waterInjection])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!importedElectricityOverride) { return }
        newDrainageStrategy.importedElectricityOverride = importedElectricityOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [importedElectricityOverride])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!fuelFlaringAndLossesOverride) { return }
        newDrainageStrategy.fuelFlaringAndLossesOverride = fuelFlaringAndLossesOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [fuelFlaringAndLossesOverride])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!netSalesGasOverride) { return }
        newDrainageStrategy.netSalesGasOverride = netSalesGasOverride
        setDrainageStrategy(newDrainageStrategy)
    }, [netSalesGasOverride])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!deferredOil) { return }
        newDrainageStrategy.deferredOilProduction = deferredOil
        setDrainageStrategy(newDrainageStrategy)
    }, [deferredOil])

    useEffect(() => {
        const newDrainageStrategy: Components.Schemas.DrainageStrategyWithProfilesDto = { ...drainageStrategy }
        if (!deferredGas) { return }
        newDrainageStrategy.deferredGasProduction = deferredGas
        setDrainageStrategy(newDrainageStrategy)
    }, [deferredGas])

    useEffect(() => {
        if (gridRef.current && gridRef.current.api && gridRef.current.api.refreshCells) {
            gridRef.current.api.refreshCells()
        }
    }, [fuelFlaringAndLosses, netSalesGas, importedElectricity])

    if (activeTabCase !== 1) { return null }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    objectKey={projectCase?.facilitiesAvailability}
                    label="Facilities availability"
                    onSubmit={handleCaseFacilitiesAvailabilityChange}
                    value={projectCase?.facilitiesAvailability
                        ? projectCase.facilitiesAvailability * 100
                        : undefined}
                    integer={false}
                    unit="%"
                    min={0}
                    max={100}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableDropdownInput
                    value={drainageStrategy.gasSolution}
                    options={gasSolutionOptions}
                    objectKey={drainageStrategy.gasSolution}
                    label="Gas solution"
                    onSubmit={handleDrainageStrategyGasSolutionChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={productionStrategyOptions[projectCase?.productionStrategyOverview]}
                    label="Production strategy overview"
                >
                    <NativeSelect
                        id="productionStrategy"
                        label=""
                        onChange={() => { }}
                        disabled
                        value={projectCase?.productionStrategyOverview}
                    >
                        {Object.entries(productionStrategyOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={artificialLiftOptions[projectCase?.artificialLift]}
                    label="Artificial lift"
                >
                    <NativeSelect
                        id="artificialLift"
                        label=""
                        onChange={() => { }}
                        disabled
                        value={projectCase?.artificialLift}
                    >
                        {Object.entries(artificialLiftOptions).map(([value, label]) => (
                            <option key={value} value={value}>{label}</option>
                        ))}
                    </NativeSelect>
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    objectKey={projectCase?.producerCount}
                    label="Oil producer wells"
                    onSubmit={() => { }}
                    value={projectCase?.producerCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    objectKey={projectCase?.waterInjectorCount}
                    label="Water injector wells"
                    onSubmit={() => { }}
                    value={projectCase?.waterInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <SwitchableNumberInput
                    objectKey={projectCase?.gasInjectorCount}
                    label="Gas injector wells"
                    onSubmit={() => { }}
                    value={projectCase?.gasInjectorCount}
                    integer
                    disabled
                />
            </Grid>
            <DateRangePicker
                setStartYear={setStartYear}
                setEndYear={setEndYear}
                startYear={startYear}
                endYear={endYear}
                labelText="Units"
                labelValue={datePickerValue}
                handleTableYearsClick={handleTableYearsClick}
            />
            <Grid item xs={12}>
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
            </Grid>
            {
                (waterInjection?.values && waterInjection.values?.length > 0)
                && (
                    <Grid item xs={12}>
                        <AgChartsTimeseries
                            data={injectionProfilesChartData()}
                            chartTitle="Injection profiles"
                            barColors={["#A8CED1"]}
                            barProfiles={["waterInjection"]}
                            barNames={["Water injection"]}
                            unit="MSm3"
                        />
                    </Grid>
                )
            }
            <Grid item xs={12}>
                <CaseTabTable
                    timeSeriesData={timeSeriesData}
                    dg4Year={projectCase?.dG4Date ? new Date(projectCase?.dG4Date).getFullYear() : 2030}
                    tableYears={tableYears}
                    tableName="Production profiles"
                    includeFooter={false}
                    gridRef={gridRef}
                    profilesToHideWithoutValues={profilesToHideWithoutValues}
                />
            </Grid>
        </Grid>
    )
}

export default CaseProductionProfilesTab
