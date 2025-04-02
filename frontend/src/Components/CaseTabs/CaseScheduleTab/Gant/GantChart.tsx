import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import {
    Box, Typography, Button, Alert, Divider,
} from "@mui/material"
import Grid from "@mui/material/Grid2"
import { useMemo, useCallback } from "react"
import styled from "styled-components"

import { CaseMilestoneDate, CASE_MILESTONE_DATES } from "../CaseScheduleTab"

import MileStoneEntry from "./MileStoneEntry"
// Import from CaseScheduleTab

import { useCaseApiData } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import {
    GANTT_CONFIG,
    dateStringToDateUtc,
    dateToQuarterIndex,
    generateDynamicQuarterlyPeriods,
    getCurrentQuarterIndex,
    quarterIndexToStartDate,
} from "@/Utils/DateUtils"

const GantContainer = styled(Box)<{ isReadOnly?: boolean }>`
  width: 100%;
  padding-bottom: ${({ isReadOnly }) => (isReadOnly ? "10px" : "20px")};
`

const SectionTitle = styled(Typography)`
  margin-bottom: 24px;
  font-weight: 500;
  padding-left: 4px;
`

const EmptyStateContainer = styled(Box)`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 40px 0;
  gap: 16px;
`

const HeaderContainer = styled(Box)<{ isReadOnly?: boolean }>`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: ${({ isReadOnly }) => (isReadOnly ? "8px" : "16px")};
  padding-bottom: ${({ isReadOnly }) => (isReadOnly ? "4px" : "0")};
  flex-wrap: wrap;
  gap: 12px;
`

const HeaderLeft = styled(Box)`
  display: flex;
  align-items: center;
`

const HeaderRight = styled(Box)`
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 8px;
`

const CompactGrid = styled(Grid)<{ isReadOnly?: boolean }>`
  margin-top: ${({ isReadOnly }) => (isReadOnly ? "-8px" : "0")};
  display: flex;
  flex-direction: column;
  gap: ${({ isReadOnly }) => (isReadOnly ? "2px" : "0")};
`

