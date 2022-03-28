/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { ChangeEventHandler, useEffect, useState } from "react"
import styled from "styled-components"
import { Button, Input, Typography } from "@equinor/eds-core-react"

import { useNavigate, useLocation, useParams } from "react-router"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import {
    buildGridData, getColumnAbsoluteYears, replaceOldData,
} from "../Components/DataTable/helpers"
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
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [surf, setSurf] = useState<Surf>()
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const [costProfileDialogOpen, setCostProfileDialogOpen] = useState(false)
    const [hasChanges, setHasChanges] = useState(false)
    const [hasName, setHasHasName] = useState(false)
    const [surfName, setSurfName] = useState<Record<string, any>>({})
    const params = useParams()
    const navigate = useNavigate()
    const location = useLocation()
    const emptyGUID = "00000000-0000-0000-0000-000000000000"

    const assetName = async (name:string | undefined) => {
        const surfProjectName = project?.surfs.find((s) => s.id === params.surfId)?.name
        if (name !== "") {
            setHasHasName(true)
            if (surfProjectName !== name) {
                setHasChanges(true)
            }
        }
    }
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
                    assetName(surf?.name)
                } else {
                    newSurf = new Surf()
                    setSurf(newSurf)
                }
                const newColumnTitles = getColumnAbsoluteYears(caseResult, newSurf?.costProfile)
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
        setColumns(getColumnAbsoluteYears(caseItem, surf?.costProfile))
    }

    const createNewSurf = (input: string, year: number) => {
        const emptySurf = Surf.Copy(surf!)
        const newCostProfile: SurfCostProfile = new SurfCostProfile()
        emptySurf.id = emptyGUID
        emptySurf.costProfile = newCostProfile
        emptySurf.costProfile.values = input.split("\t").map((i) => parseFloat(i))
        emptySurf.costProfile.startYear = year
        emptySurf.costProfile.epaVersion = "test"
        return emptySurf
    }

    const onImport = (input: string, year: number) => {
        const emptySurf: Surf = createNewSurf(input, year)
        setSurf(emptySurf)
        const newColumnTitles = getColumnAbsoluteYears(caseItem, emptySurf?.costProfile)
        setColumns(newColumnTitles)
        const newGridData = buildGridData(emptySurf?.costProfile)
        setGridData(newGridData)
        setCostProfileDialogOpen(!costProfileDialogOpen)
    }

    const handleSave = async () => {
        const surfDto = Surf.ToDto(surf!)
        if (surf?.id === emptyGUID) {
            surfDto.projectId = params.projectId
            const newProject = await GetSurfService().createSurf(params.caseId!, surfDto!)
            const newSurf = newProject.surfs.at(-1)
            const newUrl = location.pathname.replace(emptyGUID, newSurf!.id!)
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
        setSurfName({
            SurfName: surfName,
            [e.target.name]: e.target.value,
        })
    }

    return (
        <AssetViewDiv>
            <AssetHeader>
                {hasName
                    ? (<Typography variant="h2">{surf?.name}</Typography>)
                    : (
                        <Input
                            id="surfName"
                            name="surfName"
                            onChange={handleSurfNameFieldChange}
                        />
                    )}
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
