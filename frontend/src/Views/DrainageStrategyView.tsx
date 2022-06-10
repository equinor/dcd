import {
    Input, Typography,
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
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import AssetName from "../Components/AssetName"
import { unwrapCase, unwrapProjectId } from "../Utils/common"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import { NetSalesGas } from "../models/assets/drainagestrategy/NetSalesGas"
import { Co2Emissions } from "../models/assets/drainagestrategy/Co2Emissions"
import { FuelFlaringAndLosses } from "../models/assets/drainagestrategy/FuelFlaringAndLosses"
import { ProductionProfileGas } from "../models/assets/drainagestrategy/ProductionProfileGas"
import { ProductionProfileOil } from "../models/assets/drainagestrategy/ProductionProfileOil"
import { ProductionProfileWater } from "../models/assets/drainagestrategy/ProductionProfileWater"
import { ProductionProfileWaterInjection } from "../models/assets/drainagestrategy/ProductionProfileWaterInjection"
import { ProductionProfileNGL } from "../models/assets/drainagestrategy/ProductionProfileNGL"
import NumberInputInherited from "../Components/NumberInputInherited"
import ArtificialLiftInherited from "../Components/ArtificialLiftInherited"

const DrainageStrategyView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [drainageStrategyName, setDrainageStrategyName] = useState<string>("")
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [netSalesGas, setNetSalesGas] = useState<NetSalesGas>()
    const [co2Emissions, setCo2Emissions] = useState<Co2Emissions>()
    const [fuelFlaringAndLosses, setFuelFlaringAndLosses] = useState<FuelFlaringAndLosses>()
    const [productionProfileGas, setProductionProfileGas] = useState<ProductionProfileGas>()
    const [productionProfileOil, setProductionProfileOil] = useState<ProductionProfileOil>()
    const [productionProfileWater, setProductionProfileWater] = useState<ProductionProfileWater>()
    const [productionProfileNGL, setProductionProfileNGL] = useState<ProductionProfileNGL>()
    // eslint-disable-next-line max-len
    const [productionProfileWaterInjection, setProductionProfileWaterInjection] = useState<ProductionProfileWaterInjection>()
    const [nGLYield, setNGLYield] = useState<number>()
    const [artificialLift, setArtificialLift] = useState<Components.Schemas.ArtificialLift | undefined>()
    const [producerCount, setProducerCount] = useState<number>()
    const [gasInjectorCount, setGasInjectorCount] = useState<number>()
    const [waterInjectorCount, setWaterInjectorCount] = useState<number>()
    const [facilitiesAvailability, setFacilitiesAvailability] = useState<number>()

    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const projectResult: Project = await GetProjectService().getProjectByID(projectId)
                setProject(projectResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [])

    useEffect(() => {
        (async () => {
            if (project !== undefined) {
                const caseResult: Case = unwrapCase(project.cases.find((o) => o.id === params.caseId))
                setCase(caseResult)
                // eslint-disable-next-line max-len
                let newDrainage: DrainageStrategy | undefined = project.drainageStrategies.find((s) => s.id === params.drainageStrategyId)
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
                setArtificialLift(newDrainage.artificialLift)
                setGasInjectorCount(newDrainage?.gasInjectorCount)
                setWaterInjectorCount(newDrainage?.waterInjectorCount)
                setProducerCount(newDrainage?.producerCount)
                setFacilitiesAvailability(newDrainage?.facilitiesAvailability)

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
        newDrainage.artificialLift = artificialLift
        newDrainage.producerCount = producerCount
        newDrainage.gasInjectorCount = gasInjectorCount
        newDrainage.waterInjectorCount = waterInjectorCount
        newDrainage.facilitiesAvailability = facilitiesAvailability
        setDrainageStrategy(newDrainage)

        if (caseItem?.DG4Date) {
            initializeFirstAndLastYear(
                caseItem?.DG4Date?.getFullYear(),
                [netSalesGas, co2Emissions, fuelFlaringAndLosses,
                    productionProfileGas, productionProfileOil,
                    productionProfileWater, productionProfileWaterInjection,
                    productionProfileNGL],
                setFirstTSYear,
                setLastTSYear,
            )
        }
    }, [nGLYield, co2Emissions, netSalesGas, fuelFlaringAndLosses,
        productionProfileGas, productionProfileOil, productionProfileWater, productionProfileWaterInjection,
        productionProfileNGL, artificialLift, producerCount, gasInjectorCount, waterInjectorCount,
        facilitiesAvailability])

    return (
        <AssetViewDiv>
            <Wrapper>
                <Typography variant="h2">Drainage strategy</Typography>
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
            </Wrapper>
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
                    <ArtificialLiftInherited
                        currentValue={artificialLift}
                        setArtificialLift={setArtificialLift}
                        setHasChanges={setHasChanges}
                        caseArtificialLift={caseItem?.artificialLift}
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setValue={setNGLYield}
                    value={nGLYield ?? 0}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label={`NGL yield ${project?.physUnit === 0 ? "(Sm³/MSm³)" : "(bbls/mill scf)"}`}
                />
                <NumberInputInherited
                    value={drainageStrategy?.producerCount ?? 0}
                    setValue={setProducerCount}
                    setHasChanges={setHasChanges}
                    integer
                    label="Producer count"
                    caseValue={caseItem?.producerCount}
                />
                <NumberInputInherited
                    value={drainageStrategy?.gasInjectorCount ?? 0}
                    setValue={setGasInjectorCount}
                    setHasChanges={setHasChanges}
                    integer
                    label="Gas injector count"
                    caseValue={caseItem?.gasInjectorCount}
                />
                <NumberInputInherited
                    value={drainageStrategy?.waterInjectorCount ?? 0}
                    setValue={setWaterInjectorCount}
                    setHasChanges={setHasChanges}
                    integer
                    label="Water injector count"
                    caseValue={caseItem?.waterInjectorCount}
                />
                <NumberInputInherited
                    value={drainageStrategy?.facilitiesAvailability ?? 0}
                    setValue={setFacilitiesAvailability}
                    setHasChanges={setHasChanges}
                    integer={false}
                    label="Facilities availability (%)"
                    caseValue={caseItem?.facilitiesAvailability}
                />
            </Wrapper>
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCo2Emissions}
                setHasChanges={setHasChanges}
                timeSeries={co2Emissions}
                timeSeriesTitle="CO2 emissions (MTPA)"
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
                timeSeriesTitle={`Net sales gas ${project?.physUnit === 0 ? "(GSm³/yr)" : "(Bscf/yr)"}`}
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
                timeSeriesTitle={`Fuel flaring and losses ${project?.physUnit === 0 ? "(GSm³/yr)" : "(Bscf/yr)"}`}
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
                timeSeriesTitle={`Production profile gas ${project?.physUnit === 0 ? "(GSm³/yr)" : "(Bscf/yr)"}`}
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
                timeSeriesTitle={`Production profile oil ${project?.physUnit === 0 ? "(MSm³/yr)" : "(mill bbls/yr)"}`}
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
                timeSeriesTitle={`Production profile water ${project?.physUnit === 0 ? "(MSm³/yr)" : "(mill bbls/yr)"}`}
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
                timeSeriesTitle={`Production profile water injection 
                    ${project?.physUnit === 0 ? "(MSm³/yr)" : "(mill bbls/yr)"}`}
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
                timeSeriesTitle="Production profile NGL (MTPA)"
                firstYear={firstTSYear}
                lastYear={lastTSYear}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
        </AssetViewDiv>
    )
}

export default DrainageStrategyView
