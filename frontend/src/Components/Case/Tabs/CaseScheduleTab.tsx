import Grid from "@mui/material/Grid"
import { v4 as uuidv4 } from "uuid"
import { useParams } from "react-router"
import { useQueryClient, useQuery } from "react-query"
import {
    dateFromString,
    defaultDate,
    isDefaultDate,
    isDefaultDateString,
    toMonthDate,
    formatDate,
} from "../../../Utils/common"
import { useAppContext } from "../../../Context/AppContext"
import SwitchableDateInput from "../../Input/SwitchableDateInput"
import { ResourcePropertyKey } from "../../../Models/Interfaces"
import useDataEdits from "../../../Hooks/useDataEdits"
import { useProjectContext } from "../../../Context/ProjectContext"

const CaseScheduleTab = () => {
    const { project } = useProjectContext()
    const { addEdit } = useDataEdits()
    const { caseId } = useParams()
    const queryClient = useQueryClient()
    const projectId = project?.id || null

    const { data: caseData } = useQuery<Components.Schemas.CaseDto | undefined>(
        [{ projectId, caseId, resourceId: "" }],
        () => queryClient.getQueryData([{ projectId, caseId, resourceId: "" }]),
        {
            enabled: !!project && !!projectId,
            initialData: () => queryClient.getQueryData([{ projectId: project?.id, caseId, resourceId: "" }]) as Components.Schemas.CaseDto,
        },
    )

    const { editMode } = useAppContext()

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

    function handleDG0Change(newDate: string) {
        /*
        if (!projectCaseEdited) { return }

        const newCase = { ...projectCaseEdited }

        const newDate = new Date(dateValue)
        if (Number.isNaN(newDate.getTime())) {
            newCase.dG0Date = defaultDate().toISOString()
            setProjectCaseEdited(newCase)
            return
        }

        newCase.dG0Date = new Date(newDate).toISOString()
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
        setProjectCaseEdited(newCase) */
    }

    function handleDateChange(dateKey: string, dateValue: string) {
        console.log("dateKey", dateKey)
        console.log("dateValue", dateValue)

        if (!caseData) { return }

        const newDate = Number.isNaN(new Date(dateValue).getTime())
            ? defaultDate().toISOString()
            : new Date(dateValue).toISOString()

        /*
        const newDate = new Date(dateValue)

        if (dateKey === "dG0Date") {
            handleDG0Change(dateValue)
        }

        if (Number.isNaN(newDate.getTime())) {
            console.log("1")
            setProjectCaseEdited({
                ...projectCaseEdited,
                [dateKey]: defaultDate().toISOString(),
            })
        } else {
            console.log("2")
            setProjectCaseEdited({
                ...projectCaseEdited,
                [dateKey]: new Date(dateValue).toISOString(),
            })
        } */

        if (dateKey === "dG0Date") {
            handleDG0Change(newDate)
        } else {
            /*
            setProjectCaseEdited({
                ...projectCaseEdited,
                [dateKey]: newDate,
            })
            */
            const caseDataObject = caseData as any

            addEdit({
                newValue: newDate,
                previousValue: caseDataObject[dateKey],
                inputLabel: dateKey,
                projectId: caseData.projectId,
                resourceName: "case",
                resourcePropertyKey: dateKey as ResourcePropertyKey,
                caseId: caseData.id,
                newDisplayValue: formatDate(newDate),
                previousDisplayValue: formatDate(caseDataObject[dateKey]),
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
        if (!caseData) { return defaultDate().toISOString() }
        const caseDataObject = caseData as any

        if (!isDefaultDateString(String(caseDataObject[dateKey as keyof typeof caseDataObject]))) {
            return toScheduleValue(caseDataObject[dateKey as keyof typeof caseDataObject])
        }
        return defaultDate().toISOString()
    }

    if (!caseData || !projectId) {
        return (<p>Loading...</p>)
    }

    return (
        <Grid container spacing={2}>
            {caseDateKeys
                .filter((caseDateKey) => Object.keys(caseData)
                    .filter((projectCaseKey) => caseDateKey.key === projectCaseKey))
                .map((caseDate) => (
                    (caseDate.visible
                        || editMode
                        || toScheduleValue(caseData[caseDate.key as keyof typeof caseData])
                    )
                        ? (
                            <Grid item xs={12} md={6} lg={6} key={uuidv4()}>
                                <SwitchableDateInput
                                    value={getDateValue(caseDate.key)}
                                    resourcePropertyKey={caseDate.key as ResourcePropertyKey}
                                    label={caseDate.label}
                                    onChange={(e) => (handleDateChange(caseDate.key, e.target.value))}
                                    min={
                                        (caseDate.min && caseData)
                                            ? findMinDate(getDatesFromStrings(caseDate.min.map((minDate) => caseData[minDate as keyof typeof caseData])))
                                            : undefined
                                    }
                                    max={
                                        (caseDate.max && caseData)
                                            ? findMaxDate(getDatesFromStrings(caseDate.max.map((maxDate) => caseData[maxDate as keyof typeof caseData])))
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
