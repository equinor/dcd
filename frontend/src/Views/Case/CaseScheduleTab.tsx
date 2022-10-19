import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Button, Typography,
} from "@equinor/eds-core-react"
import { Project } from "../../models/Project"
import { Case } from "../../models/case/Case"
import { GetCaseService } from "../../Services/CaseService"
import CaseDateField from "../../Components/Case/CaseDateField"
import { DefaultDate, IsDefaultDate, ToMonthDate } from "../../Utils/common"

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
    margin-top: 20px;
    margin-bottom: 20px;
`
const PageTitle = styled(Typography)`
    flex-grow: 1;
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
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DG0Date = DefaultDate()
            setCase(newCase)
            return
        }
        newCase.DG0Date = new Date(e.target.value)
        if (IsDefaultDate(newCase.DG1Date)) {
            const dg = new Date(newCase.DG0Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG1Date = dg
        }
        if (IsDefaultDate(newCase.DG2Date)) {
            const dg = new Date(newCase.DG1Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG2Date = dg
        }
        if (IsDefaultDate(newCase.DG3Date)) {
            const dg = new Date(newCase.DG2Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.DG3Date = dg
        }
        if (IsDefaultDate(newCase.DG4Date)) {
            const dg = new Date(newCase.DG3Date)
            dg.setMonth(dg.getMonth() + 36)
            newCase.DG4Date = dg
        }
        setCase(newCase)
    }

    const handleDG1Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DG1Date = DefaultDate()
        } else {
            newCase.DG1Date = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDG2Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DG2Date = DefaultDate()
        } else {
            newCase.DG2Date = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDG3Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DG3Date = DefaultDate()
        } else {
            newCase.DG3Date = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DG4Date = DefaultDate()
        } else {
            newCase.DG4Date = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDGAChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DGADate = DefaultDate()
        } else {
            newCase.DGADate = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDGBChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DGBDate = DefaultDate()
        } else {
            newCase.DGBDate = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleDGCChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.DGCDate = DefaultDate()
        } else {
            newCase.DGCDate = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleAPXChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.APXDate = DefaultDate()
        } else {
            newCase.APXDate = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleAPZChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.APZDate = DefaultDate()
        } else {
            newCase.APZDate = new Date(e.target.value)
        }
        setCase(newCase)
    }

    const handleSave = async () => {
        const result = await (await GetCaseService()).update(caseItem)
        setCase(result)
    }

    const findMinDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !IsDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const minDateValue = Math.max(
            ...filteredDates.map((date) => date.getTime()),
        )
        const minDate = new Date(minDateValue)
        return ToMonthDate(minDate)
    }

    const findMaxDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !IsDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const maxDateValue = Math.min(
            ...filteredDates.map((date) => date.getTime()),
        )
        const maxDate = new Date(maxDateValue)
        return ToMonthDate(maxDate)
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
                        onChange={handleDGAChange}
                        value={caseItem.DGADate}
                        label="DGA"
                    />
                    <CaseDateField
                        onChange={handleDGBChange}
                        value={caseItem.DGBDate}
                        label="DGB"
                    />
                    <CaseDateField
                        onChange={handleDGCChange}
                        value={caseItem.DGCDate}
                        label="DGC"
                    />
                    <CaseDateField
                        onChange={handleAPXChange}
                        value={caseItem.APXDate}
                        label="APX"
                    />
                    <CaseDateField
                        onChange={handleAPZChange}
                        value={caseItem.APZDate}
                        label="APZ"
                    />
                </RowWrapper>
                <RowWrapper>
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={caseItem.DG0Date}
                        label="DG0"
                        max={findMaxDate([caseItem.DG1Date, caseItem.DG2Date, caseItem.DG3Date, caseItem.DG4Date])}
                        min={undefined}
                    />
                    <CaseDateField
                        onChange={handleDG1Change}
                        value={caseItem.DG1Date}
                        label="DG1"
                        max={findMaxDate([caseItem.DG2Date, caseItem.DG3Date, caseItem.DG4Date])}
                        min={findMinDate([caseItem.DG0Date])}
                    />
                    <CaseDateField
                        onChange={handleDG2Change}
                        value={caseItem.DG2Date}
                        label="DG2"
                        max={findMaxDate([caseItem.DG3Date, caseItem.DG4Date])}
                        min={findMinDate([caseItem.DG0Date, caseItem.DG1Date])}
                    />
                    <CaseDateField
                        onChange={handleDG3Change}
                        value={caseItem.DG3Date}
                        label="DG3"
                        max={findMaxDate([caseItem.DG4Date])}
                        min={findMinDate([caseItem.DG0Date, caseItem.DG1Date, caseItem.DG2Date])}
                    />
                    <CaseDateField
                        onChange={handleDG4Change}
                        value={caseItem.DG4Date}
                        label="DG4"
                        max={undefined}
                        min={findMinDate([caseItem.DG0Date, caseItem.DG1Date, caseItem.DG2Date, caseItem.DG3Date])}
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseScheduleTab
