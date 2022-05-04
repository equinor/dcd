import {
    Input, Typography,
} from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import AssetName from "../Components/AssetName"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Transport } from "../models/assets/transport/Transport"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"
import { TimeSeriesYears } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import Unit from "../Components/Unit"

const TransportView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()
    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
    const params = useParams()
    const [earliestTimeSeriesYear, setEarliestTimeSeriesYear] = useState<number>()
    const [latestTimeSeriesYear, setLatestTimeSeriesYear] = useState<number>()
    const [gasExportPipelineLength, setGasExportPipelineLength] = useState<number | undefined>()
    const [oilExportPipelineLength, setOilExportPipelineLength] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [unitOil, setUnitOil] = useState<Components.Schemas.Unit | undefined>()
    const [unitGas, setUnitGas] = useState<Components.Schemas.Unit | undefined>()

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
                let newTransport = project.transports.find((s) => s.id === params.transportId)
                if (newTransport !== undefined) {
                    setTransport(newTransport)
                } else {
                    newTransport = new Transport()
                    setTransport(newTransport)
                }
                setTransportName(newTransport?.name!)
                setGasExportPipelineLength(newTransport?.gasExportPipelineLength)
                setOilExportPipelineLength(newTransport?.oilExportPipelineLength)
                setUnitGas(newTransport?.gasExportPipelineLengthUnit)
                setUnitOil(newTransport?.oilExportPipelineLengthUnit)
                setMaturity(newTransport?.maturity ?? undefined)

                TimeSeriesYears(
                    newTransport,
                    caseResult!.DG4Date!.getFullYear(),
                    setEarliestTimeSeriesYear,
                    setLatestTimeSeriesYear,
                )
            }
        })()
    }, [project])

    useEffect(() => {
        if (transport !== undefined) {
            const newTransport: Transport = { ...transport }
            newTransport.gasExportPipelineLength = gasExportPipelineLength
            newTransport.oilExportPipelineLength = oilExportPipelineLength
            newTransport.gasExportPipelineLengthUnit = unitGas
            newTransport.oilExportPipelineLengthUnit = unitOil
            newTransport.maturity = maturity
            setTransport(newTransport)
        }
    }, [gasExportPipelineLength, oilExportPipelineLength, maturity, unitGas, unitOil])

    return (
        <AssetViewDiv>
            <Typography variant="h2">Transport</Typography>
            <AssetName
                setName={setTransportName}
                name={transportName}
                setHasChanges={setHasChanges}
            />
            <Wrapper>
                <Typography variant="h4">DG3</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG3Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
                <Typography variant="h4">DG4</Typography>
                <Dg4Field>
                    <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
                </Dg4Field>
            </Wrapper>
            <Wrapper>
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setGasExportPipelineLength}
                    value={gasExportPipelineLength ?? 0}
                    integer
                    label="Length of gas export pipeline"
                />
                <Unit
                    setUnit={setUnitGas}
                    currentValue={unitGas}
                    setHasChanges={setHasChanges}
                />
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilExportPipelineLength}
                    value={oilExportPipelineLength ?? 0}
                    integer
                    label="Length of oil export pipeline"
                />
                <Unit
                    setUnit={setUnitOil}
                    currentValue={unitOil}
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
                setAsset={setTransport}
                setHasChanges={setHasChanges}
                asset={transport}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={transportName}
                timeSeriesTitle="Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <TimeSeries
                caseItem={caseItem}
                setAsset={setTransport}
                setHasChanges={setHasChanges}
                asset={transport}
                timeSeriesType={TimeSeriesEnum.transportCessationCostProfileDto}
                assetName={transportName}
                timeSeriesTitle="Cessation Cost profile"
                earliestYear={earliestTimeSeriesYear!}
                latestYear={latestTimeSeriesYear!}
                setEarliestYear={setEarliestTimeSeriesYear!}
                setLatestYear={setLatestTimeSeriesYear}
            />
            <Save
                name={transportName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setTransport}
                setProject={setProject}
                asset={transport!}
                assetService={GetTransportService()}
                assetType={AssetTypeEnum.transports}
            />
        </AssetViewDiv>
    )
}

export default TransportView
