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
import { Transport } from "../models/assets/transport/Transport"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"
import { initializeFirstAndLastYear } from "./Asset/AssetHelper"
import {
    AssetViewDiv, Dg4Field, Wrapper,
} from "./Asset/StyledAssetComponents"
import AssetTypeEnum from "../models/assets/AssetTypeEnum"
import NumberInput from "../Components/NumberInput"
import Maturity from "../Components/Maturity"
import { TransportCostProfile } from "../models/assets/transport/TransportCostProfile"
import { TransportCessationCostProfile } from "../models/assets/transport/TransportCessationCostProfile"

const TransportView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()
    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
    const params = useParams()
    const [firstTSYear, setFirstTSYear] = useState<number>()
    const [lastTSYear, setLastTSYear] = useState<number>()
    const [gasExportPipelineLength, setGasExportPipelineLength] = useState<number | undefined>()
    const [oilExportPipelineLength, setOilExportPipelineLength] = useState<number | undefined>()
    const [maturity, setMaturity] = useState<Components.Schemas.Maturity | undefined>()
    const [costProfile, setCostProfile] = useState<TransportCostProfile>()
    const [cessationCostProfile, setCessationCostProfile] = useState<TransportCessationCostProfile>()

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
                setMaturity(newTransport?.maturity ?? undefined)

                setCostProfile(newTransport.costProfile)
                setCessationCostProfile(newTransport.cessationCostProfile)

                if (caseResult?.DG4Date) {
                    initializeFirstAndLastYear(
                        caseResult?.DG4Date?.getFullYear(),
                        [newTransport.costProfile, newTransport.cessationCostProfile],
                        setFirstTSYear,
                        setLastTSYear,
                    )
                }
            }
        })()
    }, [project])

    useEffect(() => {
        if (transport !== undefined) {
            const newTransport: Transport = { ...transport }
            newTransport.gasExportPipelineLength = gasExportPipelineLength
            newTransport.oilExportPipelineLength = oilExportPipelineLength
            newTransport.maturity = maturity
            newTransport.costProfile = costProfile
            newTransport.cessationCostProfile = cessationCostProfile

            if (caseItem?.DG4Date) {
                initializeFirstAndLastYear(
                    caseItem?.DG4Date?.getFullYear(),
                    [costProfile, cessationCostProfile],
                    setFirstTSYear,
                    setLastTSYear,
                )
            }
            setTransport(newTransport)
        }
    }, [gasExportPipelineLength, oilExportPipelineLength, maturity])

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
                <NumberInput
                    setHasChanges={setHasChanges}
                    setValue={setOilExportPipelineLength}
                    value={oilExportPipelineLength ?? 0}
                    integer
                    label="Length of oil export pipeline"
                />
            </Wrapper>
            <Maturity
                setMaturity={setMaturity}
                currentValue={maturity}
                setHasChanges={setHasChanges}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={costProfile}
                timeSeriesTitle="Cost profile"
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
            />
            <TimeSeries
                dG4Year={caseItem?.DG4Date?.getFullYear()}
                setTimeSeries={setCessationCostProfile}
                setHasChanges={setHasChanges}
                timeSeries={cessationCostProfile}
                timeSeriesTitle="Cessation cost profile"
                firstYear={firstTSYear!}
                lastYear={lastTSYear!}
                setFirstYear={setFirstTSYear!}
                setLastYear={setLastTSYear}
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
