import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { Project } from "../models/Project"
import { Case } from "../models/Case"

import { GetProjectService } from "../Services/ProjectService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import TimeSeries from "../Components/TimeSeries"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import { GetArtificialLiftName, initializeFirstAndLastYear } from "./Asset/AssetHelper"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import { ProductionProfileGasCostProfile } from "../models/assets/drainagestrategy/ProductionProfileGasCostProfile"
import { NetSalesGasCostProfile } from "../models/assets/drainagestrategy/NetSalesGasCostProfile"
import { Co2EmissionsCostProfile } from "../models/assets/drainagestrategy/Co2EmissionsCostProfile"
import { FuelFlaringAndLossesCostProfile } from "../models/assets/drainagestrategy/FuelFlaringAndLossesCostProfile"
import { ProductionProfileOilCostProfile } from "../models/assets/drainagestrategy/ProductionProfileOilCostProfile"
import { ProductionProfileWaterCostProfile } from "../models/assets/drainagestrategy/ProductionProfileWaterCostProfile"
// eslint-disable-next-line max-len
import { ProductionProfileWaterInjectionCostProfile } from "../models/assets/drainagestrategy/ProductionProfileWaterInjectionCostProfile"
import { ProductionProfileNGLCostProfile } from "../models/assets/drainagestrategy/ProductionProfileNGLCostProfile"

