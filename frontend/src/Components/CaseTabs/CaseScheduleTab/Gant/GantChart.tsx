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

const GantContainer = styled(Box)`
  width: 100%;
  padding-bottom: 20px;
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

const HeaderContainer = styled(Box)`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
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

const GantChart = () => {
    // Get case data and mutation function
    const { apiData } = useCaseApiData()
    const { updateMilestoneDate, isLoading } = useCaseMutation()
    const { canEdit } = useCanUserEdit()

    // Get DG4 date to center the timeline
    const dg4Date = useMemo(() => {
        if (!apiData?.case?.dg4Date) { return null }

        return dateStringToDateUtc(String(apiData.case.dg4Date))
    }, [apiData])

    // Calculate the start year for our timeline
    const timelineStartYear = useMemo(() => {
        if (dg4Date) {
            return dg4Date.getFullYear() - GANTT_CONFIG.YEARS_BEFORE_DG4
        }

        // Use default if no DG4 date
        return GANTT_CONFIG.DEFAULT_DG4_YEAR - GANTT_CONFIG.YEARS_BEFORE_DG4
    }, [dg4Date])

    // Shared data for all entry components - dynamically generated based on DG4 date
    const quarterlyPeriods = useMemo(() => generateDynamicQuarterlyPeriods(dg4Date), [dg4Date])

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

    if (!apiData?.case) {
        return null
    }

    return (
        <GantContainer>
            <HeaderContainer>
                <HeaderLeft>
                    <Typography variant="caption" color="text.secondary">
                        Timeline based on
                        {" "}
                        {dg4Date ? `actual DG4 date (${dg4Date.getFullYear()})` : `estimated DG4 year (${GANTT_CONFIG.DEFAULT_DG4_YEAR})`}
                        .
                        Showing
                        {" "}
                        {GANTT_CONFIG.YEARS_BEFORE_DG4}
                        {" "}
                        years before and
                        {" "}
                        {GANTT_CONFIG.YEARS_AFTER_DG4}
                        {" "}
                        years after.
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

            <Divider sx={{ mb: 2 }} />

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
                <Grid container spacing={0}>
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
                                />
                            </Grid>
                        )
                    })}
                </Grid>
            )}
        </GantContainer>
    )
}

export default GantChart
