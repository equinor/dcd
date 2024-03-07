import {
    ChangeEventHandler,
} from "react"
import { Input } from "@equinor/eds-core-react"
import Grid from "@mui/material/Grid"
import {
    dateFromString,
    defaultDate,
    isDefaultDate,
    isDefaultDateString,
    toMonthDate,
    formatDate,
} from "../../../Utils/common"
import InputSwitcher from "../../Input/InputSwitcher"
import { useCaseContext } from "../../../Context/CaseContext"

const CaseScheduleTab = () => {
    const { projectCase, projectCaseEdited, setProjectCaseEdited } = useCaseContext()
    if (!projectCase) return (<></>)
    const handleDG0Change: ChangeEventHandler<HTMLInputElement> = async (e) => {
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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
        if (!projectCaseEdited) return
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

    return (
        <Grid container spacing={2}>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dgaDate)}
                    label="DGA"
                >
                    <Input
                        type="month"
                        id="dgaDate"
                        name="dgaDate"
                        onChange={handleDGAChange}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dgaDate : projectCase.dgaDate)}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dgbDate)}
                    label="DGB"
                >
                    <Input
                        type="month"
                        id="dgbDate"
                        name="dgbDate"
                        onChange={handleDGBChange}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dgbDate : projectCase.dgbDate)}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dgcDate)}
                    label="DGC"
                >
                    <Input
                        type="month"
                        id="dgcDate"
                        name="dgcDate"
                        onChange={handleDGCChange}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dgcDate : projectCase.dgcDate)}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.apxDate)}
                    label="APX"
                >
                    <Input
                        type="month"
                        id="apxDate"
                        name="apxDate"
                        onChange={handleAPXChange}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.apxDate : projectCase.apxDate)}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.apzDate)}
                    label="APZ"
                >
                    <Input
                        type="month"
                        id="apzDate"
                        name="apzDate"
                        onChange={handleAPZChange}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.apzDate : projectCase.apzDate)}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dG0Date)}
                    label="DG0"
                >

                    <Input
                        type="month"
                        id="dg0Date"
                        name="dg0Date"
                        onChange={handleDG0Change}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dG0Date : projectCase.dG0Date)}
                        max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG1Date, projectCaseEdited.dG2Date, projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                        min={undefined}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dG1Date)}
                    label="DG1"
                >

                    <Input
                        type="month"
                        id="dg1Date"
                        name="dg1Date"
                        onChange={handleDG1Change}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dG1Date : projectCase.dG1Date)}
                        max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG2Date, projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                        min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date])) : undefined}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dG2Date)}
                    label="DG2"
                >
                    <Input
                        type="month"
                        id="dg2Date"
                        name="dg2Date"
                        onChange={handleDG2Change}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dG2Date : projectCase.dG2Date)}
                        max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG3Date, projectCaseEdited.dG4Date])) : undefined}
                        min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date])) : undefined}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dG3Date)}
                    label="DG3"
                >
                    <Input
                        type="month"
                        id="dg3Date"
                        name="dg3Date"
                        onChange={handleDG3Change}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dG3Date : projectCase.dG3Date)}
                        max={projectCaseEdited ? findMaxDate(getDatesFromStrings([projectCaseEdited.dG4Date])) : undefined}
                        min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date, projectCaseEdited.dG2Date])) : undefined}
                    />
                </InputSwitcher>
            </Grid>
            <Grid item xs={12} md={6} lg={6}>
                <InputSwitcher
                    value={formatDate(projectCase.dG4Date)}
                    label="DG4"
                >
                    <Input
                        type="month"
                        id="dg4Date"
                        name="dg4Date"
                        onChange={handleDG4Change}
                        value={dateFromString(projectCaseEdited ? projectCaseEdited.dG3Date : projectCase.dG3Date)}
                        max={undefined}
                        min={projectCaseEdited ? findMinDate(getDatesFromStrings([projectCaseEdited.dG0Date, projectCaseEdited.dG1Date, projectCaseEdited.dG2Date, projectCaseEdited.dG3Date])) : undefined}
                    />
                </InputSwitcher>
            </Grid>
        </Grid>
    )
}

export default CaseScheduleTab
