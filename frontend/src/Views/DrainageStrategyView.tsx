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
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"

const DrainageStrategyView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [drainageStrategyName, setDrainageStrategyName] = useState<string>("")

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
                    setDrainageStrategy(newDrainage)
                }
                setDrainageStrategyName(newDrainage?.name!)
            }
        })()
    }, [project])

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
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.co2Emissions}
                assetName={drainageStrategyName}
                timeSeriesTitle="CO2 emissions"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.fuelFlaringAndLosses}
                assetName={drainageStrategyName}
                timeSeriesTitle="Fuel flaring and losses"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.netSalesGas}
                assetName={drainageStrategyName}
                timeSeriesTitle="Net sales gas"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileGas}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile gas"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileOil}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile oil"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileWater}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile water"
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileWaterInjection}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile water injection"
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
