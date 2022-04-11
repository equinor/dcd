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
import { Topside } from "../models/assets/topside/Topside"
import { TopsideCostProfile } from "../models/assets/topside/TopsideCostProfile"
import { TopsideCessasionCostProfile } from "../models/assets/topside/TopsideCessasionCostProfile"
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

    // Cost Profile
    const [costProfileColumns, setCostProfileColumns] = useState<string[]>([""])
    const [costProfileGridData, setCostProfileGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)

    // Cessasion Cost Profile
    const [cessationCostProfileColumns, setCessationCostProfileColumns] = useState<string[]>([""])
    const [cessationCostProfileGridData, setCessationCostProfileGridData] = useState<CellValue[][]>([[]])
    const [cessationCostProfileDialogOpen, setCessationCostProfileDialogOpen] = useState(false)

    const [hasChanges, setHasChanges] = useState(false)
    const [topsideName, setTopsideName] = useState<string>("")
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
                let newTopside = projectResult.topsides.find((s) => s.id === params.topsideId)
                if (newTopside !== undefined) {
                    setTopside(newTopside)
                } else {
                    newTopside = new Topside()
                    setTopside(newTopside)
                }
                setTopsideName(newTopside.name!)
                const newCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newTopside?.costProfile)
                const newCessationCostProfileColumnTitles = getColumnAbsoluteYears(
                    caseResult,
                    newTopside?.topsideCessasionCostProfileDto,
                )

                setCostProfileColumns(newCostProfileColumnTitles)
                setCessationCostProfileColumns(newCessationCostProfileColumnTitles)

                const newCostProfileGridData = buildGridData(newTopside?.costProfile)
                const newCessationCostProfileGridData = buildGridData(newTopside?.topsideCessasionCostProfileDto)

                setCostProfileGridData(newCostProfileGridData)
                setCessationCostProfileGridData(newCessationCostProfileGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCostProfileCellsChanged = (changes:
         { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCostProfileGridData = replaceOldData(costProfileGridData, changes)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileColumns(getColumnAbsoluteYears(caseItem, topside?.costProfile))
    }

    const onCessationCostProfileCellsChanged = (changes:
         { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCessationCostProfileGridData = replaceOldData(cessationCostProfileGridData, changes)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileColumns(getColumnAbsoluteYears(caseItem, topside?.topsideCessasionCostProfileDto))
    }

    const updateInsertTopsideCostProfile = (input: string, year: number) => {
        const newTopside = new Topside(topside!)
        const newCostProfile = new TopsideCostProfile()
        newTopside.id = newTopside.id ?? emptyGuid
        newTopside.costProfile = newTopside.costProfile ?? newCostProfile
        newTopside.costProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newTopside.costProfile!.startYear = year
        newTopside.costProfile!.epaVersion = newTopside.costProfile.epaVersion ?? ""
        return newTopside
    }

    const updateInsertTopsideCessasionCostProfile = (input: string, year: number) => {
        const newTopside = new Topside(topside!)
        const newCessationCostProfile = new TopsideCessasionCostProfile()
        newTopside.id = newTopside.id ?? emptyGuid
        newTopside.topsideCessasionCostProfileDto = newTopside.topsideCessasionCostProfileDto ?? newCessationCostProfile
        newTopside.topsideCessasionCostProfileDto!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newTopside.topsideCessasionCostProfileDto!.startYear = year
        newTopside.topsideCessasionCostProfileDto!.epaVersion = newTopside.topsideCessasionCostProfileDto
            .epaVersion ?? ""
        return newTopside
    }

    const onCostProfileImport = (input: string, year: number) => {
        const newTopside = updateInsertTopsideCostProfile(input, year)
        setTopside(newTopside)
        const newCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, newTopside?.costProfile)
        setCostProfileColumns(newCostProfileColumnTitles)
        const newCostProfileGridData = buildGridData(newTopside?.costProfile)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        if (topsideName !== "") {
            setHasChanges(true)
        }
    }

    const onCessationCostProfileImport = (input: string, year: number) => {
        const newTopside = updateInsertTopsideCessasionCostProfile(input, year)
        setTopside(newTopside)
        const newCessationCostProfileColumnTitles = getColumnAbsoluteYears(
            caseItem,
            newTopside?.topsideCessasionCostProfileDto,
        )
        setCessationCostProfileColumns(newCessationCostProfileColumnTitles)
        const newCessationCostProfileGridData = buildGridData(newTopside?.topsideCessasionCostProfileDto)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileDialogOpen(!cessationCostProfileDialogOpen)
        if (topsideName !== "") {
            setHasChanges(true)
        }
    }

    const handleSave = async () => {
        const topsideDto = new Topside(topside!)
        topsideDto.name = topsideName
        if (topside?.id === emptyGuid) {
            topsideDto.projectId = params.projectId
            const updatedProject: Project = await GetTopsideService().createTopside(params.caseId!, topsideDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newTopside = updatedProject.topsides.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newTopside!.id!)
            setTopside(newTopside)
            setCase(updatedCase)
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

    const handleTopsideNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setTopsideName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    const deleteCostProfile = () => {
        const topsideCopy = new Topside(topside)
        topsideCopy.costProfile = undefined
        if (topsideName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setCostProfileColumns([])
        setCostProfileGridData([[]])
        setTopside(topsideCopy)
    }

    const deleteCessationCostProfile = () => {
        const topsideCopy = new Topside(topside)
        topsideCopy.topsideCessasionCostProfileDto = undefined
        if (topsideName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setCessationCostProfileColumns([])
        setCessationCostProfileGridData([[]])
        setTopside(topsideCopy)
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Topside</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="topsideName" label="Name" />
                    <Input
                        id="topsideName"
                        name="topsideName"
                        placeholder="Enter topside name"
                        value={topsideName}
                        onChange={handleTopsideNameFieldChange}
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
                    disabled={topside?.costProfile === undefined}
                    color="danger"
                    onClick={deleteCostProfile}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable
                    columns={costProfileColumns}
                    gridData={costProfileGridData}
                    onCellsChanged={onCostProfileCellsChanged}
                    dG4Year={caseItem?.DG4Date?.getFullYear().toString()!}
                />
            </WrapperColumn>
            {!costProfileDialogOpen
                ? null
                : (
                    <Import
                        onClose={() => { setCostProfileDialogOpen(!costProfileDialogOpen) }}
                        onImport={onCostProfileImport}
                    />
                )}

            <Wrapper>
                <Typography variant="h4">Cessasion Cost profile</Typography>
                <ImportButton onClick={() => { setCessationCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton
                    disabled={topside?.topsideCessasionCostProfileDto === undefined}
                    color="danger"
                    onClick={deleteCessationCostProfile}
                >
                    Delete
                </ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable
                    columns={cessationCostProfileColumns}
                    gridData={cessationCostProfileGridData}
                    onCellsChanged={onCessationCostProfileCellsChanged}
                    dG4Year={caseItem?.DG4Date?.getFullYear().toString()!}
                />
            </WrapperColumn>
            {!cessationCostProfileDialogOpen ? null
                : (
                    <Import
                        onClose={() => { setCessationCostProfileDialogOpen(!cessationCostProfileDialogOpen) }}
                        onImport={onCessationCostProfileImport}
                    />
                )}

            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default TopsideView
