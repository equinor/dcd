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
import { Substructure } from "../models/assets/substructure/Substructure"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetSubstructureService } from "../Services/SubstructureService"

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

const SubstructureView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()
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
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newSubstructure = projectResult.substructures.find((s) => s.id === params.substructureId)
                if (newSubstructure !== undefined) {
                    setSubstructure(newSubstructure)
                } else {
                    newSubstructure = new Substructure()
                    setSubstructure(newSubstructure)
                }
                const newColumnTitles = getColumnAbsoluteYears(caseResult, newSubstructure?.substructureCostProfile)
                setColumns(newColumnTitles)
                const newGridData = buildGridData(newSubstructure?.substructureCostProfile)
                setGridData(newGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem, substructure?.substructureCostProfile))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newSubstructure = Substructure.Copy(substructure!)
        newSubstructure.substructureCostProfile!.startYear = year
        // eslint-disable-next-line max-len
        newSubstructure.substructureCostProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setSubstructure(newSubstructure)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, newSubstructure?.substructureCostProfile)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(newSubstructure?.substructureCostProfile)
        setGridData(newGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        setHasChanges(true)
    }

    const handleSave = async () => {
        const substructureDto = Substructure.ToDto(substructure!)
        if (substructure?.id === emptyGuid) {
            substructureDto.projectId = params.projectId
            const newProject = await GetSubstructureService().createSubstructure(params.caseId!, substructureDto!)
            const newSubstructure = newProject.substructures.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newSubstructure!.id!)
            navigate(`${newUrl}`, { replace: true })
        } else {
            const newProject = await GetSubstructureService().updateSubstructure(substructureDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newSubstructure = newProject.substructures.find((s) => s.id === params.substructureId)
            setSubstructure(newSubstructure)
        }
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <Typography variant="h2">{substructure?.name}</Typography>
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

export default SubstructureView
