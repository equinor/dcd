import Grid from "@mui/material/Grid2"
import { v4 as uuidv4 } from "uuid"
import styled from "styled-components"
import { useCallback, useMemo } from "react"

import {
    dateStringToDateUtc,
} from "@/Utils/DateUtils"
import CaseScheduleTabSkeleton from "@/Components/LoadingSkeletons/CaseScheduleTabSkeleton"
import { ResourceName, ResourcePropertyKey } from "@/Models/Interfaces"
import SwitchableDateInput from "@/Components/Input/SwitchableDateInput"
import { useProjectContext } from "@/Store/ProjectContext"
import { useCaseApiData } from "@/Hooks"
import useEditCase from "@/Hooks/useEditCase"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"

const TabContainer = styled(Grid)`
    max-width: 800px;
`

interface CaseMilestoneDate {
    label: string
    key: string
    visible?: boolean
    required?: boolean
}

const CASE_MILESTONE_DATES: CaseMilestoneDate[] = [
    { label: "DGA", key: "dgaDate" },
    { label: "DGB", key: "dgbDate" },
    { label: "DGC", key: "dgcDate" },
    { label: "APbo", key: "apboDate" },
    { label: "BOR", key: "borDate" },
    { label: "VPbo", key: "vpboDate" },
    { label: "DG0", key: "dG0Date", visible: true },
    { label: "DG1", key: "dG1Date", visible: true },
    { label: "DG2", key: "dG2Date", visible: true },
    { label: "DG3", key: "dG3Date", visible: true },
    {
        label: "DG4", key: "dG4Date", visible: true, required: true,
    },
]

const CaseScheduleTab = () => {
    const { projectId } = useProjectContext()
    const { addEdit } = useEditCase()
    const { apiData } = useCaseApiData()
    const { canEdit } = useCanUserEdit()
    const { setSnackBarMessage } = useAppStore()

    const createDateChangeEdit = useCallback((dateKey: string, newDate: Date | undefined, currentCaseData: any) => {
        const milestone = CASE_MILESTONE_DATES.find((m) => m.key === dateKey)

        if (!milestone) { return null }

        const resourceObject = {
            ...currentCaseData,
            [dateKey]: newDate?.toISOString() ?? null,
        }

        return {
            uuid: uuidv4(),
            projectId: currentCaseData.projectId,
            resourceName: "case" as ResourceName,
            resourcePropertyKey: dateKey as ResourcePropertyKey,
            caseId: currentCaseData.caseId,
            resourceObject,
        }
    }, [])

    const handleDateChange = useCallback((dateKey: string, dateValue: string) => {
        if (!apiData || !apiData.case) { return }

        const milestone = CASE_MILESTONE_DATES.find((m) => m.key === dateKey)
        if (milestone?.required && dateValue === "") {
            setSnackBarMessage(`${milestone.label} is required and cannot be empty.`)
            return
        }

        let newDate: Date | undefined

        if (dateValue !== "") {
            const parsedDate = dateStringToDateUtc(dateValue)
            if (!Number.isNaN(parsedDate.getTime())) {
                newDate = parsedDate
            }
        }

        const editData = createDateChangeEdit(dateKey, newDate, apiData.case)
        if (editData) { addEdit(editData) }
    }, [addEdit, apiData, createDateChangeEdit, setSnackBarMessage])

    const getChangeHandler = useCallback(
        (dateKey: ResourcePropertyKey) => (e: React.ChangeEvent<HTMLInputElement>) => handleDateChange(dateKey, e.target.value),
        [handleDateChange],
    )

    const getDateValue = useCallback((dateKey: string): Date | undefined => {
        if (!apiData || !apiData.case) { return undefined }
        const caseDataObject = apiData.case as any

        if (caseDataObject[dateKey as keyof typeof caseDataObject]) {
            return dateStringToDateUtc(String(caseDataObject[dateKey as keyof typeof caseDataObject]))
        }

        return undefined
    }, [apiData])

    const toScheduleValue = useCallback((date: string | number | boolean | null | undefined) => {
        if (!date) { return undefined }
        const dateString = dateStringToDateUtc(String(date))
        return dateString ? true : undefined
    }, [])

    // Memoize the filtered milestone dates to prevent recalculation on every render
    const visibleMilestoneDates = useMemo(() => {
        if (!apiData || !apiData.case) { return [] }

        return CASE_MILESTONE_DATES
            .filter((m) => Object.keys(apiData.case).includes(m.key))
            .filter((m) => m.visible
                || canEdit()
                || toScheduleValue(apiData.case[m.key as keyof typeof apiData.case]))
    }, [apiData, canEdit, toScheduleValue])

    if (!apiData || !projectId) {
        return (<CaseScheduleTabSkeleton />)
    }

    return (
        <TabContainer container spacing={2}>
            {visibleMilestoneDates.map((milestone) => (
                <Grid size={{ xs: 12, md: 6, lg: 6 }} key={milestone.key}>
                    <SwitchableDateInput
                        required={milestone.required}
                        label={milestone.label}
                        value={getDateValue(milestone.key)}
                        resourcePropertyKey={milestone.key as ResourcePropertyKey}
                        onChange={getChangeHandler(milestone.key as ResourcePropertyKey)}
                    />
                </Grid>
            ))}
        </TabContainer>
    )
}

export default CaseScheduleTab
