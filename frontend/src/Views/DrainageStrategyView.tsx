import React from 'react'
import styled from 'styled-components'
import { Button, Scrim, Typography } from '@equinor/eds-core-react'
import { useTranslation } from "react-i18next";

import ExcelImport from '../Components/ExcelImport/ExcelImport'
import DataTable, { CellValue } from '../Components/DataTable/DataTable'
import { generateNewGrid, replaceOldData } from '../Components/DataTable/helpers'
import i18n from '../i18n';

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

var rowTitles = [
    i18n.t('DrainageStrategyView.ProductionProfileOil'),
    i18n.t('DrainageStrategyView.ProductionProfileGas'),
    i18n.t('DrainageStrategyView.ProductionProfileWater'),
    i18n.t('DrainageStrategyView.ProductionProfileWaterInjection'),
    i18n.t('DrainageStrategyView.FuelFlaringAndLosses'),
    i18n.t('DrainageStrategyView.NetSalesGas'),
    i18n.t('DrainageStrategyView.CO2Emissions'),
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
    ],
    [
        {
            readOnly: true,
            value: rowTitles[2],
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
            value: rowTitles[3],
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
            value: rowTitles[4],
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
            value: rowTitles[5],
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
            value: rowTitles[6],
        },
        { value: 373638 },
        { value: 237389 },
        { value: 381724 },
        { value: 281726 },
        { value: 481726 },
    ],
]

const columnTitles = ['2022', '2023', '2024', '2025', '2026']

const DrainageStrategyView = () => {
    const { t } = useTranslation();
    const [isImportOpen, setIsImportOpen] = React.useState<boolean>(false)
    const [dataIsChanged, setDataIsChanged] = React.useState<boolean>(false)
    const [columns, setColumns] = React.useState<string[]>(columnTitles)
    const [gridData, setGridData] = React.useState<CellValue[][]>(initialGridData)
    
    const closeImportView = () => setIsImportOpen(false)
    const openImportView = () => setIsImportOpen(true)
    const onCellsChanged = (changes: any[]) => {
        
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

    const reloadGridDataWithTranslation = () => {
        var currentGridData = gridData
        for (let i = 0; i<rowTitles.length; i++) {
            currentGridData[0][i].value = rowTitles[i]
        }
          
        setGridData(currentGridData)
        setDataIsChanged(true)
    }
    

    const saveDataImport = () => {
        // TODO CODE TO SAVE DATA HERE
        setDataIsChanged(false)
    }

    return (
        <Wrapper>
            <ImportButton onClick={openImportView}>{t('DrainageStrategyView.Import')}</ImportButton>
            <Typography variant="h3">{t('DrainageStrategyView.DrainageStrategy')}</Typography>
            <DataTable columns={columns} gridData={gridData} onCellsChanged={onCellsChanged} />
            {isImportOpen && (
                <ScrimBackground isDismissable={true} onClose={closeImportView}>
                    <ExcelImport onClose={closeImportView} onImport={onImport} />
                </ScrimBackground>
            )}
            {dataIsChanged && (
                <Buttons>
                    <CancelButton variant={'outlined'} onClick={revertChange}>
                        {t('DrainageStrategyView.CancelChange')}
                    </CancelButton>
                    <Button onClick={saveDataImport}>{t('DrainageStrategyView.SaveNewData')}</Button>
                </Buttons>
            )}
        </Wrapper>
    )
}

export default DrainageStrategyView
