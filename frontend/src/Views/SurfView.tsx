/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useEffect, useState } from "react"
import styled from "styled-components"
import { Button, Input, Typography } from "@equinor/eds-core-react"

import { useParams } from "react-router"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import { buildGridData, getColumnTitles, replaceOldData } from "../Components/DataTable/helpers"
import { Surf } from "../models/assets/surf/Surf"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { SurfCostProfile } from "../models/assets/surf/SurfCostProfile"
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
    const [tempSurf, setTempSurf] = useState<Surf>()
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)
    const [hasChanges, setHasChanges] = useState(false)
    const params = useParams()
    const emptyGUID = "00000000-0000-0000-0000-000000000000"

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                const newSurf = projectResult.surfs.find((s) => s.id === params.surfId || s.id === emptyGUID)
                setSurf(newSurf)
                //Cached version, used before pressing "Save"
                setTempSurf(newSurf)
                const newColumnTitles = getColumnTitles(caseResult, newSurf?.costProfile)
                setColumns(newColumnTitles)
                const newGridData = buildGridData(newSurf?.costProfile)
                setGridData(newGridData)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(getColumnTitles(caseItem, surf?.costProfile))
    }

    const onImport = (input: string, year: number) => {
        // const newSurf = Surf.Copy(surf!)^
        const emptySurf: Surf = {} as Surf
        const newCostProfile: SurfCostProfile = new SurfCostProfile()
        emptySurf.costProfile = newCostProfile
        emptySurf.costProfile.values = input.split("\t").map((i) => parseFloat(i))
        emptySurf.costProfile.startYear = year
        
        setTempSurf(emptySurf)
        
        const newColumnTitles = getColumnTitles(caseItem, emptySurf?.costProfile)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(emptySurf?.costProfile)
        setGridData(newGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
    
    }

    const handleSave = async () => {
        const emptySurf = {} as Surf
        // if (params.surfId){

        // }
        const surfDto = Surf.ToDto(emptySurf)
        const newProject = await GetSurfService().updateSurf(surfDto!)
        setProject(newProject)
        const newCase = newProject.cases.find((o) => o.id === params.caseId)
        setCase(newCase)
        const newSurf = newProject.surfs.find((s) => s.id === params.surfId)
        setSurf(newSurf)
        setHasChanges(false)
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                <Typography variant="h2">{surf ? surf.name : "untitled" }</Typography>
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

export default SurfView
