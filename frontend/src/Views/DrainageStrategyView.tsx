import {
    Input, Typography, Label,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import { DrainageStrategy } from "../models/assets/drainagestrategy/DrainageStrategy"
import { Project } from "../models/Project"
import { Case } from "../models/Case"

import { GetProjectService } from "../Services/ProjectService"
import { GetDrainageStrategyService } from "../Services/DrainageStrategyService"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { EMPTY_GUID } from "../Utils/constants"
import {
    AssetHeader, AssetViewDiv, Dg4Field, SaveButton, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"

const DrainageStrategyView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [drainageStrategy, setDrainageStrategy] = useState<DrainageStrategy>()
    const [drainageStrategyName, setDrainageStrategyName] = useState<string>("")
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()

    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

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
                // eslint-disable-next-line max-len
                setEarliestTimeSeriesYear(Math.min(
                    newDrainage!.co2Emissions!.startYear!,
                    newDrainage!.fuelFlaringAndLosses!.startYear!,
                    newDrainage!.netSalesGas!.startYear!,
                    newDrainage!.productionProfileGas!.startYear!,
                    newDrainage!.productionProfileOil!.startYear!,
                    newDrainage!.productionProfileWater!.startYear!,
                    newDrainage!.productionProfileWaterInjection!.startYear!,
                )
                     + caseResult!.DG4Date!.getFullYear())
                // eslint-disable-next-line max-len
                setLatestTimeSeriesYear(Math.max(
                    newDrainage!.co2Emissions!.startYear!
                     + newDrainage.co2Emissions!.values!.length!,
                     newDrainage!.fuelFlaringAndLosses!.startYear!
                     + newDrainage.fuelFlaringAndLosses!.values!.length!,
                     newDrainage!.netSalesGas!.startYear!
                     + newDrainage.netSalesGas!.values!.length!,
                     newDrainage!.productionProfileGas!.startYear!
                     + newDrainage.productionProfileGas!.values!.length!,
                     newDrainage!.productionProfileOil!.startYear!
                     + newDrainage.productionProfileOil!.values!.length!,
                     newDrainage!.productionProfileWater!.startYear!
                     + newDrainage.productionProfileWater!.values!.length!,
                     newDrainage!.productionProfileWaterInjection!.startYear!
                     + newDrainage.productionProfileWaterInjection!.values!.length!,
                ) + caseResult!.DG4Date!.getFullYear())
            }
        })()
    }, [project])

    const handleSave = async () => {
        const drainageStrategyDto = DrainageStrategy.toDto(drainageStrategy!)
        drainageStrategyDto.name = drainageStrategyName
        if (drainageStrategyDto?.id === EMPTY_GUID) {
            drainageStrategyDto.projectId = params.projectId
            const newProject: Project = await GetDrainageStrategyService()
                .createDrainageStrategy(params.caseId!, drainageStrategyDto!)
            const newDrainageStrategy = newProject.drainageStrategies.at(-1)
            const newUrl = location.pathname.replace(EMPTY_GUID, newDrainageStrategy!.id!)
            navigate(`${newUrl}`)
            setProject(newProject)
        } else {
            const newProject = await GetDrainageStrategyService().updateDrainageStrategy(drainageStrategyDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

    const handleDrainageStrategyNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setDrainageStrategyName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="drainagStrategyName" label="Name" />
                    <Input
                        id="drainagStrategyName"
                        name="drainagStrategyName"
                        placeholder="Enter Drainage Strategy name"
                        value={drainageStrategyName}
                        onChange={handleDrainageStrategyNameFieldChange}
                    />
                </WrapperColumn>
            </AssetHeader>
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
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.fuelFlaringAndLosses}
                assetName={drainageStrategyName}
                timeSeriesTitle="Fuel flaring and losses"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.netSalesGas}
                assetName={drainageStrategyName}
                timeSeriesTitle="Net sales gas"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileGas}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile gas"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileOil}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile oil"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileWater}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile water"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setDrainageStrategy}
                setHasChanges={setHasChanges}
                asset={drainageStrategy}
                timeSeriesType={TimeSeriesEnum.productionProfileWaterInjection}
                assetName={drainageStrategyName}
                timeSeriesTitle="Production profile water injection"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default DrainageStrategyView
