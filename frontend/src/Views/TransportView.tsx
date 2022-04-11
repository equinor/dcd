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
    const [, setProject] = useState<Project>()
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
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newTransport = projectResult.transports.find((s) => s.id === params.transportId)
                if (newTransport !== undefined) {
                    setTransport(newTransport)
                } else {
                    newTransport = new Transport()
                    setTransport(newTransport)
                }
                setTransportName(newTransport.name!)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const handleSave = async () => {
        const transportDto = new Transport(transport!)
        transportDto.name = transportName
        if (transport?.id === emptyGuid) {
            transportDto.projectId = params.projectId
            const updatedProject = await GetTransportService().createTransport(params.caseId!, transportDto!)
            const updatedCase = updatedProject.cases.find((c) => c.id === params.caseId)
            const newTransport = updatedProject.transports.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newTransport!.id!)
            setProject(updatedProject)
            setCase(updatedCase)
            setTransport(newTransport)
            navigate(`${newUrl}`, { replace: true })
        } else {
            transportDto.projectId = params.projectId
            const newProject = await GetTransportService().updateTransport(transportDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((c) => c.id === params.caseId)
            setCase(newCase)
            const newTransport = newProject.transports.find((t) => t.id === params.transportId)
            setTransport(newTransport)
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
                timeSeries={transport?.costProfile}
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
