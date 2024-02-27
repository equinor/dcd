import {
    Dispatch,
    SetStateAction,
    ChangeEventHandler,
} from "react"
import styled from "styled-components"

import {
    Typography,
} from "@equinor/eds-core-react"
import CaseDateField from "../../Input/CaseDateInput"
import {
    dateFromString,
    defaultDate,
    isDefaultDate,
    isDefaultDateString,
    toMonthDate,
    formatDate,
} from "../../../Utils/common"
import InputContainer from "../../Input/Containers/InputContainer"
import InputSwitcher from "../../Input/InputSwitcher"

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

const CaseScheduleTab = ({
    caseItem,
    setCase,
    activeTab,
}: Props) => {
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG0Date = defaultDate().toISOString()
            setCase(newCase)
            return
        }
        newCase.dG0Date = new Date(e.target.value).toISOString()
        if (newCase.dG1Date && isDefaultDateString(newCase.dG1Date)) {
            const dg = new Date(newCase.dG0Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG1Date = dg.toISOString()
        }
        if (isDefaultDateString(newCase.dG2Date)) {
            const dg = new Date(newCase.dG1Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG2Date = dg.toISOString()
        }
        if (isDefaultDateString(newCase.dG3Date)) {
            const dg = new Date(newCase.dG2Date)
            dg.setMonth(dg.getMonth() + 12)
            newCase.dG3Date = dg.toISOString()
        }
        if (isDefaultDateString(newCase.dG4Date)) {
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
            newCase.dG1Date = defaultDate().toISOString()
        } else {
            newCase.dG1Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG2Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG2Date = defaultDate().toISOString()
        } else {
            newCase.dG2Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG3Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG3Date = defaultDate().toISOString()
        } else {
            newCase.dG3Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG4Date = defaultDate().toISOString()
        } else {
            newCase.dG4Date = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGAChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase: Components.Schemas.CaseDto = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgaDate = defaultDate().toISOString()
        } else {
            newCase.dgaDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGBChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgbDate = defaultDate().toISOString()
        } else {
            newCase.dgbDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleDGCChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgcDate = defaultDate().toISOString()
        } else {
            newCase.dgcDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleAPXChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apxDate = defaultDate().toISOString()
        } else {
            newCase.apxDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const handleAPZChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        const newCase = { ...caseItem }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apzDate = defaultDate().toISOString()
        } else {
            newCase.apzDate = new Date(e.target.value).toISOString()
        }
        setCase(newCase)
    }

    const findMinDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !isDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const minDateValue = Math.max(
            ...filteredDates.map((date) => date.getTime()),
        )
        const minDate = new Date(minDateValue)
        return toMonthDate(minDate)
    }

    const findMaxDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !isDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const maxDateValue = Math.min(
            ...filteredDates.map((date) => date.getTime()),
        )
        const maxDate = new Date(maxDateValue)
        return toMonthDate(maxDate)
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
            <InputContainer mobileColumns={1} desktopColumns={2} breakPoint={850}>

                <InputSwitcher
                    value={formatDate(caseItem.dgaDate)}
                    label="DGA"
                >
                    <CaseDateField
                        onChange={handleDGAChange}
                        value={dateFromString(caseItem.dgaDate)}
                        label="DGA"
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dgbDate)}
                    label="DGB"
                >
                    <CaseDateField
                        onChange={handleDGBChange}
                        value={dateFromString(caseItem.dgbDate)}
                        label="DGB"
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dgcDate)}
                    label="DGC"
                >
                    <CaseDateField
                        onChange={handleDGCChange}
                        value={dateFromString(caseItem.dgcDate)}
                        label="DGC"
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.apxDate)}
                    label="APX"
                >
                    <CaseDateField
                        onChange={handleAPXChange}
                        value={dateFromString(caseItem.apxDate)}
                        label="APX"
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.apzDate)}
                    label="APZ"
                >
                    <CaseDateField
                        onChange={handleAPZChange}
                        value={dateFromString(caseItem.apzDate)}
                        label="APZ"
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dG0Date)}
                    label="DG0"
                >

                    <CaseDateField
                        onChange={handleDG0Change}
                        value={dateFromString(caseItem.dG0Date)}
                        label="DG0"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG1Date, caseItem.dG2Date, caseItem.dG3Date, caseItem.dG4Date]))}
                        min={undefined}
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dG1Date)}
                    label="DG1"
                >

                    <CaseDateField
                        onChange={handleDG1Change}
                        value={dateFromString(caseItem.dG1Date)}
                        label="DG1"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG2Date, caseItem.dG3Date, caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date]))}
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dG2Date)}
                    label="DG2"
                >
                    <CaseDateField
                        onChange={handleDG2Change}
                        value={dateFromString(caseItem.dG2Date)}
                        label="DG2"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG3Date, caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date]))}
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dG3Date)}
                    label="DG3"
                >
                    <CaseDateField
                        onChange={handleDG3Change}
                        value={dateFromString(caseItem.dG3Date)}
                        label="DG3"
                        max={findMaxDate(getDatesFromStrings([caseItem.dG4Date]))}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date, caseItem.dG2Date]))}
                    />
                </InputSwitcher>
                <InputSwitcher
                    value={formatDate(caseItem.dG4Date)}
                    label="DG4"
                >
                    <CaseDateField
                        onChange={handleDG4Change}
                        value={dateFromString(caseItem.dG4Date)}
                        label="DG4"
                        max={undefined}
                        min={findMinDate(getDatesFromStrings([caseItem.dG0Date, caseItem.dG1Date, caseItem.dG2Date, caseItem.dG3Date]))}
                    />
                </InputSwitcher>
            </InputContainer>
        </>
    )
}

export default CaseScheduleTab
