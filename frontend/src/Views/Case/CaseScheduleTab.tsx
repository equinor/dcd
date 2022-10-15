import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, Label, NativeSelect, Typography,
} from "@equinor/eds-core-react"
import TextArea from "@equinor/fusion-react-textarea/dist/TextArea"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { GetCaseService } from "../../Services/CaseService"
import CaseNumberInput from "../../Components/Case/CaseNumberInput"
import CaseDateField from "../../Components/Case/CaseDateField"
import { DefaultDate, IsInvalidDate } from "../../Utils/common"

const ColumnWrapper = styled.div`
    display: flex;
    flex-direction: column;
`
const RowWrapper = styled.div`
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    margin-bottom: 78px;
`
const TopWrapper = styled.div`
    display: flex;
    flex-direction: row;
    padding: 1.5rem 2rem;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
`
const DescriptionField = styled(TextArea)`
    margin-bottom: 50px;
`
const NativeSelectField = styled(NativeSelect)`
    width: 250px;
`

interface Props {
    project: Project,
    setProject: Dispatch<SetStateAction<Project | undefined>>,
    caseItem: Case,
    setCase: Dispatch<SetStateAction<Case | undefined>>,
}

function CaseScheduleTab({
    project,
    setProject,
    caseItem,
    setCase,
}: Props) {
    /*
    DG0 = Next month
    DG1 = DG0 + 12 months
    DG2 = DG1 + 12 months
    DG3 = DG2 + 12 months
    DG4 = DG3 + 36 months
    */
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        newCase.DG0Date = new Date(e.target.value)
        if (IsInvalidDate(newCase.DG1Date)) {
            const dg = new Date(newCase.DG0Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG1Date = dg
        }
        if (IsInvalidDate(newCase.DG2Date)) {
            const dg = new Date(newCase.DG1Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG2Date = dg
        }
        if (IsInvalidDate(newCase.DG3Date)) {
            const dg = new Date(newCase.DG2Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG3Date = dg
        }
        if (IsInvalidDate(newCase.DG4Date)) {
            const dg = new Date(newCase.DG3Date)
            dg.setMonth(dg.getMonth() + 36)
            newCase.DG4Date = dg
        }
        setCase(newCase)
    }

    const handleSave = async () => {
        const result = await (await GetCaseService()).update(caseItem)
        setCase(result)
    }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Schedule</PageTitle>
                <Button onClick={handleSave}>Save</Button>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={caseItem.DG0Date}
                        label="DG0"
                    />
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={caseItem.DG0Date}
                        label="DG0"
                    />
                </RowWrapper>
                <RowWrapper>
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={caseItem.DG0Date}
                        label="DG0"
                    />
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={caseItem.DG0Date}
                        label="DG0"
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseScheduleTab
