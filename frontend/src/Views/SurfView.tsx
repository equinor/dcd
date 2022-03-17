import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import { replaceOldData } from "../Components/DataTable/helpers"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const rowTitles = [
    "Substructure Cost Profile",
    "Dryweight",
    "Maturity",
]

// TODO: This data will have to be generated from the format received from the API
const initialGridData = [
    [
        {
            readOnly: true,
            value: rowTitles[0],
        },
        { value: 453678 },
        { value: 383920 },
        { value: 481726 },
        { value: 481726 },
        { value: 363728 },
        { value: 453678 },
        { value: 383920 },
    ],
    [
        {
            readOnly: true,
            value: rowTitles[1],
        },
        { value: 678290 },
        { value: 647382 },
        { value: 881726 },
        { value: 363728 },
        { value: 281726 },
        { value: 678290 },
        { value: 647382 },
    ],
    [
        {
            readOnly: true,
            value: rowTitles[2],
        },
        { value: 678290 },
        { value: 647382 },
        { value: 881726 },
        { value: 363728 },
        { value: 281726 },
        { value: 678290 },
        { value: 647382 },
    ],
]

const columnTitles = ["2022", "2023", "2024", "2025", "2026", "2027", "2028"]

const SurfView = () => {
    const [columns, setColumns] = React.useState<string[]>(columnTitles)
    const [gridData, setGridData] = React.useState<CellValue[][]>(initialGridData)

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setColumns(columnTitles)
    }

    return (
        <Wrapper>
            <Typography variant="h3">Surf</Typography>
            <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
        </Wrapper>
    )
}

export default SurfView
