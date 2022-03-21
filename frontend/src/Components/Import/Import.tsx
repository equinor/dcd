import React, { useState } from "react"
import styled from "styled-components"
import {
 Button, Dialog, Input, Typography,
} from "@equinor/eds-core-react"

import { tsvToJson } from "./helpers"

const StyledDialog = styled(Dialog)`
    width: 50rem;
`

const TextArea = styled.textarea`
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

const Label = styled.label`
    margin-top: 2rem;
`

const ImportButton = styled(Button)`
    margin-right: 0.5rem;
`

const Main = styled.div`
    display: flex;
    flex-direction: column;
    padding: 1rem;
`

interface Props {
    onClose: () => void
    onImport: (input: string, year: number) => void
}

function ExcelImport({ onClose, onImport }: Props) {
    const [dataInput, setDataInput] = useState<string>("")
    const [startYear, setStartYear] = useState(0)

    const example = "value1\tvalue2\tvalue3\tvalue4"

    const onChangeStartYear = (event: any) => {
        setStartYear(event.currentTarget.value)
        console.log(startYear)
    }

    const onChangeInput = (event: any) => {
        setDataInput(event.currentTarget.value)
    }

    const onClickImport = () => {
        console.log("1")
        if (!dataInput) {
            return
        }
        console.log("2")
        // const inputAsJson = tsvToJson(dataInput)
        onImport(dataInput, startYear)
    }

    return (
        <StyledDialog>
            <Dialog.Title>Import tab separated date from Excel</Dialog.Title>
            <Main>
                <Typography>Start year relative to DG4</Typography>
                <Input type="number" onChange={onChangeStartYear} />
                <Label>Paste your values here:</Label>
                <TextArea
                    cols={30}
                    rows={3}
                    placeholder={example}
                    value={dataInput}
                    onChange={onChangeInput}
                />
            </Main>
            <Dialog.Actions>
                <ImportButton onClick={onClickImport}>Import</ImportButton>
                <Button variant="outlined" onClick={onClose}>
                    Cancel
                </Button>
            </Dialog.Actions>
        </StyledDialog>
    )
}

export default ExcelImport
