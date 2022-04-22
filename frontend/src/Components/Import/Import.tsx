import React, { useState, useEffect } from "react"
import styled from "styled-components"
import {
    Button, Dialog, Input, Typography,
} from "@equinor/eds-core-react"
import { useParams } from "react-router"
import { Case } from "../../models/Case"
import { Project } from "../../models/Project"
import { GetProjectService } from "../../Services/ProjectService"

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
    const [, setProject] = useState<Project>()
    const params = useParams()
    const [caseItem, setCase] = useState<Case>()

    const example: string = "value1\tvalue2\tvalue3\tvalue4"

    const dG4Date: number = caseItem?.DG4Date?.getFullYear()!
    const startYearImport: number = Number(dG4Date) + Number(startYear)

    const unwrapProjectId = (projectId?: string | undefined): string => {
        if (projectId === undefined || projectId === null) {
            throw new Error("Attempted to Import Timeseries onto a Project which does not exist")
        }
        return projectId
    }

    const unwrapCaseId = (caseId?: string | undefined): string => {
        if (caseId === undefined || caseId === null) {
            throw new Error("Attempted to Import Timeseries onto a Case Id which does not exist")
        }
        return caseId
    }

    const unwrapCase = (casee?: Case | undefined): Case => {
        if (casee === undefined || casee === null) {
            throw new Error("Attempted to Import Timeseries onto a Case which does not exist")
        }
        return casee
    }

    useEffect(() => {
        (async () => {
            try {
                const projectId: string = unwrapProjectId(params.projectId)
                const caseId: string = unwrapCaseId(params.caseId)
                const projectResult: Project = await GetProjectService().getProjectByID(projectId)
                setProject(projectResult)
                const caseResult: Case = unwrapCase(projectResult.cases.find((o) => o.id === caseId))
                setCase(caseResult)
            } catch (error) {
                console.error(`[CaseView] Error while fetching project ${params.projectId}`, error)
            }
        })()
    }, [params.projectId, params.caseId])

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
