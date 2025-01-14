import Grid from "@mui/material/Grid"
import { v4 as uuidv4 } from "uuid"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"

import {
    isDefaultDateString,
    defaultDate,
    formatDate,
    isDefaultDate,
    toMonthDate,
    dateStringToDateUtc,
    dateFromTimestamp,
} from "@/Utils/DateUtils"
import CaseScheduleTabSkeleton from "@/Components/LoadingSkeletons/CaseScheduleTabSkeleton"
import { ResourceObject, ResourcePropertyKey } from "@/Models/Interfaces"
import SwitchableDateInput from "@/Components/Input/SwitchableDateInput"
import { useProjectContext } from "@/Context/ProjectContext"
import { caseQueryFn } from "@/Services/QueryFunctions"
import { useAppContext } from "@/Context/AppContext"

const CaseScheduleTab = ({ addEdit }: { addEdit: any }) => {
    const { editMode } = useAppContext()
    const { caseId, revisionId, tab } = useParams()
    const { projectId, isRevision } = useProjectContext()

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

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
            label: "APbo",
            key: "apboDate",
        },
        {
            label: "BOR",
            key: "borDate",
        },
        {
            label: "VPbo",
            key: "vpboDate",
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

    if (!apiData || !projectId) {
        return (<CaseScheduleTabSkeleton />)
    }

    const caseData = apiData.case

    const getDGOChangesObject = (newDate: Date): ResourceObject | undefined => {
        const newCaseObject = caseData
        newCaseObject.dG0Date = newDate.toISOString()

        if (newCaseObject.dG1Date && isDefaultDateString(newCaseObject.dG1Date)) {
            const dg = dateStringToDateUtc(newCaseObject.dG0Date)
            dg.setMonth(dg.getMonth() + 12)
            newCaseObject.dG1Date = dg.toISOString()
        }
        if (isDefaultDateString(newCaseObject.dG2Date)) {
            const dg = dateStringToDateUtc(newCaseObject.dG1Date)
            dg.setMonth(dg.getMonth() + 12)
            newCaseObject.dG2Date = dg.toISOString()
        }
        if (isDefaultDateString(newCaseObject.dG3Date)) {
            const dg = dateStringToDateUtc(newCaseObject.dG2Date)
            dg.setMonth(dg.getMonth() + 12)
            newCaseObject.dG3Date = dg.toISOString()
        }
        if (isDefaultDateString(newCaseObject.dG4Date)) {
            const dg = dateStringToDateUtc(newCaseObject.dG3Date)
            dg.setMonth(dg.getMonth() + 36)
            newCaseObject.dG4Date = dg.toISOString()
        }

        return newCaseObject
    }

    const getNewCaseObject = (dateKey: string, newDate: Date): ResourceObject => {
        const newCaseObject = caseData
        newCaseObject[dateKey as keyof typeof newCaseObject] = newDate.toISOString() as never // workaround for TS error

        return newCaseObject
    }

    function handleDateChange(dateKey: string, dateValue: string) {
        const dateValueYear = Number(dateValue.substring(0, 4))

        if ((dateValueYear >= 2010 && dateValueYear <= 2110) || dateValue === "") {
            const caseDataCopy: any = { ...caseData }
            const newDate = Number.isNaN(dateStringToDateUtc(dateValue).getTime())
                ? defaultDate()
                : dateStringToDateUtc(dateValue)
            const dg0Object = dateKey === "dG0Date" && getDGOChangesObject(newDate)

            addEdit({
                inputLabel: dateKey,
                projectId: caseData.projectId,
                resourceName: "case",
                resourcePropertyKey: dateKey as ResourcePropertyKey,
                caseId: caseData.caseId,
                newDisplayValue: formatDate(newDate.toISOString()),
                previousDisplayValue: formatDate(caseDataCopy[dateKey]),
                newResourceObject: dg0Object || getNewCaseObject(dateKey, newDate),
                previousResourceObject: dg0Object && caseDataCopy,
                tabName: tab,
            })
        }
    }
    const findMinDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !isDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const minDateValue = Math.max(
            ...filteredDates.map((date) => date.getTime()),
        )
        const minDate = dateFromTimestamp(minDateValue)
        return toMonthDate(minDate)
    }

    const findMaxDate = (dates: Date[]) => {
        const filteredDates = dates.filter((d) => !isDefaultDate(d))
        if (filteredDates.length === 0) { return undefined }

        const maxDateValue = Math.min(
            ...filteredDates.map((date) => date.getTime()),
        )
        const maxDate = dateFromTimestamp(maxDateValue)
        return toMonthDate(maxDate)
    }

    const getDatesFromStrings = (dateStrings: any[]) => {
        const dates = dateStrings.map((d) => dateStringToDateUtc(String(d)))
        return dates
    }

    const toScheduleValue = (date: string | number | boolean | null | undefined) => {
        const paramString = String(date)
        const dateString = dateStringToDateUtc(paramString)
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
                                    onChange={(e) => handleDateChange(caseDate.key as ResourcePropertyKey, e.target.value)}
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