const DrainageStrategyView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [drainageStrategyName, setDrainageStrategyName] = useState<string>("")
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [netSalesGas, setNetSalesGas] = useState<NetSalesGasCostProfile>()
    const [co2Emissions, setCo2Emissions] = useState<Co2EmissionsCostProfile>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLossesCostProfile>()
    const [productionProfileGas, setProductionProfileGas] = useState<ProductionProfileGasCostProfile>()
    const [productionProfileOil, setProductionProfileOil] = useState<ProductionProfileOilCostProfile>()
    const [productionProfileWater, setProductionProfileWater] = useState<ProductionProfileWaterCostProfile>()
    const [productionProfileNGL, setProductionProfileNGL] = useState<ProductionProfileNGLCostProfile>()
    // eslint-disable-next-line max-len
    const [productionProfileWaterInjection, setProductionProfileWaterInjection] = useState<ProductionProfileWaterInjectionCostProfile>()
    const [nGLYield, setNGLYield] = useState<number>()

    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult = project.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newDrainage = project!.drainageStrategies.find((s) => s.id === params.drainageStrategyId)
                if (newDrainage !== undefined) {
                    setDrainageStrategy(newDrainage)
                } else {
                    newDrainage = new DrainageStrategy()
                    newDrainage.producerCount = caseResult?.producerCount
                    newDrainage.gasInjectorCount = caseResult?.gasInjectorCount
                    newDrainage.waterInjectorCount = caseResult?.waterInjectorCount
                    newDrainage.artificialLift = caseResult?.artificialLift
                    setDrainageStrategy(newDrainage)
                }
                setDrainageStrategyName(newDrainage?.name!)

                setNGLYield(newDrainage.nglYield)

                setNetSalesGas(newDrainage.netSalesGas)
                setCo2Emissions(newDrainage.co2Emissions)
                setFuelFlaringAndLosses(newDrainage.fuelFlaringAndLosses)
                setProductionProfileGas(newDrainage.productionProfileGas)
                setProductionProfileOil(newDrainage.productionProfileOil)
                setProductionProfileWater(newDrainage.productionProfileWater)
                setProductionProfileWaterInjection(newDrainage.productionProfileWaterInjection)
                setProductionProfileNGL(newDrainage.productionProfileNGL)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newDrainage.netSalesGas, newDrainage.co2Emissions, newDrainage.fuelFlaringAndLosses,
                            newDrainage.productionProfileGas, newDrainage.productionProfileOil,
                            newDrainage.productionProfileWater, newDrainage.productionProfileWaterInjection,
                            newDrainage.productionProfileNGL],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        const newDrainage: DrainageStrategy = { ...drainageStrategy }
        newDrainage.nglYield = nGLYield
        newDrainage.co2Emissions = co2Emissions
        newDrainage.netSalesGas = netSalesGas
        newDrainage.fuelFlaringAndLosses = fuelFlaringAndLosses
        newDrainage.productionProfileGas = productionProfileGas
        newDrainage.productionProfileOil = productionProfileOil
        newDrainage.productionProfileWater = productionProfileWater
        newDrainage.productionProfileWaterInjection = productionProfileWaterInjection
        newDrainage.productionProfileNGL = productionProfileNGL
        setDrainageStrategy(newDrainage)

        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [newDrainage.netSalesGas, newDrainage.co2Emissions, newDrainage.fuelFlaringAndLosses,
                    newDrainage.productionProfileGas, newDrainage.productionProfileOil,
                    newDrainage.productionProfileWater, newDrainage.productionProfileWaterInjection,
                    newDrainage.productionProfileNGL],
                setFirstTSYear,
                setLastTSYear,
            )
        }
    }, [nGLYield, co2Emissions, netSalesGas, fuelFlaringAndLosses,
        productionProfileGas, productionProfileOil, productionProfileWater, productionProfileWaterInjection,
        productionProfileNGL])

    return (
        <AssetViewDiv>
            <AssetName
                setName={setDrainageStrategyName}
                name={drainageStrategyName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <Wrapper>
                <WrapperColumn>
                    <Label htmlFor="name" label="Artificial lift" />
                    <Input
                        id="artificialLift"
                        disabled
                        defaultValue={GetArtificialLiftName(drainageStrategy?.artificialLift)}
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setValue={setNGLYield}
                    value={nGLYield ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label="NGL yield"
                />
                <NumberInput
                    value={drainageStrategy?.producerCount ?? 0}
                    integer
                    disabled
                    label="Producer count"
                />
                <NumberInput
                    value={drainageStrategy?.gasInjectorCount ?? 0}
                    integer
                    disabled
                    label="Gas injector count"
                />
                <NumberInput
                    value={drainageStrategy?.waterInjectorCount ?? 0}
                    integer
                    disabled
                    label="Water injector count"
                />
                <NumberInput
                    value={caseItem?.facilitiesAvailability ?? 0}
                    integer={false}
                    disabled
                    label="Facilities availability"
                />
            </Wrapper>
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCo2Emissions}
                setHasChanges={setHasChanges}
                timeSeries={co2Emissions}
                timeSeriesTitle="CO2 emissions"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setNetSalesGas}
                setHasChanges={setHasChanges}
                timeSeries={netSalesGas}
                timeSeriesTitle="Net sales gas"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setFuelFlaringAndLosses}
                setHasChanges={setHasChanges}
                timeSeries={fuelFlaringAndLosses}
                timeSeriesTitle="Fuel flaring and losses"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setProductionProfileGas}
                setHasChanges={setHasChanges}
                timeSeries={productionProfileGas}
                timeSeriesTitle="Production profile gas"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setProductionProfileOil}
                setHasChanges={setHasChanges}
                timeSeries={productionProfileOil}
                timeSeriesTitle="Production profile oil"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setProductionProfileWater}
                setHasChanges={setHasChanges}
                timeSeries={productionProfileWater}
                timeSeriesTitle="Production profile water"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setProductionProfileWaterInjection}
                setHasChanges={setHasChanges}
                timeSeries={productionProfileWaterInjection}
                timeSeriesTitle="Production profile water"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setProductionProfileNGL}
                setHasChanges={setHasChanges}
                timeSeries={productionProfileNGL}
                timeSeriesTitle="Production profile NGL"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <Save
                name={drainageStrategyName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setDrainageStrategy}
                setProject={setProject}
                asset={drainageStrategy!}
                assetService={GetDrainageStrategyService()}
                assetType={AssetTypeEnum.drainageStrategies}
            />
        </AssetViewDiv>
    )
}

export default DrainageStrategyView
