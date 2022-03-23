import {
    Typography,
    Input,
} from "@equinor/eds-core-react"
import {
    useState,
    ChangeEventHandler,
} from "react"
import { useParams } from "react-router-dom"
import styled from "styled-components"
import { Project } from "../models/Project"
import { Case } from "../models/Case"
import { GetCaseService } from "../Services/CaseService"

const Dg4Field = styled.div`
    margin-bottom: 3.5rem;
    width: 10rem;
    display: flex;
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
    const [, setCaseDg4Date] = useState<Date>()

    const handleDg4FieldChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        setCaseDg4Date(new Date(e.target.value))
        const caseDto = Case.Copy(caseItem!)
        caseDto.DG4Date = new Date(e.target.value)
        const newProject = await GetCaseService().updateCase(caseDto)
        setProject(newProject)
        const caseResult = newProject.cases.find((o) => o.id === params.caseId)
        setCase(caseResult)
    }

    const dg4ReturnDate = () => {
        const dg4DateGet = caseItem?.DG4Date?.toLocaleDateString("en-CA")
        if (dg4DateGet !== "0001-01-01") {
            return dg4DateGet
        }
        return ""
    }

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
            </Dg4Field>
        </>
    )
}

export default CaseDG4Date
