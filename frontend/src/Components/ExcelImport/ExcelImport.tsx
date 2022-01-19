import React, { useState } from 'react'
import styled from 'styled-components'
import { Button, Dialog, Typography } from '@equinor/eds-core-react'
import { Case } from '../../types'
import { tsvToJson } from './helpers'

const StyledTextArea = styled.textarea`
    flex-grow: 1;
    resize: none;
    white-space: pre;
    overflow: scroll;
    tab-size: 20;
`

const Bold = styled.em`
    font-style: normal;
    font-weight: bold;
`

const Main = styled.div`
    display: flex;
    flex-direction: column;
    padding: 1rem;
`

interface Props {
    onClose: () => void
    onImport: (obj: { [key: string]: string }[]) => void
}

const ExcelImport = ({ onClose, onImport }: Props) => {
    const [dataInput, setDataInput] = useState<string>('')

    const example =
        `E.g\n` +
        `forecastYear\tforecastMonth\toilProduction\trichGasProduction\n` +
        `2019\t1\t1000000\t1200000\n` +
        `2019\t2\t1000000\t1200000\n` +
        `2019\t3\t1000000\t1200000\n` +
        `2019\t4\t1000000\t1200000\n` +
        `2019\t5\t1000000\t1200000`

    const onChangeInput = (event: any) => {
        setDataInput(event.currentTarget.value)
    }

    const onClickImport = () => {
        if (!dataInput) {
            return
        }
        const inputAsJson = tsvToJson(dataInput)
        onImport(inputAsJson)
    }

    return (
        <Dialog style={{ width: '50rem' }}>
            <Dialog.Title>Import data from Excel</Dialog.Title>
            <Main>
                <Typography>
                    To paste values correctly in the table, make sure that <Bold>column titles</Bold> are part of the paste.
                </Typography>
                <label style={{ marginTop: '2rem' }}>Paste your information here:</label>
                <StyledTextArea cols={30} rows={10} placeholder={example} value={dataInput} onChange={onChangeInput}></StyledTextArea>
            </Main>
            <Dialog.Actions>
                <Button style={{ marginRight: '0.5rem' }} onClick={onClickImport}>
                    Import
                </Button>
                <Button variant="outlined" onClick={onClose}>
                    Cancel
                </Button>
            </Dialog.Actions>
        </Dialog>
    )
}

export default ExcelImport
