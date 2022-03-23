import React, { useState } from "react"
import styled from "styled-components"
import {
    Button, Dialog, Input, Typography,
} from "@equinor/eds-core-react"

const StyledDialog = styled(Dialog)`
    width: 50rem;
    z-index: 1000;
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 50px;
    z-index: 1000;
    background-color: white;
    border: 2px solid gray;
`

const TextArea = styled.textarea`
    flex-grow: 1;
    resize: none;
    white-space: pre;
    overflow: scroll;
    tab-size: 20;
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

function Import({ onClose, onImport }: Props) {
    const [dataInput, setDataInput] = useState<string>("")
    const [startYear, setStartYear] = useState(0)

    const example = "value1\tvalue2\tvalue3\tvalue4"

    const onChangeStartYear = (event: any) => {
        setStartYear(event.currentTarget.value)
    }

    const onChangeInput = (event: any) => {
        setDataInput(event.currentTarget.value)
    }

    const onClickImport = () => {
        if (!dataInput) {
            return
        }
        onImport(dataInput, startYear)
    }

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (e.key === "Tab") {
            e.preventDefault()
            const input = `${dataInput}\t`
            setDataInput(input)
        }
    }

    return (
        <StyledDialog>
            <Dialog.Title>Import tab separated data from Excel</Dialog.Title>
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
                    onKeyDown={handleKeyDown}
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

export default Import
