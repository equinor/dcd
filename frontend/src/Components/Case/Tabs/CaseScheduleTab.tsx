import Grid from "@mui/material/Grid"
import {
    dateFromString,
    defaultDate,
    isDefaultDate,
    isDefaultDateString,
    toMonthDate,
} from "../../../Utils/common"
import { useCaseContext } from "../../../Context/CaseContext"
import { useAppContext } from "../../../Context/AppContext"
import SwitchableDateInput from "../../Input/SwitchableDateInput"

const CaseScheduleTab = () => {
    const { projectCase, projectCaseEdited, setProjectCaseEdited } = useCaseContext()
    const { editMode } = useAppContext()
    if (!projectCase) { return null }
    const caseDateKeys = [
        {
            label: "DGA",
            key: "dgaDate",
        },
        {
            label: "DGB",
            key: "dgbDate",
        },
        {
            label: "DGC",
            key: "dgcDate",
        },
        {
            label: "APX",
            key: "apxDate",
        },
        {
            label: "APZ",
            key: "apzDate",
        },
        {
            visible: true,
            label: "DG0",
            key: "dG0Date",
            max: ["dG1Date", "dG2Date", "dG3Date", "dG4Date"],
        },
        {
            visible: true,
            label: "DG1",
            key: "dG1Date",
            min: ["dG0Date"],
            max: ["dG2Date", "dG3Date", "dG4Date"],
        },
        {
            visible: true,
            label: "DG2",
            key: "dG2Date",
            min: ["dG0Date", "dG1Date"],
            max: ["dG3Date", "dG4Date"],
        },
        {
            visible: true,
            label: "DG3",
            key: "dG3Date",
            min: ["dG0Date", "dG1Date", "dG2Date"],
            max: ["dG4Date"],
        },
        {
            visible: true,
            label: "DG4",
            key: "dG4Date",
            min: ["dG0Date", "dG1Date", "dG2Date", "dG3Date"],
        },
    ]

    function handleDG0Change(dateValue: string) {
        if (!projectCaseEdited) { return }
        const newCase = { ...projectCaseEdited }
        const newDate = new Date(dateValue)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG0Date = defaultDate().toISOString()
            setProjectCaseEdited(newCase)
            return
        }
        newCase.dG0Date = new Date(dateValue).toISOString()
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

    function handleDateChange(dateKey: string, dateValue: string) {
        if (!projectCaseEdited) { return }
        const newDate = new Date(dateValue)
        if (dateKey === "dG0Date") {
            handleDG0Change(dateValue)
            return
        }
        if (Number.isNaN(newDate.getTime())) {
            setProjectCaseEdited({
                ...projectCaseEdited,
                [dateKey]: defaultDate().toISOString(),
            })
        } else {
            setProjectCaseEdited({
                ...projectCaseEdited,
                [dateKey]: new Date(dateValue).toISOString(),
            })
        }
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

    const getDatesFromStrings = (dateStrings: any[]) => {
        const dates = dateStrings.map((d) => new Date(String(d)))
        return dates
    }

    const toScheduleValue = (date: string | number | boolean | Components.Schemas.CapexYear | null | undefined) => {
        const paramString = String(date)
        const dateString = dateFromString(paramString)
        if (isDefaultDate(dateString)) {
            return undefined
        }
        return toMonthDate(dateString)
    }

    const getDateValue = (dateKey: string) => {
        if (projectCaseEdited) {
            return toScheduleValue(projectCaseEdited[dateKey as keyof typeof projectCaseEdited])
        }
        if (!isDefaultDateString(String(projectCase[dateKey as keyof typeof projectCase]))) {
            return toScheduleValue(projectCase[dateKey as keyof typeof projectCase])
        }
        return defaultDate().toISOString()
    }

    return (
        <Grid container spacing={2}>
            {caseDateKeys
                .filter((caseDateKey) => Object.keys(projectCase)
                    .filter((projectCaseKey) => caseDateKey.key === projectCaseKey))
                .map((caseDate) => (
                    (caseDate.visible
                    || editMode
                    || toScheduleValue(projectCase[caseDate.key as keyof typeof projectCase])
                    )
                        ? (
                            <Grid item xs={12} md={6} lg={6}>
                                <SwitchableDateInput
                                    value={getDateValue(caseDate.key)}
                                    objectKey={caseDate.key}
                                    label={caseDate.label}
                                    onChange={(e) => (handleDateChange(caseDate.key, e.target.value))}
                                    min={
                                        (caseDate.min && projectCaseEdited)
                                            ? findMinDate(getDatesFromStrings(caseDate.min.map((minDate) => projectCaseEdited[minDate as keyof typeof projectCaseEdited])))
                                            : undefined
                                    }
                                    max={
                                        (caseDate.max && projectCaseEdited)
                                            ? findMaxDate(getDatesFromStrings(caseDate.max.map((maxDate) => projectCaseEdited[maxDate as keyof typeof projectCaseEdited])))
                                            : undefined
                                    }
                                />
                            </Grid>
                        )
                        : null
                ))}
        </Grid>
    )
}

export default CaseScheduleTab
