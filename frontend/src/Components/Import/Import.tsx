import React, { useState, useEffect } from "react"
import styled from "styled-components"
import {
    Button, Dialog, Input, Typography,
} from "@equinor/eds-core-react"
import { useParams } from "react-router"
import { Case } from "../../models/Case"
import { Project } from "../../models/Project"
import { GetProjectService } from "../../Services/ProjectService"
import { unwrapCase, unwrapCaseId, unwrapProjectId } from "../../Utils/common"

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
    const { fusionProjectId, caseId } = useParams<Record<string, string | undefined>>()
    const [dataInput, setDataInput] = useState<string>("")
    const [startYear, setStartYear] = useState(0)
    const [, setProject] = useState<Project>()
    const [caseItem, setCase] = useState<Case>()

    const example = "value1\tvalue2\tvalue3\tvalue4"

    const dG4Date: number = caseItem?.DG4Date?.getFullYear()!
    const startYearImport: number = Number(dG4Date) + Number(startYear)

    useEffect(() => {
        (async () => {
            try {
                const projectId = unwrapProjectId(fusionProjectId)
                // eslint-disable-next-line no-underscore-dangle
                const _caseId = unwrapCaseId(caseId)
                const projectResult = await (await GetProjectService()).getProjectByID(projectId)
                setProject(projectResult)
                const caseResult = unwrapCase(projectResult.cases.find((o) => o.id === _caseId))
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${fusionProjectId}`, error)
            }
        })()
    }, [])

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
                <Typography variant="h6">
                    Start year is:
                    {" "}
                    {startYearImport}
                </Typography>
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
