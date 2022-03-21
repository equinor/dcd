/* eslint-disable max-len */
import { Input, Typography } from "@equinor/eds-core-react"
import { useEffect, useState } from "react"
import { useParams } from "react-router"
import styled from "styled-components"
import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import { replaceOldData } from "../Components/DataTable/helpers"
import { Substructure } from "../models/assets/substructure/Substructure"
import { SubstructureCostProfile } from "../models/assets/substructure/SubstructureCostProfile"
import { Case } from "../models/Case"
import { Project } from "../models/Project"
import { GetProjectService } from "../Services/ProjectService"

const CaseHeader = styled.div`
    margin-bottom: 2rem;
    display: flex;

    > *:first-child {
        margin-right: 2rem;
    }
`

const CaseViewDiv = styled.div`
    margin: 2rem;
    display: flex;
    flex-direction: column;
`

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const Dg4Field = styled.div`
    margin-bottom: 2rem;
    width: 10rem;
    display: flex;
`

const SubstructureView = () => {
    const [project, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()
    const [substructure, setSubstructure] = useState<Substructure>()
    const [columns, setColumns] = useState<string[]>([""])
    const [gridData, setGridData] = useState<CellValue[][]>([[]])
    const params = useParams()

    const getColumnTitles = (newCaseItem?: Case, newSubstructure?: Substructure) => {
        const startYears: string[] = []
        // eslint-disable-next-line no-unsafe-optional-chaining
        const startYear = (newCaseItem?.DG4Date?.getFullYear() ?? 0) + (newSubstructure?.substructureCostProfile?.startYear ?? 0)
        newSubstructure?.substructureCostProfile?.values?.forEach((_, i) => {
            startYears.push((startYear + i).toString())
        })
        return startYears
    }

    const buildGridData = (costProfile?: SubstructureCostProfile) => {
        const grid: ({readOnly: boolean, value: string} | {value: number, readOnly?: boolean})[][] = [
            [
                {
                    readOnly: true,
                    value: "Cost profile",
                },
            ],
        ]

        costProfile?.values?.forEach((element) => {
            console.log(grid[0][0].value)
            grid[0].push({ value: element })
        })

        console.log("Grid: ", grid)

        return grid
    }

    useEffect(() => {
        (async () => {
            try {
                const projectResult = await GetProjectService().getProjectByID(params.projectId!)
                setProject(projectResult)
                const caseResult = projectResult.cases.find((o) => o.id === params.caseId)
                setCase(caseResult)
                const newSubstructure = projectResult.substructures.find((s) => s.id === params.substructureId)
                setSubstructure(newSubstructure)
                const newColumnTitles = getColumnTitles(caseResult, newSubstructure)
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
        setColumns(getColumnTitles(caseItem, substructure))
    }

    return (
        <CaseViewDiv>
            <CaseHeader>
                <Typography variant="h2">{substructure?.name}</Typography>
            </CaseHeader>
            <Typography>DG4</Typography>
            <Dg4Field>
                <Input disabled defaultValue={caseItem?.DG4Date?.toLocaleDateString("en-CA")} type="date" />
            </Dg4Field>
            <Wrapper>
                <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
            </Wrapper>
        </CaseViewDiv>
    )
}

export default SubstructureView
