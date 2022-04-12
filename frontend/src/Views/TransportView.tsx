import {
    Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import TimeSeries from "../Components/TimeSeries"
import TimeSeriesEnum from "../models/assets/TimeSeriesEnum"
import { Transport } from "../models/assets/transport/Transport"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"
import { emptyGuid } from "../Utils/constants"
import {
    AssetHeader, AssetViewDiv, Dg4Field, SaveButton, Wrapper, WrapperColumn,
} from "./Asset/StyledAssetComponents"

const TransportView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()
    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
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
                let newTransport = project!.transports.find((s) => s.id === params.transportId)
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

    const handleSave = async () => {
        const transportDto = new Transport(transport!)
        transportDto.name = transportName
        if (transport?.id === emptyGuid) {
            transportDto.projectId = params.projectId
            const newProject = await GetTransportService().createTransport(params.caseId!, transportDto!)
            const newTransport = newProject.transports.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newTransport!.id!)
            navigate(`${newUrl}`, { replace: true })
            setProject(newProject)
        } else {
            const newProject = await GetTransportService().updateTransport(transportDto!)
            setProject(newProject)
        }
        setHasChanges(false)
    }

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
            {" "}
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TransportView