const GantChart = () => {
    // Get case data and mutation function
    const { apiData } = useCaseApiData()
    const { updateMilestoneDate, isLoading } = useCaseMutation()
    const { canEdit } = useCanUserEdit()

    // Get DG4 date for display purposes only
    const dg4Date = useMemo(() => {
        if (!apiData?.case?.dg4Date) { return null }

        return dateStringToDateUtc(String(apiData.case.dg4Date))
    }, [apiData])

    // Calculate the start year for our timeline - use fixed current year rather than DG4
    const currentYear = new Date().getFullYear()
    // Use current year as a fixed reference point
    const timelineStartYear = currentYear - 20

    // Shared data for all entry components - fixed range based on current year
    const quarterlyPeriods = useMemo(() => {
        // Create a Date with current year as reference
        const fixedReferenceDate = new Date()

        return generateDynamicQuarterlyPeriods(fixedReferenceDate)
    }, [])

    // Function to handle milestone date changes from the Gantt chart
    const handleMilestoneDateChange = useCallback((dateKey: string, newQuarterIndex: number) => {
        if (!apiData || !apiData.case) { return }

        // Convert the quarter index to a date (first day of quarter)
        const newDate = quarterIndexToStartDate(newQuarterIndex, timelineStartYear)

        // Update the milestone date through the API
        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate, timelineStartYear])

    // Function to clear a milestone date
    const handleClearMilestone = useCallback((dateKey: string) => {
        if (!apiData || !apiData.case || !canEdit()) { return }

        // Check if this is a required milestone (e.g., DG4)
        const milestone = CASE_MILESTONE_DATES.find((m: CaseMilestoneDate) => m.key === dateKey)

        if (milestone?.required) {
            return // Don't allow clearing required milestones
        }

        // Set the milestone date to null to clear it
        updateMilestoneDate(dateKey, null)
    }, [apiData, updateMilestoneDate, canEdit])

    // Function to add a new milestone date
    const handleAddMilestone = useCallback((dateKey: string) => {
        if (!apiData || !apiData.case) { return }

        // Get the current quarter index based on today's date and set milestone date to it
        const quarterIndex = getCurrentQuarterIndex(timelineStartYear)
        const newDate = quarterIndexToStartDate(quarterIndex, timelineStartYear)

        // Update the milestone date through the API
        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate, timelineStartYear])

    // Get quarter index for each milestone from the API data
    const milestoneDates = useMemo(() => {
        if (!apiData?.case) { return {} }

        const result: Record<string, number> = {}

        // Map each milestone key to its quarter index
        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (dateValue) {
                const date = dateStringToDateUtc(String(dateValue))
                const quarterIndex = dateToQuarterIndex(date, timelineStartYear)

                if (quarterIndex !== undefined) {
                    result[milestone.key] = quarterIndex
                }
            }
        })

        return result
    }, [apiData, timelineStartYear])

    // Check if there are any milestones to display
    const hasMilestones = useMemo(() => Object.keys(milestoneDates).length > 0, [milestoneDates])

    // Get milestones that don't have dates set
    const missingMilestones = useMemo(
        () => CASE_MILESTONE_DATES.filter((milestone: CaseMilestoneDate) => milestoneDates[milestone.key] === undefined),
        [milestoneDates],
    )

    const isReadOnly = !canEdit()

    if (!apiData?.case) {
        return null
    }

    return (
        <GantContainer isReadOnly={isReadOnly}>
            <HeaderContainer isReadOnly={isReadOnly}>
                <HeaderLeft>
                    <Typography variant="caption" color="text.secondary">
                        {canEdit()
                            ? `Timeline showing from ${currentYear - 20} to ${currentYear + 40}. Milestones can be adjusted by dragging or using the dropdown.`
                            : `Timeline showing project milestones from ${currentYear - 20} to ${currentYear + 40}.`}
                    </Typography>
                </HeaderLeft>

                {canEdit() && missingMilestones.length > 0 && (
                    <HeaderRight>
                        {missingMilestones.map((milestone: CaseMilestoneDate) => (
                            <Button
                                key={milestone.key}
                                variant="outlined"
                                color="primary"
                                size="small"
                                onClick={() => handleAddMilestone(milestone.key)}
                                disabled={isLoading}
                                startIcon={<Icon data={add} size={16} />}
                            >
                                {milestone.label}
                            </Button>
                        ))}
                    </HeaderRight>
                )}
            </HeaderContainer>

            <Divider sx={{ mb: isReadOnly ? 1 : 2 }} />

            {!hasMilestones ? (
                <EmptyStateContainer>
                    <Alert
                        severity="info"
                        sx={{
                            width: "100%",
                            maxWidth: "600px",
                            "& .MuiAlert-message": {
                                width: "100%",
                            },
                        }}
                    >
                        No milestone dates have been set yet for the Gantt chart.
                        {canEdit() ? " Use the 'Add' buttons at the top to add milestones to the timeline." : ""}
                    </Alert>
                </EmptyStateContainer>
            ) : (
                <CompactGrid container spacing={0} isReadOnly={isReadOnly}>
                    {CASE_MILESTONE_DATES.map((milestone: CaseMilestoneDate) => {
                        // If the milestone exists in API data, display it
                        const quarterIndex = milestoneDates[milestone.key]

                        // Skip milestones that don't have dates set
                        if (quarterIndex === undefined) { return null }

                        return (
                            <Grid size={{ xs: 12 }} key={milestone.key}>
                                <MileStoneEntry
                                    title={milestone.label}
                                    description={milestone.description}
                                    value={quarterIndex}
                                    onChange={(newValue: number) => handleMilestoneDateChange(milestone.key, newValue)}
                                    onClear={canEdit() && !milestone.required ? () => handleClearMilestone(milestone.key) : undefined}
                                    periodsData={quarterlyPeriods}
                                    disabled={isLoading}
                                    readOnly={isReadOnly}
                                />
                            </Grid>
                        )
                    })}
                </CompactGrid>
            )}
        </GantContainer>
    )
}

export default GantChart
