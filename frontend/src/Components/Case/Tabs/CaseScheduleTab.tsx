import Grid from "@mui/material/Grid2"
import { v4 as uuidv4 } from "uuid"
import { useParams } from "react-router"
import { useQuery } from "@tanstack/react-query"
import styled from "styled-components"

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

const TabContainer = styled(Grid)`
    max-width: 800px;
`

// Types for Decision Gate structure
interface DecisionGate {
    label: string
    key: string
    monthsAfterPrevious?: number
    dependsOn?: string[]
    cannotBeAfter?: string[]
    visible?: boolean
}

// Define the Decision Gate sequence and their relationships
const DECISION_GATES: DecisionGate[] = [
    { label: "DGA", key: "dgaDate" },
    { label: "DGB", key: "dgbDate" },
    { label: "DGC", key: "dgcDate" },
    { label: "APbo", key: "apboDate" },
    { label: "BOR", key: "borDate" },
    { label: "VPbo", key: "vpboDate" },
    {
        label: "DG0",
        key: "dG0Date",
        cannotBeAfter: ["dG1Date", "dG2Date", "dG3Date", "dG4Date"],
        visible: true,
    },
    {
        label: "DG1",
        key: "dG1Date",
        monthsAfterPrevious: 12,
        dependsOn: ["dG0Date"],
        cannotBeAfter: ["dG2Date", "dG3Date", "dG4Date"],
        visible: true,
    },
    {
        label: "DG2",
        key: "dG2Date",
        monthsAfterPrevious: 12,
        dependsOn: ["dG0Date", "dG1Date"],
        cannotBeAfter: ["dG3Date", "dG4Date"],
        visible: true,
    },
    {
        label: "DG3",
        key: "dG3Date",
        monthsAfterPrevious: 12,
        dependsOn: ["dG0Date", "dG1Date", "dG2Date"],
        cannotBeAfter: ["dG4Date"],
        visible: true,
    },
    {
        label: "DG4",
        key: "dG4Date",
        monthsAfterPrevious: 36,
        dependsOn: ["dG0Date", "dG1Date", "dG2Date", "dG3Date"],
        visible: true,
    },
]

const CaseScheduleTab = ({ addEdit }: { addEdit: any }) => {
    const { editMode } = useAppContext()
    const { caseId, revisionId, tab } = useParams()
    const { projectId, isRevision } = useProjectContext()

    const { data: apiData } = useQuery({
        queryKey: ["caseApiData", isRevision ? revisionId : projectId, caseId],
        queryFn: () => caseQueryFn(isRevision ? revisionId ?? "" : projectId, caseId),
        enabled: !!projectId && !!caseId,
    })

    if (!apiData || !projectId) {
        return (<CaseScheduleTabSkeleton />)
    }

    const caseData = apiData.case

    const calculateNextDate = (currentDate: Date, monthsToAdd: number): Date => {
        const nextDate = dateStringToDateUtc(currentDate.toISOString())
        nextDate.setMonth(nextDate.getMonth() + monthsToAdd)
        return nextDate
    }

    const updateCascadingDates = (startGate: DecisionGate, newDate: Date, currentData: any): ResourceObject => {
        const updatedData = { ...currentData }
        let currentDate = newDate

        // Find all subsequent gates that need updating
        const subsequentGates = DECISION_GATES
            .slice(DECISION_GATES.indexOf(startGate) + 1)
            .filter((gate) => gate.monthsAfterPrevious)

        // Update each subsequent gate if it's using the default date
        subsequentGates.forEach((gate) => {
            if (gate.monthsAfterPrevious && isDefaultDateString(updatedData[gate.key])) {
                currentDate = calculateNextDate(currentDate, gate.monthsAfterPrevious)
                updatedData[gate.key] = currentDate.toISOString()
            }
        })

        return updatedData
    }

    const createDateChangeEdit = (dateKey: string, newDate: Date, currentCaseData: any) => {
        const gate = DECISION_GATES.find((g) => g.key === dateKey)
        const caseDataCopy = { ...currentCaseData }

        if (!gate) { return null }

        return {
            inputLabel: dateKey,
            projectId: currentCaseData.projectId,
            resourceName: "case",
            resourcePropertyKey: dateKey as ResourcePropertyKey,
            caseId: currentCaseData.caseId,
            newDisplayValue: formatDate(newDate.toISOString()),
            previousDisplayValue: formatDate(caseDataCopy[dateKey]),
            newResourceObject: gate.monthsAfterPrevious
                ? updateCascadingDates(gate, newDate, currentCaseData)
                : { ...currentCaseData, [dateKey]: newDate.toISOString() },
            previousResourceObject: caseDataCopy,
            tabName: tab,
        }
    }

    function handleDateChange(dateKey: string, dateValue: string) {
        const dateValueYear = Number(dateValue.substring(0, 4))

        if ((dateValueYear >= 2010 && dateValueYear <= 2110) || dateValue === "") {
            const newDate = Number.isNaN(dateStringToDateUtc(dateValue).getTime())
                ? defaultDate()
                : dateStringToDateUtc(dateValue)

            const editData = createDateChangeEdit(dateKey, newDate, caseData)
            if (editData) { addEdit(editData) }
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
        if (!caseData) { return defaultDate() }
        const caseDataObject = caseData as any

        if (!isDefaultDateString(String(caseDataObject[dateKey as keyof typeof caseDataObject]))) {
            return dateStringToDateUtc(String(caseDataObject[dateKey as keyof typeof caseDataObject]))
        }
        return defaultDate()
    }

    return (
        <TabContainer container spacing={2}>
            {DECISION_GATES
                .filter((caseDateKey) => Object.keys(caseData)
                    .filter((projectCaseKey) => caseDateKey.key === projectCaseKey))
                .map((caseDate) => (
                    (caseDate.visible
                        || editMode
                        || toScheduleValue(caseData[caseDate.key as keyof typeof caseData])
                    )
                        ? (
                            <Grid size={{ xs: 12, md: 6, lg: 6 }} key={uuidv4()}>
                                <SwitchableDateInput
                                    label={caseDate.label}
                                    value={getDateValue(caseDate.key)}
                                    resourcePropertyKey={caseDate.key as ResourcePropertyKey}
                                    onChange={(e) => handleDateChange(caseDate.key as ResourcePropertyKey, e.target.value)}
                                    min={
                                        (caseDate.dependsOn && caseData)
                                            ? findMinDate(getDatesFromStrings(caseDate.dependsOn.map((minDate) => caseData[minDate as keyof typeof caseData])))
                                            : undefined
                                    }
                                    max={
                                        (caseDate.cannotBeAfter && caseData)
                                            ? findMaxDate(getDatesFromStrings(caseDate.cannotBeAfter.map((maxDate) => caseData[maxDate as keyof typeof caseData])))
                                            : undefined
                                    }
                                />
                            </Grid>
                        )
                        : null
                ))}
        </TabContainer>
    )
}

export default CaseScheduleTab
