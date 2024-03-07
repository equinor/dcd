import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
    useState,
    useEffect,
    useRef,
} from "react"
import { Button, NativeSelect } from "@equinor/eds-core-react"
import CaseNumberInput from "../../Input/CaseNumberInput"
import CaseTabTable from "../Components/CaseTabTable"
import { ITimeSeries } from "../../../Models/ITimeSeries"
import { SetTableYearsFromProfiles } from "../Components/CaseTabTableHelper"
import { AgChartsTimeseries, setValueToCorrespondingYear } from "../../AgGrid/AgChartsTimeseries"
import { ITimeSeriesOverride } from "../../../Models/ITimeSeriesOverride"
import InputSwitcher from "../../Input/InputSwitcher"
import Grid from "@mui/material/Grid"
import { useProjectContext } from "../../../Context/ProjectContext"
import { useCaseContext } from "../../../Context/CaseContext"

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
    drainageStrategy: Components.Schemas.DrainageStrategyDto,
    setDrainageStrategy: Dispatch<SetStateAction<Components.Schemas.DrainageStrategyDto | undefined>>,

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
    const { projectCase, projectCaseEdited, setProjectCaseEdited, activeTabCase } = useCaseContext()
    if (!projectCase) return (<></>)
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
        const newCase = { ...projectCaseEdited }
        const newfacilitiesAvailability = e.currentTarget.value.length > 0
            ? Math.min(Math.max(Number(e.currentTarget.value), 0), 100) : undefined
        if (newfacilitiesAvailability !== undefined) {
            newCase.facilitiesAvailability = newfacilitiesAvailability / 100
        } else { newCase.facilitiesAvailability = 0 }
        newCase ?? setProjectCaseEdited(newCase)
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

                    SetTableYearsFromProfiles([drainageStrategy.netSalesGas, drainageStrategy.fuelFlaringAndLosses,
                    drainageStrategy.netSalesGasOverride, drainageStrategy.fuelFlaringAndLossesOverride,
                    drainageStrategy.productionProfileGas, drainageStrategy.productionProfileOil,
                    drainageStrategy.productionProfileWater, drainageStrategy.productionProfileNGL,
                    drainageStrategy.productionProfileWaterInjection, drainageStrategy.importedElectricityOverride,
                    drainageStrategy.co2EmissionsOverride,
                    ], new Date(projectCase?.dG4Date).getFullYear(), setStartYear, setEndYear, setTableYears)
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
    }, [activeTabCase])

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

    if (activeTabCase !== 1) { return null }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={projectCase?.facilitiesAvailability !== undefined
                        ? `${projectCase?.facilitiesAvailability * 100}%` : ""}
                    label="Facilities availability"
                >
                    <CaseNumberInput
                        onChange={handleCaseFacilitiesAvailabilityChange}
                        defaultValue={projectCase?.facilitiesAvailability
                            !== undefined ? projectCase?.facilitiesAvailability * 100 : undefined}
                        integer={false}
                        unit="%"
                        min={0}
                        max={100}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    value={drainageStrategy?.gasSolution === 0 ? "Export" : "Injection"}
                    label="Gas solution"
                >
                    <NativeSelect
                        id="gasSolution"
                        label=""
                        onChange={handleDrainageStrategyGasSolutionChange}
                        value={drainageStrategy?.gasSolution}
                    >
                        <option key={0} value={0}>Export</option>
                        <option key={1} value={1}>Injection</option>
                    </NativeSelect>
                </InputSwitcher>
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
                <InputSwitcher
                    label="Oil producer wells"
                    value={projectCase?.producerCount.toString()}
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={projectCase?.producerCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    label="Water injector wells"
                    value={projectCase?.waterInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={projectCase?.waterInjectorCount}
                        integer
                        disabled
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={3}>
                <InputSwitcher
                    label="Gas injector wells"
                    value={projectCase?.gasInjectorCount.toString()}
                >
                    <CaseNumberInput
                        onChange={() => { }}
                        defaultValue={projectCase?.gasInjectorCount}
                        integer
                        disabled
                    />
                </InputSwitcher>

            </Grid>
            <Grid item xs={12} container spacing={1} justifyContent="flex-end" alignItems="flex-end">
                <Grid item>
                    <NativeSelect
                        id="unit"
                        label="Units"
                        onChange={() => { }}
                        value={project?.physicalUnit}
                        disabled
                    >
                        <option key={0} value={0}>SI</option>
                        <option key={1} value={1}>Oil field</option>
                    </NativeSelect>
                </Grid>
                <Grid item>
                    <CaseNumberInput
                        onChange={handleStartYearChange}
                        defaultValue={startYear}
                        integer
                        label="Start year"
                        min={2010}
                    max={2110}
                />
                </Grid>
                <Grid item>
                    <CaseNumberInput
                        onChange={handleEndYearChange}
                        defaultValue={endYear}
                        integer
                        label="End year"
                        min={2010}
                    max={2110}
                />
                </Grid>
                <Grid item>
                    <Button onClick={handleTableYearsClick}>  Apply </Button>
                </Grid>
            </Grid>
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
                && (<Grid item xs={12}>
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
            />
            </Grid>
        </Grid>
    )
}

export default CaseProductionProfilesTab
