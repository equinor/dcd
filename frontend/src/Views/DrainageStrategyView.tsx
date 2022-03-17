import React from "react"
import styled from "styled-components"
import { Typography } from "@equinor/eds-core-react"

import DataTable, { CellValue } from "../Components/DataTable/DataTable"
import { replaceOldData } from "../Components/DataTable/helpers"

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

// TODO: This data will have to be generated from the format received from the API
const initialGridData = [
    [
        {
            readOnly: true,
            value: "Production profile oil",
        },
        { value: 453678 },
        { value: 383920 },
        { value: 481726 },
        { value: 481726 },
        { value: 363728 },
    ],
    [
        {
            readOnly: true,
            value: "Production profile gas",
        },
        { value: 678290 },
        { value: 647382 },
        { value: 881726 },
        { value: 363728 },
        { value: 281726 },
    ],
    [
        {
            readOnly: true,
            value: "Production profile water",
        },
        { value: 363728 },
        { value: 281726 },
        { value: 381723 },
        { value: 481726 },
        { value: 363728 },
    ],
    [
        {
            readOnly: true,
            value: "Production profile water injection",
        },
        { value: 373638 },
        { value: 237389 },
        { value: 381724 },
        { value: 281726 },
        { value: 373638 },
    ],
    [
        {
            readOnly: true,
            value: "Fuel flaring and losses",
        },
        { value: 373638 },
        { value: 237389 },
        { value: 363728 },
        { value: 381724 },
        { value: 281726 },
    ],
    [
        {
            readOnly: true,
            value: "Net sales gas",
        },
        { value: 373638 },
        { value: 237389 },
        { value: 363728 },
        { value: 281726 },
        { value: 381724 },
    ],
    [
        {
            readOnly: true,
            value: "CO2 emissions",
        },
        { value: 373638 },
        { value: 237389 },
        { value: 381724 },
        { value: 281726 },
        { value: 481726 },
    ],
]

const columnTitles = ["2022", "2023", "2024", "2025", "2026"]

function DrainageStrategyView() {
    const [columns, setColumns] = React.useState<string[]>(columnTitles)
    const [gridData, setGridData] = React.useState<CellValue[][]>(initialGridData)

    const onCellsChanged = (changes: any[]) => {
        setColumns(columnTitles)
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
    }

    return (
        <Wrapper>
            <Typography variant="h3">Drainage Strategy</Typography>
            <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
        </Wrapper>
    )
}

export default DrainageStrategyView
