import {
    Button, Input, Typography, Label,
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
import { Substructure } from "../models/assets/substructure/Substructure"
import { SubstructureCessationCostProfile } from "../models/assets/substructure/SubstructureCessationCostProfile"
import { SubstructureCostProfile } from "../models/assets/substructure/SubstructureCostProfile"
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

    const [costProfileColumns, setCostProfileColumns] = useState<string[]>([""])
    const [costProfileGridData, setCostProfileGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)

    const [cessationCostProfileColumns, setCessationCostProfileColumns] = useState<string[]>([""])
    const [cessationCostProfileGridData, setCessationCostProfileGridData] = useState<CellValue[][]>([[]])
    const [cessationCostProfileDialogOpen, setCessationCostProfileDialogOpen] = useState(false)

    const [hasChanges, setHasChanges] = useState(false)
    const [substructureName, setSubstructureName] = useState<string>("")
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
                setSubstructureName(newSubstructure.name!)

                const newCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newSubstructure?.costProfile)
                const newCessationCostProfileColumnTitles = getColumnAbsoluteYears(
                    caseResult,
                    newSubstructure?.substructureCessationCostProfileDto,
                )

                setCostProfileColumns(newCostProfileColumnTitles)
                setCessationCostProfileColumns(newCessationCostProfileColumnTitles)

                const newCostProfileGridData = buildGridData(newSubstructure?.costProfile)
                const newCessationCostProfileGridData = buildGridData(
                    newSubstructure?.substructureCessationCostProfileDto,
                )

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
        setCostProfileColumns(getColumnAbsoluteYears(caseItem, substructure?.costProfile))
    }

    const onCessationCostProfileCellsChanged = (changes:
        { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCessationCostProfileGridData = replaceOldData(cessationCostProfileGridData, changes)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileColumns(
            getColumnAbsoluteYears(
                caseItem,
                substructure?.substructureCessationCostProfileDto,
            ),
        )
    }

    const updateInsertSubstructureCostProfile = (input: string, year: number) => {
        const newSubstructure = new Substructure(substructure!)
        const newCostProfile = new SubstructureCostProfile()
        newSubstructure.id = newSubstructure.id ?? emptyGuid
        newSubstructure.costProfile = newSubstructure.costProfile ?? newCostProfile
        newSubstructure.costProfile!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newSubstructure.costProfile!.startYear = year
        newSubstructure.costProfile!.epaVersion = newSubstructure.costProfile.epaVersion ?? ""
        return newSubstructure
    }

    const updateInsertSubstructureCessationCostProfile = (input: string, year: number) => {
        const newSubstructure = new Substructure(substructure!)
        const newCessationCostProfile = new SubstructureCessationCostProfile()
        newSubstructure.id = newSubstructure.id ?? emptyGuid
        newSubstructure.substructureCessationCostProfileDto = newSubstructure.substructureCessationCostProfileDto
         ?? newCessationCostProfile
        newSubstructure.substructureCessationCostProfileDto!.values = input.replace(/(\r\n|\n|\r)/gm, "")
            .split("\t").map((i) => parseFloat(i))
        newSubstructure.substructureCessationCostProfileDto!.startYear = year
        newSubstructure.substructureCessationCostProfileDto!.epaVersion = newSubstructure
            .substructureCessationCostProfileDto.epaVersion ?? ""
        return newSubstructure
    }

    const onCostProfileImport = (input: string, year: number) => {
        const newSubstructure = updateInsertSubstructureCostProfile(input, year)
        setSubstructure(newSubstructure)
        const newCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, newSubstructure?.costProfile)
        setCostProfileColumns(newCostProfileColumnTitles)
        const newCostProfileGridData = buildGridData(newSubstructure?.costProfile)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        if (substructureName !== "") {
            setHasChanges(true)
        }
    }

    const onCessationCostProfileImport = (input: string, year: number) => {
        const newSubstructure = updateInsertSubstructureCessationCostProfile(input, year)
        setSubstructure(newSubstructure)
        const newCessationCostProfileColumnTitles = getColumnAbsoluteYears(
            caseItem,
            newSubstructure?.substructureCessationCostProfileDto,
        )
        setCessationCostProfileColumns(newCessationCostProfileColumnTitles)
        const newCessationCostProfileGridData = buildGridData(newSubstructure?.substructureCessationCostProfileDto)
        setCessationCostProfileGridData(newCessationCostProfileGridData)
        setCessationCostProfileDialogOpen(!cessationCostProfileDialogOpen)
        if (substructureName !== "") {
            setHasChanges(true)
        }
    }

    const handleSave = async () => {
        const substructureDto = new Substructure(substructure!)
        substructureDto.name = substructureName
        if (substructure?.id === emptyGuid) {
            substructureDto.projectId = params.projectId
            const updatedProject: Project = await
            GetSubstructureService().createSubstructure(params.caseId!, substructureDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newSubstructure = updatedProject.substructures.at(-1)
            const newUrl = location.pathname.replace(emptyGuid, newSubstructure!.id!)
            setSubstructure(newSubstructure)
            setCase(updatedCase)
            navigate(`${newUrl}`, { replace: true })
        } else {
            substructureDto.projectId = params.projectId
            const newProject = await GetSubstructureService().updateSubstructure(substructureDto!)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newSubstructure = newProject.substructures.find((s) => s.id === params.substructureId)
            setSubstructure(newSubstructure)
        }
        setHasChanges(false)
    }

    const deleteCostProfile = () => {
        const substructureCopy = new Substructure(substructure)
        substructureCopy.costProfile = undefined
        if (substructureName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setCostProfileColumns([])
        setCostProfileGridData([[]])
        setSubstructure(substructureCopy)
    }

    const deleteCessationCostProfile = () => {
        const substructureCopy = new Substructure(substructure)
        substructureCopy.substructureCessationCostProfileDto = undefined
        if (substructureName !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
        setCessationCostProfileColumns([])
        setCessationCostProfileGridData([[]])
        setSubstructure(substructureCopy)
    }

    const handleSubstructureNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setSubstructureName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "") {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Substructure</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="substructureName" label="Name" />
                    <Input
                        id="substructureName"
                        name="substructureName"
                        placeholder="Enter substructure name"
                        value={substructureName}
                        onChange={handleSubstructureNameFieldChange}
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
                    disabled={substructure?.costProfile === undefined}
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
            {(!costProfileDialogOpen
                ? null : (
                    <Import
                        onClose={() => { setCostProfileDialogOpen(!costProfileDialogOpen) }}
                        onImport={onCostProfileImport}
                    />
                ))}

            <Wrapper>
                <Typography variant="h4">Cessation Cost profile</Typography>
                <ImportButton onClick={() => { setCessationCostProfileDialogOpen(true) }}>Import</ImportButton>
                <ImportButton
                    disabled={substructure?.substructureCessationCostProfileDto === undefined}
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
            {!cessationCostProfileDialogOpen
                ? null
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

export default SubstructureView
