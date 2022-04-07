import React, { ChangeEventHandler, useEffect, useState } from "react"
import styled from "styled-components"
import {
    Button, Input, Label, Typography,
} from "@equinor/eds-core-react"

import { useNavigate, useLocation, useParams } from "react-router"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { SurfCostProfile } from "../models/assets/surf/SurfCostProfile"
import { SurfCessasionCostProfile } from "../models/assets/surf/SurfCessasionCostProfile"
import { GetProjectService } from "../Services/ProjectService"
import Import from "../Components/Import/Import"
import { GetSurfService } from "../Services/SurfService"

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

const SurfView = () => {
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()

    //Cost Profile
    const [costProfileColumns, setCostProfileColumnes] = useState<string[]>([""])
    const [costProfileGridData, setCostProfileGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)

    //Cessasion Cost Profile
    const [cessasionCostProfileColumns, setCessasionCostProfileColumnes] = useState<string[]>([""])
    const [cessasionCostProfileGridData, setCessasionCostProfileGridData] = useState<CellValue[][]>([[]])
    const [cessasionCostProfileDialogOpen, setCessasionCostProfileDialogOpen] = useState(false)

    const [hasChanges, setHasChanges] = useState(false)
    const [surfName, setSurfName] = useState<string>("")
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()
    const emptyGUID = "00000000-0000-0000-0000-000000000000"

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                let newSurf = projectResult.surfs.find((s) => s.id === params.surfId)
                if (newSurf !== undefined) {
                    setSurf(newSurf)
                } else {
                    newSurf = new Surf()
                    setSurf(newSurf)
                }
                setSurfName(newSurf.name!)
                //Cost Profile
                const newCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newSurf?.costProfile)
                const newCessasionCostProfileColumnTitles = getColumnAbsoluteYears(caseResult, newSurf?.surfCessationCostProfileDto)

                setCostProfileColumnes(newCostProfileColumnTitles)
                setCessasionCostProfileColumnes(newCessasionCostProfileColumnTitles)
                
                const newCostProfileGridData = buildGridData(newSurf?.costProfile)
                const newCessasionCostProfileGridData = buildGridData(newSurf?.surfCessationCostProfileDto)

                setCostProfileGridData(newCostProfileGridData)
                setCessasionCostProfileGridData(newCessasionCostProfileGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCostProfileCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCostProfileGridData = replaceOldData(costProfileGridData, changes)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileColumnes(getColumnAbsoluteYears(caseItem, surf?.costProfile))
        setHasChanges(true)
    }

    const onCessasionCostProfileCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newCessasionCostProfileGridData = replaceOldData(cessasionCostProfileGridData, changes)
        setCessasionCostProfileGridData(newCessasionCostProfileGridData)
        setCessasionCostProfileColumnes(getColumnAbsoluteYears(caseItem, surf?.surfCessationCostProfileDto))
        setHasChanges(true)
    }

    const updateInsertSurfCostProfile = (input: string, year: number) => {
        const newSurf = new Surf(surf!)
        const newCostProfile: SurfCostProfile = new SurfCostProfile()
        newSurf.id = newSurf.id ?? emptyGUID
        newSurf.costProfile = newSurf.costProfile ?? newCostProfile
        newSurf.costProfile.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        newSurf.costProfile.startYear = year
        newSurf.costProfile.epaVersion = newSurf.costProfile.epaVersion ?? ""
        return newSurf
    }

    const updateInsertSurfCessasionCostProfile = (input: string, year: number) => {
        const newSurf = new Surf(surf!)
        const newCessasionCostProfile: SurfCessasionCostProfile = new SurfCessasionCostProfile()
        newSurf.id = newSurf.id ?? emptyGUID
        newSurf.surfCessationCostProfileDto = newSurf.surfCessationCostProfileDto ?? newCessasionCostProfile
        newSurf.surfCessationCostProfileDto.values = input.replace(/(\r\n|\n|\r)/gm, "").split("\t").map((i) => parseFloat(i))
        newSurf.surfCessationCostProfileDto.startYear = year
        newSurf.surfCessationCostProfileDto.epaVersion = newSurf.surfCessationCostProfileDto.epaVersion ?? ""
        return newSurf
    }

    const onCostProfileImport = (input: string, year: number) => {
        const emptySurf: Surf = updateInsertSurfCostProfile(input, year)
        setSurf(emptySurf)
        const newCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, emptySurf?.costProfile)
        setCostProfileColumnes(newCostProfileColumnTitles)
        const newCostProfileGridData = buildGridData(emptySurf?.costProfile)
        setCostProfileGridData(newCostProfileGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
        setHasChanges(true)
    }

    const onCessasionCostProfileImport = (input: string, year: number) => {
        const emptySurf: Surf = updateInsertSurfCessasionCostProfile(input, year)
        setSurf(emptySurf)
        const newCesassionCostProfileColumnTitles = getColumnAbsoluteYears(caseItem, emptySurf?.surfCessationCostProfileDto)
        setCessasionCostProfileColumnes(newCesassionCostProfileColumnTitles)
        const newCessasionCostProfileGridData = buildGridData(emptySurf?.surfCessationCostProfileDto)
        setCessasionCostProfileGridData(newCessasionCostProfileGridData)
        setCessasionCostProfileDialogOpen(!cessasionCostProfileDialogOpen)
        setHasChanges(true)
    }

    const handleSave = async () => {
        const surfDto = new Surf(surf!)
        surfDto.name = surfName
        if (surf?.id === emptyGUID) {
            surfDto.projectId = params.projectId
            const updatedProject = await GetSurfService().createSurf(params.caseId!, surfDto!)
            const updatedCase = updatedProject.cases.find((o) => o.id === params.caseId)
            const newSurf = updatedProject.surfs.at(-1)
            const newUrl = location.pathname.replace(emptyGUID, newSurf!.id!)
            setProject(updatedProject)
            setCase(updatedCase)
            setSurf(newSurf)
            navigate(`${newUrl}`, { replace: true })
        } else {
            const newProject = await GetSurfService().updateSurf(surfDto)
            setProject(newProject)
            const newCase = newProject.cases.find((o) => o.id === params.caseId)
            setCase(newCase)
            const newSurf = newProject.surfs.find((a) => a.id === params.surfId)
            setSurf(newSurf)
        }
        setHasChanges(false)
    }

    const handleSurfNameFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setSurfName(e.target.value)
        if (e.target.value !== undefined && e.target.value !== "" && e.target.value !== surf?.name) {
            setHasChanges(true)
        } else {
            setHasChanges(false)
        }
    }

    return (
        <AssetViewDiv>
            <Typography variant="h2">Surf</Typography>
            <AssetHeader>
                <WrapperColumn>
                    <Label htmlFor="surfName" label="Name" />
                    <Input
                        id="surfName"
                        name="surfName"
                        placeholder="Enter surf name"
                        defaultValue={surf?.name}
                        onChange={handleSurfNameFieldChange}
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
                <Typography variant="h4">Cessasion Cost profile</Typography>
                <ImportButton onClick={() => { setCessasionCostProfileDialogOpen(true) }}>Import</ImportButton>
            </Wrapper>
            <WrapperColumn>
                <DataTable columns={cessasionCostProfileColumns} gridData={cessasionCostProfileGridData} onCellsChanged={onCessasionCostProfileCellsChanged} />
            </WrapperColumn>
            {!cessasionCostProfileDialogOpen ? null
                : <Import onClose={() => { setCessasionCostProfileDialogOpen(!cessasionCostProfileDialogOpen) }} onImport={onCessasionCostProfileImport} />}

            <Wrapper><SaveButton disabled={!hasChanges} onClick={handleSave}>Save</SaveButton></Wrapper>
        </AssetViewDiv>
    )
}

export default SurfView
