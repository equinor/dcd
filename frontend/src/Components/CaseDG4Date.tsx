import {
    Typography,
    Input,
    Button,
    EdsProvider,
    Tooltip,
    Icon,
} from "@equinor/eds-core-react"
import { save } from "@equinor/eds-icons"
import {
    useState,
    useEffect,
    ChangeEventHandler,
    MouseEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetCaseService } from "../Services/CaseService"
import { unwrapCase } from "../Utils/common"

const Dg4Field = styled.div`
    margin-bottom: 3.5rem;
    width: 12rem;
    display: flex;
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

interface Props {
    setProject: React.Dispatch<React.SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: React.Dispatch<React.SetStateAction<Case | undefined>>
}

const CaseDG4Date = ({
    setProject,
    caseItem,
    setCase,
}: Props) => {
    const params = useParams()
    const [caseDg4Date, setCaseDg4Date] = useState<Date>()

    useEffect(() => {
        (async () => {
            setCaseDg4Date(undefined)
        })()
    }, [params.projectId, params.caseId])

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg4Date(new Date(e.target.value))
    }

    const saveDg4Date: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const unwrappedCase: Case = unwrapCase(caseItem)
            const caseDto = Case.Copy(unwrappedCase)
            caseDto.DG4Date = caseDg4Date
            const newProject = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === params.caseId)
            setCase(caseResult)
            setCaseDg4Date(undefined)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const dg4ReturnDate = () => caseItem?.DG4Date?.toLocaleDateString("en-CA")

    return (
        <>
            <Typography variant="h6">DG4</Typography>
            <Dg4Field>
                <Input
                    defaultValue={dg4ReturnDate()}
                    key={dg4ReturnDate()}
                    id="dg4Date"
                    type="date"
                    name="dg4Date"
                    onChange={handleDg4FieldChange}
                />
                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title="Save DG4 date">
                            <Button
                                variant="ghost_icon"
                                aria-label="Save DG4 date"
                                onClick={saveDg4Date}
                                disabled={caseDg4Date === undefined}
                            >
                                <Icon data={save} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </Dg4Field>
        </>
    )
}

export default CaseDG4Date
