import React from 'react'
import styled from 'styled-components'
import { Button, Scrim, Typography } from '@equinor/eds-core-react'

import ExcelImport from '../Components/ExcelImport/ExcelImport'
import DataTable, { CellValue } from '../Components/DataTable/DataTable'
import { generateNewGrid, replaceOldData } from '../Components/DataTable/helpers'

const Wrapper = styled.div`
    display: flex;
    flex-direction: column;
`

const ImportButton = styled(Button)`
    width: 6rem;
    align-self: flex-end;
`

const ScrimBackground = styled(Scrim)`
    width: 100%;
`

const Buttons = styled.div`
    align-self: flex-end;
    margin-top: 3rem;
`

const CancelButton = styled(Button)`
    margin-right: 1rem;
`

// TODO: This data will have to be generated from the format received from the API
const initialGridData = [
    [
        {
            readOnly: true,
            value: 'Cost profile',
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
            value: 'G&G and admin cost',
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

const rowTitles = ['Cost profile', 'G&G and admin cost']

const columnTitles = ['2022', '2023', '2024', '2025', '2026', '2027', '2028']

const ExplorationView = () => {
    const [isImportOpen, setIsImportOpen] = React.useState<boolean>(false)
    const [dataIsChanged, setDataIsChanged] = React.useState<boolean>(false)
    const [columns, setColumns] = React.useState<string[]>(columnTitles)
    const [gridData, setGridData] = React.useState<CellValue[][]>(initialGridData)

    const closeImportView = () => setIsImportOpen(false)
    const openImportView = () => setIsImportOpen(true)

    const onCellsChanged = (changes: { cell: { value: number }; col: number; row: number; value: string }[]) => {
        const newGridData = replaceOldData(gridData, changes)
        setGridData(newGridData)
        setDataIsChanged(true)
    }

    const onImport = (data: { [key: string]: string }[]) => {
        const { newGridData, newColumns } = generateNewGrid(data, rowTitles)

        setColumns(newColumns)
        setGridData(newGridData)
        setDataIsChanged(true)

        closeImportView()
    }

    const revertChange = () => {
        setGridData(initialGridData)
        setColumns(columnTitles)
        setDataIsChanged(false)
    }

    const saveDataImport = () => {
        // TODO CODE TO SAVE DATA HERE
        setDataIsChanged(false)
    }

    return (
        <Wrapper>
            <ImportButton onClick={openImportView}>Import...</ImportButton>
            <Typography variant="h3">Exploration</Typography>
            <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
            {isImportOpen && (
                <ScrimBackground isDismissable={true} onClose={closeImportView}>
                    <ExcelImport onClose={closeImportView} onImport={onImport} />
                </ScrimBackground>
            )}
            {dataIsChanged && (
                <Buttons>
                    <CancelButton variant={'outlined'} onClick={revertChange}>
                        Cancel change
                    </CancelButton>
                    <Button onClick={saveDataImport}>Save new data</Button>
                </Buttons>
            )}
        </Wrapper>
    )
}

export default ExplorationView
