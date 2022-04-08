import {
    Button, Input, Label, Typography,
} from "@equinor/eds-core-react"
import { ChangeEventHandler, useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import { Transport } from "../models/assets/transport/Transport"
import { TransportCostProfile } from "../models/assets/transport/TransportCostProfile"
import { TransportCessationCostProfile } from "../models/assets/transport/TransportCessationCostProfile"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTransportService } from "../Services/TransportService"

const AssetHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const AssetViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: row;
`

const WrapperColumn = styled.div`
    display: flex;
    flex-direction: column;
`

const ImportButton = styled(Button)`
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
    }
`

const SaveButton = styled(Button)`
    margin-top: 5rem;
    margin-left: 2rem;
    &:disabled {
        margin-left: 2rem;
        margin-top: 5rem;
    }
`

const Dg4Field = styled.div`
    margin-left: 1rem;
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

const TransportView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [transport, setTransport] = useState<Transport>()

    //Cost Profiel
    const [costProfileColumns, setCostProfileColumns] = useState<string[]>([""])
    const [costProfileGridData, setCostProfileGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)
    
    //CessasionCostProfile
    const [cessationCostProfileColumns, setCessationCostProfileColumns] = useState<string[]>([""])
    const [cessationCostProfileGridData, setCessationCostProfileGridData] = useState<CellValue[][]>([[]])
    const [cessationCostProfileDialogOpen, setCessationCostProfileDialogOpen] = useState(false)

    const [hasChanges, setHasChanges] = useState(false)
    const [transportName, setTransportName] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    const emptyGuid = "00000000-0000-0000-0000-000000000000"

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
                const newCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newTransport?.costProfile)
                const newCessastionCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newTransport?.transportCessationCostProfileDto)

                setCostProfileColumns(newCostProfileColumnTitles)
                setCessationCostProfileColumns(newCessastionCostProfileColumnTitles)

                const newCostProfileGridData = buildGridData(newTransport?.costProfile)
                const newCessationCostProfileGridData = buildGridData(newTransport?.transportCessationCostProfileDto)
                
                setCostProfileGridData(newCostProfileGridData)
                setCessationCostProfileGridData(newCessationCostProfileGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCostProfileCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCostProfileGridData = replaceOldData(costProfileGridData, changes)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileColumns(getColumnAbsoluteYears(caseItem, transport?.costProfile))
        setHasChanges(true)
    }

    const onCessationCostProfileCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCessationCostProfileGridData = replaceOldData(cessationCostProfileGridData, changes)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileColumns(getColumnAbsoluteYears(caseItem, transport?.transportCessationCostProfileDto))
        setHasChanges(true)
    }

    const updateInsertTransportCostProfile = (input: string, year: number) => {
        const newTransport = new Transport(transport!)
        const newCostProfile = new TransportCostProfile()
        newTransport.id = newTransport.id ?? emptyGuid
        newTransport.costProfile = newTransport.costProfile ?? newCostProfile
        newTransport.costProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newTransport.costProfile!.startYear = year
        newTransport.costProfile!.epaVersion = newTransport.costProfile.epaVersion ?? ""
        return newTransport
    }

    const updateInsertTransportCessationCostProfile = (input: string, year: number) => {
        const newTransport = new Transport(transport!)
        const newCessationCostProfile = new TransportCessationCostProfile()
        newTransport.id = newTransport.id ?? emptyGuid
        newTransport.transportCessationCostProfileDto = newTransport.transportCessationCostProfileDto ?? newCessationCostProfile
        newTransport.transportCessationCostProfileDto!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newTransport.transportCessationCostProfileDto!.startYear = year
        newTransport.transportCessationCostProfileDto!.epaVersion = newTransport.transportCessationCostProfileDto.epaVersion ?? ""
        return newTransport
    }

    const onCostProfileImport = (input: string, year: number) => {
        const newTransport = updateInsertTransportCostProfile(input, year)
        setTransport(newTransport)
        const newCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, newTransport?.costProfile)
        setCostProfileColumns(newCostProfileColumnTitles)
        const newCostProfileGridData = buildGridData(newTransport?.costProfile)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        setHasChanges(true)
    }

    const onCessationCostProfileImport = (input: string, year: number) => {
        const newTransport = updateInsertTransportCessationCostProfile(input, year)
        setTransport(newTransport)
        const newCessationCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, newTransport?.transportCessationCostProfileDto)
        setCessationCostProfileColumns(newCessationCostProfileColumnTitles)
        const newCessationCostProfileGridData = buildGridData(newTransport?.transportCessationCostProfileDto)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileDialogOpen(!cessationCostProfileDialogOpen)
        setHasChanges(true)
    }
    
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
        if (e.target.value !== undefined && e.target.value !== "" && e.target.value !== transport?.name) {
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
                        defaultValue={transport?.name}
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
            <Wrapper>
                <Typography variant="h4">Cost profile</Typography>
                <ImportButton onClick={() => { setCostProfileDialogOpen(true) }}>Import</ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={costProfileColumns} gridData={costProfileGridData} onCellsChanged={onCostProfileCellsChanged} />
            </WrapperColumn>
            {!costProfileDialogOpen ? null
                : <Import onClose={() => { setCostProfileDialogOpen(!costProfileDialogOpen) }} onImport={onCostProfileImport} />}

            <Wrapper>
                <Typography variant="h4">Cessation Cost profile</Typography>
                <ImportButton onClick={() => { setCessationCostProfileDialogOpen(true) }}>Import</ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={cessationCostProfileColumns} gridData={cessationCostProfileGridData} onCellsChanged={onCessationCostProfileCellsChanged} />
            </WrapperColumn>
            {!cessationCostProfileDialogOpen ? null
                : <Import onClose={() => { setCessationCostProfileDialogOpen(!cessationCostProfileDialogOpen) }} onImport={onCessationCostProfileImport} />}


            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TransportView
