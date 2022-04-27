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
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetCaseService } from "../Services/CaseService"

const DgField = styled.div`
    margin-bottom: 2.5rem;
    width: 12rem;
    display: flex;
`

const Wrapper = styled.div`
    display: flex;
    > *:not(:last-child) {
        margin-right: 1rem;
    }
    flex-direction: row;
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
}

const CaseDGDate = ({
    setProject,
    caseItem,
    setCase,
}: Props) => {
    const params = useParams()
    const [caseDg1Date, setCaseDg1Date] = useState<Date>()
    const [caseDg2Date, setCaseDg2Date] = useState<Date>()
    const [caseDg3Date, setCaseDg3Date] = useState<Date>()
    const [caseDg4Date, setCaseDg4Date] = useState<Date>()
    const [dg1DateWarning, setDg1DateWarning] = useState<string>("")
    const [dg2DateWarning, setDg2DateWarning] = useState<string>("")
    const [dg3DateWarning, setDg3DateWarning] = useState<string>("")

    useEffect(() => {
        (async () => {
            setCaseDg1Date(undefined)
            setCaseDg2Date(undefined)
            setCaseDg3Date(undefined)
            setCaseDg4Date(undefined)
        })()
    }, [params.projectId, params.caseId])

    const resetWarning = () => {
        setDg1DateWarning("")
        setDg2DateWarning("")
        setDg3DateWarning("")
    }

    useEffect(() => {
        (async () => {
            if (caseDg1Date! >= caseItem?.DG2Date!) {
                return setDg1DateWarning("DG1 date cannot be higher than DG2 date. Saving blocked.")
            }
            if (caseDg2Date! >= caseItem?.DG3Date!) {
                return setDg2DateWarning("DG2 date cannot be higher than DG3 date. Saving blocked.")
            }
            if (caseDg3Date! >= caseItem?.DG4Date!) {
                return setDg3DateWarning("DG3 date cannot be higher than DG4 date. Saving blocked.")
            }
            return resetWarning()
        })()
    })

    const handleDg1FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg1Date(new Date(e.target.value))
    }

    const handleDg2FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg2Date(new Date(e.target.value))
    }

    const handleDg3FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg3Date(new Date(e.target.value))
    }

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg4Date(new Date(e.target.value))
    }

    const saveDg1Date: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const caseDto = Case.Copy(caseItem!)
            caseDto.DG1Date = caseDg1Date
            const newProject = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === params.caseId)
            setCase(caseResult)
            setCaseDg1Date(undefined)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const saveDg2Date: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const caseDto = Case.Copy(caseItem!)
            caseDto.DG2Date = caseDg2Date
            const newProject = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === params.caseId)
            setCase(caseResult)
            setCaseDg2Date(undefined)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const saveDg3Date: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const caseDto = Case.Copy(caseItem!)
            caseDto.DG3Date = caseDg3Date
            const newProject = await GetCaseService().updateCase(caseDto)
            setProject(newProject)
            const caseResult = newProject.cases.find((o) => o.id === params.caseId)
            setCase(caseResult)
            setCaseDg3Date(undefined)
        } catch (error) {
            console.error("[CaseView] error while submitting form data", error)
        }
    }

    const saveDg4Date: MouseEventHandler<HTMLButtonElement> = async (e) => {
        e.preventDefault()
        try {
            const caseDto = Case.Copy(caseItem!)
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

    const compareDGDates = () => {
        if (caseDg1Date! >= caseItem?.DG2Date!) {
            return true
        }
        if (caseDg2Date! >= caseItem?.DG3Date!) {
            return true
        }
        if (caseDg3Date! >= caseItem?.DG4Date!) {
            return true
        }
        return false
    }

    const disableAllButDG1 = () => {
        if (caseDg1Date! >= caseItem?.DG2Date!) {
            return true
        }
        return false
    }

    const disableAllButDG2 = () => {
        if (caseDg2Date! >= caseItem?.DG3Date!) {
            return true
        }
        return false
    }

    const disableAllButDG3 = () => {
        if (caseDg3Date! >= caseItem?.DG4Date!) {
            return true
        }
        return false
    }

    const dg1ReturnDate = () => caseItem?.DG1Date?.toLocaleDateString("en-CA")
    const dg2ReturnDate = () => caseItem?.DG2Date?.toLocaleDateString("en-CA")
    const dg3ReturnDate = () => caseItem?.DG3Date?.toLocaleDateString("en-CA")
    const dg4ReturnDate = () => caseItem?.DG4Date?.toLocaleDateString("en-CA")

    return (
        <>
            <Typography variant="h6" color="red">{dg1DateWarning}</Typography>
            <Typography variant="h6" color="red">{dg2DateWarning}</Typography>
            <Typography variant="h6" color="red">{dg3DateWarning}</Typography>

            <Wrapper>
                <Typography variant="h6">DG1</Typography>
                <DgField>
                    <Input
                        defaultValue={dg1ReturnDate()}
                        key={dg1ReturnDate()}
                        id="dgDate"
                        type="date"
                        name="dgDate"
                        onChange={handleDg1FieldChange}
                        readOnly={disableAllButDG2() || disableAllButDG3()}
                    />
                    <EdsProvider density="compact">
                        <ActionsContainer>
                            <Tooltip title="Save DG1 date">
                                <Button
                                    variant="ghost_icon"
                                    aria-label="Save DG1 date"
                                    onClick={saveDg1Date}
                                    disabled={
                                        caseDg1Date === undefined
                                        || compareDGDates()
                                    }
                                >
                                    <Icon data={save} />
                                </Button>
                            </Tooltip>
                        </ActionsContainer>
                    </EdsProvider>
                </DgField>
                <Typography variant="h6">DG3</Typography>
                <DgField>
                    <Input
                        defaultValue={dg3ReturnDate()}
                        key={dg3ReturnDate()}
                        id="dgDate"
                        type="date"
                        name="dgDate"
                        onChange={handleDg3FieldChange}
                        readOnly={disableAllButDG1() || disableAllButDG2()}
                    />
                    <EdsProvider density="compact">
                        <ActionsContainer>
                            <Tooltip title="Save DG3 date">
                                <Button
                                    variant="ghost_icon"
                                    aria-label="Save DG3 date"
                                    onClick={saveDg3Date}
                                    disabled={
                                        caseDg3Date === undefined
                                        || compareDGDates()
                                    }
                                >
                                    <Icon data={save} />
                                </Button>
                            </Tooltip>
                        </ActionsContainer>
                    </EdsProvider>
                </DgField>
            </Wrapper>
            <Wrapper>
                <Typography variant="h6">DG2</Typography>
                <DgField>
                    <Input
                        defaultValue={dg2ReturnDate()}
                        key={dg2ReturnDate()}
                        id="dgDate"
                        type="date"
                        name="dgDate"
                        onChange={handleDg2FieldChange}
                        readOnly={disableAllButDG1() || disableAllButDG3()}
                    />
                    <EdsProvider density="compact">
                        <ActionsContainer>
                            <Tooltip title="Save DG2 date">
                                <Button
                                    variant="ghost_icon"
                                    aria-label="Save DG2 date"
                                    onClick={saveDg2Date}
                                    disabled={
                                        caseDg2Date === undefined
                                        || compareDGDates()
                                    }
                                >
                                    <Icon data={save} />
                                </Button>
                            </Tooltip>
                        </ActionsContainer>
                    </EdsProvider>
                </DgField>
                <Typography variant="h6">DG4</Typography>
                <DgField>
                    <Input
                        defaultValue={dg4ReturnDate()}
                        key={dg4ReturnDate()}
                        id="dgDate"
                        type="date"
                        name="dgDate"
                        onChange={handleDg4FieldChange}
                        readOnly={disableAllButDG1() || disableAllButDG2() || disableAllButDG3()}
                    />
                    <EdsProvider density="compact">
                        <ActionsContainer>
                            <Tooltip title="Save DG4 date">
                                <Button
                                    variant="ghost_icon"
                                    aria-label="Save DG4 date"
                                    onClick={saveDg4Date}
                                    disabled={
                                        caseDg4Date === undefined
                                        || compareDGDates()
                                    }
                                >
                                    <Icon data={save} />
                                </Button>
                            </Tooltip>
                        </ActionsContainer>
                    </EdsProvider>
                </DgField>
            </Wrapper>
        </>
    )
}

export default CaseDGDate
