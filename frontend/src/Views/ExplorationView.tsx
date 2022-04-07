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
import { Exploration } from "../models/assets/exploration/Exploration"

import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"
import { GetExplorationService } from "../Services/ExplorationService"
import { ExplorationCostProfile } from "../models/assets/exploration/ExplorationCostProfile"

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

const ExplorationView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [exploration, setExploration] = useState<Exploration>()
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)
    const [hasChanges, setHasChanges] = useState(false)
    const [name, setName] = useState<string>("")
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
                let newExploration = projectResult.explorations.find((s) => s.id === params.explorationId)
                if (newExploration !== undefined) {
                    setExploration(newExploration)
                } else {
                    newExploration = new Exploration()
                    setExploration(newExploration)
                }
                setName(newExploration?.name!)
                const newColumnTitles = getColumnAbsoluteYears(caseResult, newExploration?.costProfile)
                setColumns(newColumnTitles)
                const newGridData = buildGridData(newExploration?.costProfile)
                setGridData(newGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnAbsoluteYears(caseItem, exploration?.costProfile))
        setHasChanges(true)
    }

    const onImport = (input: string, year: number) => {
        const newExploration = new Exploration(exploration!)
        if (newExploration.costProfile === undefined) {
            newExploration.costProfile = new ExplorationCostProfile()
        }
        newExploration.costProfile!.startYear = year
        // eslint-disable-next-line max-len
        newExploration.costProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        setExploration(newExploration)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, newExploration?.costProfile)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(newExploration?.costProfile)
        setGridData(newGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        if (name !== "") {
            setHasChanges(true)
        }
    }

    const handleSave = async () => {
        const explorationDto = new Exploration(exploration!)
        explorationDto.name = name
        if (exploration?.id === emptyGuid) {
            explorationDto.projectId = params.projectId
            const newProject = await GetExplorationService().createExploration(params.caseId!, explorationDto!)
            const newExploration = newProject.explorations.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newExploration!.id!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            setExploration(newExploration)
            navigate(`${newUrl}`, { replace: true })
        } else {
            const newProject = await GetExplorationService().updateExploration(explorationDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newExploration = newProject.explorations.find((s) => s.id === params.explorationId)
            setExploration(newExploration)
        }
        setHasChanges(false)
    }

    const handleNameChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "" && e.target.value !== exploration?.name) {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    const deleteCostProfile = () => {
        const explorationCopy = new Exploration(exploration)
        explorationCopy.costProfile = undefined
        if (name !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setColumns([])
        setGridData([[]])
        setExploration(explorationCopy)
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Exploration</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="name" label="Name" />
                    <Input
                        id="name"
                        name="name"
                        placeholder="Enter name"
                        defaultValue={exploration?.name}
                        onChange={handleNameChange}
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
                <ImportButton
                    disabled={exploration?.costProfile === undefined}
                    color="danger"
                    onClick={deleteCostProfile}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable
                    columns={columns}
                    gridData={gridData}
                    onCellsChanged={onCellsChanged}
                    dG4Year={caseItem?.DG4Date?.getFullYear().toString()!}
                />
            </WrapperColumn>
            {!costProfileDialogOpen ? null
                : <Import onClose={() => { setCostProfileDialogOpen(!costProfileDialogOpen) }} onImport={onImport} />}
            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default ExplorationView
