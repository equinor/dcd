import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Typography,
} from "@equinor/eds-core-react"
import CaseDateField from "../../Components/Case/CaseDateField"
import {
    DateFromString,
    DefaultDate,
    IsDefaultDate,
    IsDefaultDateString,
    ToMonthDate,
} from "../../Utils/common"

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
    caseItem: Components.Schemas.CaseDto,
    setCase: Dispatch<SetStateAction<Components.Schemas.CaseDto | undefined>>,
    activeTab: number
}

function CaseScheduleTab({
    caseItem,
    setCase,
    activeTab,
}: Props) {
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG0Date = DefaultDate().toISOString()
            setCase(newCase)
            return
        }
        newCase.dG0Date = new Date(e.target.value).toISOString()
        if (newCase.dG1Date && IsDefaultDateString(newCase.dG1Date)) {
            const dg = new Date(newCase.dG0Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG1Date = dg.toISOString()
        }
        if (IsDefaultDateString(newCase.dG2Date)) {
            const dg = new Date(newCase.dG1Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG2Date = dg.toISOString()
        }
        if (IsDefaultDateString(newCase.dG3Date)) {
            const dg = new Date(newCase.dG2Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG3Date = dg.toISOString()
        }
        if (IsDefaultDateString(newCase.dG4Date)) {
            const dg = new Date(newCase.dG3Date)
            dg.setMonth(dg.getMonth() + 36)
            newCase.dG4Date = dg.toISOString()
        }
        setCase(newCase)
    }

    const handleDG1Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG1Date = DefaultDate().toISOString()
        } else {
            newCase.dG1Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG2Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG2Date = DefaultDate().toISOString()
        } else {
            newCase.dG2Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG3Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG3Date = DefaultDate().toISOString()
        } else {
            newCase.dG3Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG4Date = DefaultDate().toISOString()
        } else {
            newCase.dG4Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGAChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgaDate = DefaultDate().toISOString()
        } else {
            newCase.dgaDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGBChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgbDate = DefaultDate().toISOString()
        } else {
            newCase.dgbDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGCChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgcDate = DefaultDate().toISOString()
        } else {
            newCase.dgcDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleAPXChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apxDate = DefaultDate().toISOString()
        } else {
            newCase.apxDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleAPZChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apzDate = DefaultDate().toISOString()
        } else {
            newCase.apzDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
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

    const getDatesFromStrings = (dateStrings: string[]) => {
        const dates = dateStrings.map((d) => new Date(d))
        return dates
    }

    if (activeTab !== 2) { return null }

    return (
        <>
            <TopWrapper>
                <PageTitle variant="h3">Schedule</PageTitle>
            </TopWrapper>
            <ColumnWrapper>
                <RowWrapper>
                    <CaseDateField
                        onChange={handleDGAChange}
                        value={DateFromString(caseItem.dgaDate)}
                        label="DGA"
                    />
                    <CaseDateField
                        onChange={handleDGBChange}
                        value={DateFromString(caseItem.dgbDate)}
                        label="DGB"
                    />
                    <CaseDateField
                        onChange={handleDGCChange}
                        value={DateFromString(caseItem.dgcDate)}
                        label="DGC"
                    />
                    <CaseDateField
                        onChange={handleAPXChange}
                        value={DateFromString(caseItem.apxDate)}
                        label="APX"
                    />
                    <CaseDateField
                        onChange={handleAPZChange}
                        value={DateFromString(caseItem.apzDate)}
                        label="APZ"
                    />
                </RowWrapper>
                <RowWrapper>
                    <CaseDateField
                        onChange={handleDG0Change}
                        value={DateFromString(caseItem.dG0Date)}
                        label="DG0"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG1Date, caseItem.dG2Date, caseItem.dG3Date, caseItem.dG4Date]))}
                        min={undefined}
                    />
                    <CaseDateField
                        onChange={handleDG1Change}
                        value={DateFromString(caseItem.dG1Date)}
                        label="DG1"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG2Date, caseItem.dG3Date, caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date]))}
                    />
                    <CaseDateField
                        onChange={handleDG2Change}
                        value={DateFromString(caseItem.dG2Date)}
                        label="DG2"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG3Date, caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date]))}
                    />
                    <CaseDateField
                        onChange={handleDG3Change}
                        value={DateFromString(caseItem.dG3Date)}
                        label="DG3"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date, caseItem.dG2Date]))}
                    />
                    <CaseDateField
                        onChange={handleDG4Change}
                        value={DateFromString(caseItem.dG4Date)}
                        label="DG4"
                        max={undefined}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date, caseItem.dG2Date, caseItem.dG3Date]))}
                    />
                </RowWrapper>
            </ColumnWrapper>
        </>
    )
}

export default CaseScheduleTab
