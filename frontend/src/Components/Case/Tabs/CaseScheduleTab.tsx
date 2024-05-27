import {
    ChangeEventHandler,
} from "react"
import Grid from "@mui/material/Grid"
import {
    dateFromString,
    defaultDate,
    isDefaultDate,
    isDefaultDateString,
    toMonthDate,
} from "../../../Utils/common"
import { useCaseContext } from "../../../Context/CaseContext"
import SwitchableDateInput from "../../Input/SwitchableDateInput"

const CaseScheduleTab = () => {
    const { projectCase, projectCaseEdited, setProjectCaseEdited } = useCaseContext()
    if (!projectCase) { return null }
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG0Date = defaultDate().toISOString()
            setProjectCaseEdited(newCase)
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
        setProjectCaseEdited(newCase)
    }

    const handleDG1Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG1Date = defaultDate().toISOString()
        } else {
            newCase.dG1Date = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDG2Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG2Date = defaultDate().toISOString()
        } else {
            newCase.dG2Date = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDG3Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG3Date = defaultDate().toISOString()
        } else {
            newCase.dG3Date = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDG4Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG4Date = defaultDate().toISOString()
        } else {
            newCase.dG4Date = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDGAChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase: Components.Schemas.CaseDto = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgaDate = defaultDate().toISOString()
        } else {
            newCase.dgaDate = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDGBChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgbDate = defaultDate().toISOString()
        } else {
            newCase.dgbDate = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleDGCChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dgcDate = defaultDate().toISOString()
        } else {
            newCase.dgcDate = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleAPXChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apxDate = defaultDate().toISOString()
        } else {
            newCase.apxDate = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
    }

    const handleAPZChange: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(e.target.value)
        if (Number.isNaN(newDate.getTime())) {
            newCase.apzDate = defaultDate().toISOString()
        } else {
            newCase.apzDate = new Date(e.target.value).toISOString()
        }
        setProjectCaseEdited(newCase)
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

    const toScheduleValue = (date: string) => {
        const dateString = dateFromString(date)
        if (isDefaultDate(dateString)) {
            return undefined
        }
        return toMonthDate(dateString)
    }

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dgaDate : projectCase.dgaDate)}
                    objectKey={projectCase.dgaDate}
                    label="DGA"
                    onChange={handleDGAChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dgbDate : projectCase.dgbDate)}
                    objectKey={projectCase.dgbDate}
                    label="DGB"
                    onChange={handleDGBChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dgcDate : projectCase.dgcDate)}
                    objectKey={projectCase.dgcDate}
                    label="DGC"
                    onChange={handleDGCChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.apxDate : projectCase.apxDate)}
                    objectKey={projectCase.apxDate}
                    label="APX"
                    onChange={handleAPXChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.apzDate : projectCase.apzDate)}
                    objectKey={projectCase.apzDate}
                    label="APZ"
                    onChange={handleAPZChange}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dG0Date : projectCase.dG0Date)}
                    objectKey={projectCase.dG0Date}
                    label="DG0"
                    onChange={handleDG0Change}
                    max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG1Date, projectCaseEdited.dG2Date, projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                    min={undefined}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dG1Date : projectCase.dG1Date)}
                    objectKey={projectCase.dG1Date}
                    label="DG1"
                    onChange={handleDG1Change}
                    max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG2Date, projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                    min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date])) : undefined}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dG2Date : projectCase.dG2Date)}
                    objectKey={projectCase.dG2Date}
                    label="DG2"
                    onChange={handleDG2Change}
                    max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                    min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date])) : undefined}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dG3Date : projectCase.dG3Date)}
                    objectKey={projectCase.dG3Date}
                    label="DG3"
                    onChange={handleDG3Change}
                    max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG4Date])) : undefined}
                    min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date, projectCaseEdited.dG2Date])) : undefined}
                />
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <SwitchableDateInput
                    value={toScheduleValue(projectCaseEdited ? projectCaseEdited.dG4Date : projectCase.dG4Date)}
                    objectKey={projectCase.dG4Date}
                    label="DG4"
                    onChange={handleDG4Change}
                    max={undefined}
                    min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date, projectCaseEdited.dG2Date, projectCaseEdited.dG3Date])) : undefined}
                />
            </Grid>
        </Grid>
    )
}

export default CaseScheduleTab
