import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useParams,
} from "react-router"
import Save from "../Components/Save"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Transport } from "../models/assets/transport/Transport"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"
import {
    AssetHeader, AssetViewDiv, Dg4Field, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"

const TransportView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()
    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
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
                let newTransport = project.transports.find((s) => s.id === params.transportId)
                if (newTransport !== undefined) {
                    setTransport(newTransport)
                } else {
                    newTransport = new Transport()
                    setTransport(newTransport)
                }
                setTransportName(newTransport?.name!)
            }
        })()
    }, [project])

    const handleTransportNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setTransportName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Transport</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="transportName" label="Name" />
                    <Input
                        id="transportName"
                        name="transportName"
                        placeholder="Enter Transport name"
                        value={transportName}
                        onChange={handleTransportNameFieldChange}
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
                setAsset={setTransport}
                setHasChanges={setHasChanges}
                asset={transport}
                timeSeriesType={TimeSeriesEnum.costProfile}
                assetName={transportName}
                timeSeriesTitle="Cost profile"
            />
            <Save
                name={transportName}
                setHasChanges={setHasChanges}
                hasChanges={hasChanges}
                setAsset={setTransport}
                setProject={setProject}
                asset={transport!}
                assetService={GetTransportService()}
            />
        </AssetViewDiv>
    )
}

export default TransportView
