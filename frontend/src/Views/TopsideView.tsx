import { Button, Input, Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import {
    useLocation, useNavigate, useParams,
} from "react-router"
import styled from "styled-components"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import Import from "../Components/Import/Import"
import { Topside } from "../models/assets/topside/Topside"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetTopsideService } from "../Services/TopsideService"

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

const TopsideView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [topside, setTopside] = useState<Topside>()
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)
    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()

    const emptyGuid = "00000000-0000-0000-0000-000000000000"

    useEffect(() => {
        (async () => {
            try {
                console.log("UseEffect")
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newTopside = projectResult.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    setTopside(newTopside)
                }
                const newColumnTitles = getColumnAbsoluteYears(caseResult, newTopside?.topsideCostProfile)
                setColumns(newColumnTitles)
                const newGridData = buildGridData(newTopside?.topsideCostProfile)
                setGridData(newGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem, topside?.topsideCostProfile))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        console.log("Before copying")
        console.log(topside)
        const newTopside = Topside.Copy(topside!)
        console.log("After copied")
        console.log(newTopside)
        newTopside.topsideCostProfile!.startYear = year
        newTopside.topsideCostProfile!.epaVersion = ""
        // eslint-disable-next-line max-len
        newTopside.topsideCostProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setTopside(newTopside)
        console.log("After setTopside")
        console.log(newTopside)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, newTopside?.topsideCostProfile)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(newTopside?.topsideCostProfile)
        setGridData(newGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        setHasChanges(true)
        console.log("onImport ending")
        console.log(newTopside.topsideCostProfile)
    }

    const handleSave = async () => {
        const topsideDto = Topside.ToDto(topside!)
        if (topside?.id === emptyGuid) {
            topsideDto.projectId = params.projectId
            const newProject: Project = await GetTopsideService().createTopside(params.caseId!, topsideDto!)
            const newTopside = newProject.topsides.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newTopside!.id!)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setTopside(newTopside)
            setCase(newCase)
            navigate(`${newUrl}`, { replace: true })
        } else {
            topsideDto.projectId = params.projectId
            const newProject = await GetTopsideService().updateTopside(topsideDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newTopside = newProject.topsides.find((s) => s.id === params.topsideId)
            setTopside(newTopside)
        }
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <Typography variant="h2">{topside?.name}</Typography>
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
                <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
            </WrapperColumn>
            {!costProfileDialogOpen ? null
                : <Import onClose={() => { setCostProfileDialogOpen(!costProfileDialogOpen) }} onImport={onImport} />}
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TopsideView
