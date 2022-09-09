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
    Dispatch,
    SetStateAction,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { GetCaseService } from "../../Services/CaseService"
import { IsInvalidDate, ToMonthDate, unwrapCase } from "../../Utils/common"
import DGEnum from "../../models/DGEnum"

const DgField = styled.div`
    margin-bottom: 2.5rem;
    width: 12rem;
    display: flex;
`

const ActionsContainer = styled.div`
    > *:not(:last-child) {
        margin-right: 0.5rem;
    }
`

interface Props {
    setProject: Dispatch<SetStateAction<Project | undefined>>
    caseItem: Case | undefined,
    setCase: Dispatch<SetStateAction<Case | undefined>>
    dGType: DGEnum,
    dGName: string,
}

const CaseDGDate = ({
    setProject,
    caseItem,
    setCase,
    dGType,
    dGName,
}: Props) => {
    const [caseDgDate, setCaseDgDate] = useState<Date>()

    const { caseId } = useParams<Record<string, string | undefined>>()

    useEffect(() => {
        (async () => {
            setCaseDgDate(undefined)
        })()
    }, [])

    const handleDgFieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDgDate(new Date(e.target.value))
    }

    const saveDgDate: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const unwrappedCase: Case = unwrapCase(caseItem)
            const caseDto = Case.Copy(unwrappedCase)
            caseDto[dGType] = caseDgDate
            const newProject = await (await GetCaseService()).updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === caseId)
            setCase(caseResult)
            setCaseDgDate(undefined)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const dgReturnDate = () => (IsInvalidDate(caseItem?.[dGType])
        ? undefined
        : ToMonthDate(caseItem?.[dGType]))

    const limitDateToNextDGDate = () => {
        if (dGType === DGEnum.DG0) {
            return IsInvalidDate(caseItem?.DG1Date) ? undefined : ToMonthDate(caseItem?.DG1Date)
        }
        if (dGType === DGEnum.DG1) {
            return IsInvalidDate(caseItem?.DG2Date) ? undefined : ToMonthDate(caseItem?.DG2Date)
        }
        if (dGType === DGEnum.DG2) {
            return IsInvalidDate(caseItem?.DG3Date) ? undefined : ToMonthDate(caseItem?.DG3Date)
        }
        if (dGType === DGEnum.DG3) {
            return IsInvalidDate(caseItem?.DG4Date) ? undefined : ToMonthDate(caseItem?.DG4Date)
        }
        return undefined
    }

    return (
        <>
            <Typography variant="h6">{dGName}</Typography>
            <DgField>
                <Input
                    type="month"
                    defaultValue={dgReturnDate()}
                    key={dgReturnDate()}
                    id="dgDate"
                    name="dgDate"
                    onChange={handleDgFieldChange}
                    max={limitDateToNextDGDate()}
                />
                <EdsProvider density="compact">
                    <ActionsContainer>
                        <Tooltip title={`Save ${dGName} date`}>
                            <Button
                                variant="ghost_icon"
                                aria-label={`Save ${dGName} date`}
                                onClick={saveDgDate}
                                disabled={caseDgDate === undefined}
                            >
                                <Icon data={save} />
                            </Button>
                        </Tooltip>
                    </ActionsContainer>
                </EdsProvider>
            </DgField>
        </>
    )
}

export default CaseDGDate
