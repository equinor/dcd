import { Typography } from "@mui/material"
import Grid from "@mui/material/Grid2"
import { useCallback, useMemo } from "react"
import styled from "styled-components"

import { CASE_MILESTONE_DATES } from "./CaseScheduleTab"

import SwitchableDateInput from "@/Components/Input/SwitchableDateInput"
import { useCaseApiData } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { dateStringToDateUtc } from "@/Utils/DateUtils"

const MilestonesContainer = styled(Grid)`
    max-width: 800px;
    margin: 0 auto;
`

const SectionTitle = styled(Typography)`
    margin-bottom: 16px;
    font-weight: 500;
`

const OldSchedulePickers = () => {
    const { apiData } = useCaseApiData()
    const { canEdit } = useCanUserEdit()
    const { setSnackBarMessage } = useAppStore()
    const { updateMilestoneDate } = useCaseMutation()

    const handleDateChange = useCallback((dateKey: string, dateValue: string) => {
        if (!apiData || !apiData.case) { return }

        const milestone = CASE_MILESTONE_DATES.find((m) => m.key === dateKey)

        if (milestone?.required && dateValue === "") {
            setSnackBarMessage(`${milestone.label} is required and cannot be empty.`)

            return
        }

        let newDate: Date | null = null

        if (dateValue !== "") {
            const parsedDate = dateStringToDateUtc(dateValue)

            if (!Number.isNaN(parsedDate.getTime())) {
                newDate = parsedDate
            }
        }

        updateMilestoneDate(dateKey, newDate)
    }, [apiData, setSnackBarMessage, updateMilestoneDate])

    const getChangeHandler = useCallback(
        (dateKey: any) => (e: React.ChangeEvent<HTMLInputElement>) => handleDateChange(dateKey, e.target.value),
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

    const visibleMilestoneDates = useMemo(() => {
        if (!apiData || !apiData.case) { return [] }

        return CASE_MILESTONE_DATES
            .filter((m) => Object.keys(apiData.case).includes(m.key))
            .filter((m) => m.visible
                || canEdit()
                || toScheduleValue(apiData.case[m.key as keyof typeof apiData.case]))
    }, [apiData, canEdit, toScheduleValue])

    if (!apiData) {
        return null
    }

    return (
        <MilestonesContainer container spacing={2}>
            <Grid size={{ xs: 12 }}>
                <SectionTitle variant="h6">Date Selection</SectionTitle>
            </Grid>

            {visibleMilestoneDates.map((milestone) => (
                <Grid size={{ xs: 12, md: 6, lg: 6 }} key={milestone.key}>
                    <SwitchableDateInput
                        required={milestone.required}
                        label={milestone.label}
                        value={getDateValue(milestone.key)}
                        onChange={getChangeHandler(milestone.key)}
                    />
                </Grid>
            ))}
        </MilestonesContainer>
    )
}

export default OldSchedulePickers
