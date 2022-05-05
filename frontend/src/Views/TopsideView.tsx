import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"
import { GetArtificialLiftName, TimeSeriesYears } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import Maturity from "../Components/Maturity"
import NumberInput from "../Components/NumberInput"
import Unit from "../Components/Unit"

const TopsideView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
    const params = useParams()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()
    const [oilCapacity, setOilCapacity] = useState<number | undefined>()
    const [oilCapacityUnit, setOilCapacityUnit] = useState<Components.Schemas.Unit | undefined>()
    const [gasCapacity, setGasCapacity] = useState<number | undefined>()
    const [gasCapacityUnit, setGasCapacityUnit] = useState<Components.Schemas.Unit | undefined>()
    const [dryweight, setDryweight] = useState<number | undefined>()
    const [dryweightUnit, setDryweightUnit] = useState<Components.Schemas.Unit | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()

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
                let newTopside = project.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    newTopside.artificialLift = caseResult?.artificialLift
                    setTopside(newTopside)
                }
                setTopsideName(newTopside?.name!)
                setDryweight(newTopside?.dryWeight)
                setDryweightUnit(newTopside?.dryWeightUnit)
                setOilCapacity(newTopside?.oilCapacity)
                setOilCapacityUnit(newTopside?.oilCapacityUnit)
                setGasCapacity(newTopside?.gasCapacity)
                setGasCapacityUnit(newTopside?.gasCapacityUnit)
                setMaturity(newTopside?.maturity ?? undefined)

                TimeSeriesYears(
                    newTopside,
                    caseResult!.DG4Date!.getFullYear(),
                    setEarliestTimeSeriesYear,
                    setLatestTimeSeriesYear,
                )
            }
        })()
    }, [project])

    useEffect(() => {
        if (topside !== undefined) {
            const newTopside: Topside = { ...topside }
            newTopside.dryWeight = dryweight
            newTopside.dryWeightUnit = dryweightUnit
            newTopside.oilCapacity = oilCapacity
            newTopside.oilCapacityUnit = oilCapacityUnit
            newTopside.gasCapacity = gasCapacity
            newTopside.gasCapacityUnit = gasCapacityUnit
            newTopside.maturity = maturity
            setTopside(newTopside)
        }
    }, [dryweight, dryweightUnit, oilCapacity, oilCapacityUnit, gasCapacity, gasCapacityUnit, maturity])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Topside</Typography>
            <AssetName
                setName={setTopsideName}
                name={topsideName}
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
                        defaultValue={GetArtificialLiftName(topside?.artificialLift)}
                    />
                </WrapperColumn>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setDryweight}
                    value={dryweight ?? 0}
                    integer
                    label="Topside dry weight"
                />
                <Unit
                    setUnit={setDryweightUnit}
                    currentValue={dryweightUnit}
                    setHasChanges={setHasChanges}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilCapacity}
                    value={oilCapacity ?? 0}
                    integer={false}
                    label="Capacity oil"
                />
                <Unit
                    setUnit={setOilCapacityUnit}
                    currentValue={oilCapacityUnit}
                    setHasChanges={setHasChanges}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasCapacity}
                    value={gasCapacity ?? 0}
                    integer={false}
                    label="Capacity gas"
                />
                <Unit
                    setUnit={setGasCapacityUnit}
                    currentValue={gasCapacityUnit}
                    setHasChanges={setHasChanges}
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setTopside}
                setHasChanges={setHasChanges}
                asset={topside}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={topsideName}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setTopside}
                setHasChanges={setHasChanges}
                asset={topside}
                timeSeriesType={TimeSeriesEnum.topsideCessationCostProfileDto}
                assetName={topsideName}
                timeSeriesTitle="Cessation Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <Save
                name={topsideName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setTopside}
                setProject={setProject}
                asset={topside!}
                assetService={GetTopsideService()}
                assetType={AssetTypeEnum.topsides}
            />
        </AssetViewDiv>
    )
}

export default TopsideView
