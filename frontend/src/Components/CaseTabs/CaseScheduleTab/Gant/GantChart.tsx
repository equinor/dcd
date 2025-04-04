import { Icon } from "@equinor/eds-core-react"
import { add } from "@equinor/eds-icons"
import {
    Box, Typography, Button, Slider,
} from "@mui/material"
import Grid from "@mui/material/Grid2"
import {
    useMemo,
    useCallback,
    useState,
    useEffect,
    useRef,
} from "react"
import styled from "styled-components"

import { CaseMilestoneDate, CASE_MILESTONE_DATES } from "../CaseScheduleTab"

import MileStoneEntry from "./MileStoneEntry"
import RangeChangeConfirmModal, { MilestoneToRemove, MilestoneToMove } from "./RangeChangeConfirmModal"

import CaseScheduleTabSkeleton from "@/Components/LoadingSkeletons/CaseScheduleTabSkeleton"
import { useCaseApiData } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { GANTT_CHART_CONFIG } from "@/Utils/Config/constants"
import {
    dateStringToDateUtc,
    dateToQuarterIndex,
    generateDynamicQuarterlyPeriods,
    quarterIndexToStartDate,
} from "@/Utils/DateUtils"

const GantContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  width: 100%;
  padding-bottom: ${({ $isReadOnly }): string => ($isReadOnly ? "10px" : "20px")};
`

const RangeSliderContainer = styled(Box)`
  display: flex;
  flex-direction: column;
  margin-bottom: 16px;
  padding: 8px 24px;
  border: 1px solid #eaeaea;
  border-radius: 4px;
  background-color: #fafafa;
`

const HeaderContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: ${({ $isReadOnly }): string => ($isReadOnly ? "8px" : "16px")};
  padding-bottom: ${({ $isReadOnly }): string => ($isReadOnly ? "4px" : "0")};
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

const CompactGrid = styled(Grid)<{ $isReadOnly?: boolean }>`
  margin-top: ${({ $isReadOnly }): string => ($isReadOnly ? "-8px" : "0")};
  display: flex;
  flex-direction: column;
  gap: ${({ $isReadOnly }): string => ($isReadOnly ? "2px" : "0")};
`

const GantChart = (): JSX.Element => {
    const { apiData } = useCaseApiData()
    const { updateMilestoneDate, updateWholeCase, isLoading } = useCaseMutation()
    const { canEdit } = useCanUserEdit()

    const currentYear = new Date().getFullYear()

    const [rangeStartYear, setRangeStartYear] = useState(currentYear - 20)
    const [rangeEndYear, setRangeEndYear] = useState(currentYear + 40)

    const [visualSliderValues, setVisualSliderValues] = useState<[number, number]>([rangeStartYear, rangeEndYear])

    const [showConfirmModal, setShowConfirmModal] = useState(false)
    const [pendingStartYear, setPendingStartYear] = useState<number | null>(null)
    const [pendingEndYear, setPendingEndYear] = useState<number | null>(null)
    const [removeMilestones, setRemoveMilestones] = useState<MilestoneToRemove[]>([])
    const [moveMilestones, setMoveMilestones] = useState<MilestoneToMove[]>([])

    const initialCenteringDoneRef = useRef(false)

    const MAX_RANGE_YEARS = GANTT_CHART_CONFIG.MAX_VISIBLE_RANGE_YEARS

    useEffect(() => {
        const range = rangeEndYear - rangeStartYear

        if (range > MAX_RANGE_YEARS) {
            setRangeStartYear(rangeEndYear - MAX_RANGE_YEARS)
        }
    }, [rangeEndYear, rangeStartYear])

    // Adjust the range on initial load to center around DG4 date
    useEffect(() => {
        if (initialCenteringDoneRef.current || !apiData?.case?.dg4Date) {
            return
        }

        const dg4Date = dateStringToDateUtc(String(apiData.case.dg4Date))
        const dg4Year = dg4Date.getFullYear()

        // Protection against extreme years (e.g., year 9999)
        const reasonableMinYear = currentYear - 100
        const reasonableMaxYear = currentYear + 100
        const safeYear = Math.max(reasonableMinYear, Math.min(dg4Year, reasonableMaxYear))

        // Use half the MAX_RANGE_YEARS to ensure DG4 is visible
        const halfRange = Math.floor(MAX_RANGE_YEARS / 2)

        const newStartYear = Math.max(GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR, safeYear - halfRange)
        const newEndYear = Math.min(GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR, newStartYear + MAX_RANGE_YEARS)

        setRangeStartYear(newStartYear)
        setRangeEndYear(newEndYear)
        setVisualSliderValues([newStartYear, newEndYear])

        initialCenteringDoneRef.current = true
    }, [apiData, currentYear, MAX_RANGE_YEARS])

    const checkMilestonesInRange = useCallback(async (startYear: number, endYear: number): Promise<void> => {
        if (!apiData?.case) { return }

        const currentMilestoneDates: Record<string, { value: string | null; year: number | null }> = {}

        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (typeof dateValue === "string") {
                const date = dateStringToDateUtc(dateValue)

                currentMilestoneDates[milestone.key] = {
                    value: dateValue,
                    year: date.getFullYear(),
                }
            } else {
                currentMilestoneDates[milestone.key] = { value: null, year: null }
            }
        })

        const milestonesToUpdate: {
            key: string;
            label: string;
            currentDate: string | null;
            currentYear: number | null;
            newDate: string | null;
            reason: string;
            required: boolean;
        }[] = []

        let hasOutOfRangeMilestones = false

        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (dateValue) {
                const date = dateStringToDateUtc(String(dateValue))
                const year = date.getFullYear()

                if (year < startYear || year > endYear) {
                    hasOutOfRangeMilestones = true
                }
            }
        })

        if (hasOutOfRangeMilestones) {
            const middleYear = Math.floor((startYear + endYear) / 2)
            const middleQuarterIndex = (middleYear - startYear) * 4 + 1 // Q2 of the middle year
            const middleDate = quarterIndexToStartDate(middleQuarterIndex, startYear)

            CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
                const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

                if (!dateValue) { return }

                const date = dateStringToDateUtc(String(dateValue))
                const year = date.getFullYear()
                const isRequired = milestone.required === true
                const isOutOfRange = year < startYear || year > endYear

                if (isOutOfRange) {
                    if (isRequired) {
                        // Required milestone (DG4) - move to middle of range if out of range
                        if (middleDate) {
                            const newDateStr = middleDate.toISOString()

                            if (newDateStr !== dateValue) {
                                milestonesToUpdate.push({
                                    key: milestone.key,
                                    label: milestone.label,
                                    currentDate: date.toISOString(),
                                    currentYear: year,
                                    newDate: newDateStr,
                                    reason: `Required milestone (${milestone.label}) moved to middle of range (was outside: ${year})`,
                                    required: true,
                                })
                            }
                        }
                    } else {
                        // Non-required milestone - remove it if outside range and not already null
                        milestonesToUpdate.push({
                            key: milestone.key,
                            label: milestone.label,
                            currentDate: date.toISOString(),
                            currentYear: year,
                            newDate: null,
                            reason: `Optional milestone (${milestone.label}) removed from timeline (outside range ${startYear}-${endYear})`,
                            required: false,
                        })
                    }
                }
            })
        }

        if (milestonesToUpdate.length > 0) {
            const updates: Record<string, string | null> = {}

            milestonesToUpdate.forEach(({ key, newDate }) => {
                updates[key] = newDate
            })

            const Newcase = { ...apiData.case, ...updates }

            updateWholeCase(Newcase)
        }
    }, [apiData, updateMilestoneDate])

    const checkForOutOfRangeMilestones = (startYear: number, endYear: number): {
        toRemove: MilestoneToRemove[];
        toMove: MilestoneToMove[];
    } => {
        if (!apiData?.case) { return { toRemove: [], toMove: [] } }

        const toRemove: MilestoneToRemove[] = []
        const toMove: MilestoneToMove[] = []

        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (dateValue) {
                const date = dateStringToDateUtc(String(dateValue))
                const year = date.getFullYear()

                // Check if milestone is outside the new range
                if (year < startYear || year > endYear) {
                    if (milestone.required) {
                        // Required milestones will be moved to center
                        toMove.push({
                            key: milestone.key,
                            label: milestone.label,
                            fromYear: year,
                        })
                    } else {
                        // Optional milestones will be removed
                        toRemove.push({
                            key: milestone.key,
                            label: milestone.label,
                            fromYear: year,
                        })
                    }
                }
            }
        })

        return { toRemove, toMove }
    }

    const applyRangeChange = (startYear: number, endYear: number): void => {
        setRangeStartYear(startYear)
        setRangeEndYear(endYear)
        setVisualSliderValues([startYear, endYear])

        checkMilestonesInRange(startYear, endYear)
    }

    const handleConfirmRangeChange = (): void => {
        if (pendingStartYear !== null && pendingEndYear !== null) {
            applyRangeChange(pendingStartYear, pendingEndYear)
        }
        setShowConfirmModal(false)
        setPendingStartYear(null)
        setPendingEndYear(null)
        setRemoveMilestones([])
        setMoveMilestones([])
    }

    const handleCancelRangeChange = (): void => {
        setVisualSliderValues([rangeStartYear, rangeEndYear])
        setShowConfirmModal(false)
        setPendingStartYear(null)
        setPendingEndYear(null)
        setRemoveMilestones([])
        setMoveMilestones([])
    }

    const handleSliderDrag = (_event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]): void => {
        if (isLoading) { return }

        const [newStart, newEnd] = newValue as number[]

        const range = newEnd - newStart
        let visualStartYear = newStart
        let visualEndYear = newEnd

        if (range > MAX_RANGE_YEARS) {
            const currentEnd = visualSliderValues[1]

            if (newEnd !== currentEnd) {
                // User is dragging the end handle
                visualStartYear = newEnd - MAX_RANGE_YEARS
                visualEndYear = newEnd
            } else {
                // User is dragging the start handle
                visualStartYear = newStart
                visualEndYear = newStart + MAX_RANGE_YEARS
            }
        }

        setVisualSliderValues([visualStartYear, visualEndYear])
    }

    const handleSliderCommit = (_event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]): void => {
        if (isLoading) { return }

        const [newStart, newEnd] = newValue as number[]

        // Enforce the maximum 50-year range
        const range = newEnd - newStart
        let finalStartYear = newStart
        let finalEndYear = newEnd

        if (range > MAX_RANGE_YEARS) {
            // If the range would exceed MAX_RANGE_YEARS, adjust both ends to maintain the last movement direction
            if (newEnd !== rangeEndYear) {
                // User is dragging the end handle
                finalStartYear = newEnd - MAX_RANGE_YEARS
                finalEndYear = newEnd
            } else {
                // User is dragging the start handle
                finalStartYear = newStart
                finalEndYear = newStart + MAX_RANGE_YEARS
            }
        }

        const { toRemove, toMove } = checkForOutOfRangeMilestones(finalStartYear, finalEndYear)

        if (toRemove.length > 0 || toMove.length > 0) {
            setPendingStartYear(finalStartYear)
            setPendingEndYear(finalEndYear)
            setRemoveMilestones(toRemove)
            setMoveMilestones(toMove)
            setShowConfirmModal(true)

            setVisualSliderValues([finalStartYear, finalEndYear])
        } else {
            applyRangeChange(finalStartYear, finalEndYear)
        }
    }

    // Shared data for all entry components - dynamic range based on slider values
    const quarterlyPeriods = useMemo((): ReturnType<typeof generateDynamicQuarterlyPeriods> => {
        const fixedReferenceDate = new Date()

        fixedReferenceDate.setFullYear(rangeStartYear)

        const totalYears = rangeEndYear - rangeStartYear + 1

        return generateDynamicQuarterlyPeriods(fixedReferenceDate, totalYears)
    }, [rangeStartYear, rangeEndYear])

    const handleMilestoneDateChange = useCallback((dateKey: string, newQuarterIndex: number): void => {
        if (!apiData || !apiData.case) { return }

        const newDate = quarterIndexToStartDate(newQuarterIndex, rangeStartYear)

        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate, rangeStartYear])

    const handleClearMilestone = useCallback((dateKey: string): void => {
        if (!apiData || !apiData.case || !canEdit()) { return }

        const milestone = CASE_MILESTONE_DATES.find((m: CaseMilestoneDate) => m.key === dateKey)

        if (milestone?.required) {
            return
        }

        updateMilestoneDate(dateKey, null)
    }, [apiData, updateMilestoneDate, canEdit])

    const handleAddMilestone = useCallback((dateKey: string): void => {
        if (!apiData || !apiData.case) { return }

        const middleYear = Math.floor((rangeStartYear + rangeEndYear) / 2)
        const middleQuarter = 2
        const middleQuarterIndex = (middleYear - rangeStartYear) * 4 + middleQuarter - 1
        const newDate = quarterIndexToStartDate(middleQuarterIndex, rangeStartYear)

        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate, rangeStartYear, rangeEndYear])

    const milestoneDates = useMemo((): Record<string, number> => {
        if (!apiData?.case) { return {} }

        const result: Record<string, number> = {}

        // Map each milestone key to its quarter index
        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (dateValue) {
                const date = dateStringToDateUtc(String(dateValue))
                const quarterIndex = dateToQuarterIndex(date, rangeStartYear)

                if (quarterIndex !== undefined) {
                    result[milestone.key] = quarterIndex
                }
            }
        })

        return result
    }, [apiData, rangeStartYear])

    const missingMilestones = useMemo(
        (): CaseMilestoneDate[] => CASE_MILESTONE_DATES.filter((milestone: CaseMilestoneDate) => milestoneDates[milestone.key] === undefined),
        [milestoneDates],
    )

    const isReadOnly = !canEdit()

    if (!apiData?.case) {
        return (<CaseScheduleTabSkeleton />)
    }

    return (
        <GantContainer $isReadOnly={isReadOnly}>
            {canEdit() && (
                <RangeSliderContainer>
                    <Typography variant="subtitle2" gutterBottom>
                        Adjust Timeline Range (Max 50 Years)
                    </Typography>
                    <Slider
                        value={visualSliderValues}
                        onChange={handleSliderDrag}
                        onChangeCommitted={handleSliderCommit}
                        min={GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR}
                        max={GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR}
                        step={1}
                        disabled={isLoading}
                        marks={[
                            { value: GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR, label: `${GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR}` },
                            { value: currentYear, label: `${currentYear}` },
                            { value: GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR, label: `${GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR}` },
                        ]}
                        valueLabelDisplay="on"
                        getAriaValueText={(value): string => `${value}`}
                        valueLabelFormat={(value): string => `${value}`}
                    />
                </RangeSliderContainer>
            )}

            <HeaderContainer $isReadOnly={isReadOnly}>
                <HeaderLeft>
                    <Typography variant="caption" color="text.secondary">
                        {((): string => (canEdit()
                            ? `Timeline showing from ${rangeStartYear} to ${rangeEndYear}. Milestones can be adjusted by dragging.`
                            : `Timeline showing project milestones from ${rangeStartYear} to ${rangeEndYear}.`))()}
                    </Typography>
                </HeaderLeft>

                {canEdit() && missingMilestones.length > 0 && (
                    <HeaderRight>
                        {missingMilestones.map((milestone: CaseMilestoneDate): JSX.Element => (
                            <Button
                                key={milestone.key}
                                variant="outlined"
                                color="primary"
                                size="small"
                                onClick={(): void => handleAddMilestone(milestone.key)}
                                disabled={isLoading}
                                startIcon={<Icon data={add} size={16} />}
                            >
                                {milestone.label}
                            </Button>
                        ))}
                    </HeaderRight>
                )}
            </HeaderContainer>

            <CompactGrid container spacing={0} $isReadOnly={isReadOnly}>
                {CASE_MILESTONE_DATES.map((milestone: CaseMilestoneDate): JSX.Element | null => {
                    const quarterIndex = milestoneDates[milestone.key]

                    if (quarterIndex === undefined) { return null }

                    return (
                        <Grid size={{ xs: 12 }} key={milestone.key}>
                            <MileStoneEntry
                                title={milestone.label}
                                value={quarterIndex}
                                onChange={(newValue: number): void => handleMilestoneDateChange(milestone.key, newValue)}
                                onClear={(): void => handleClearMilestone(milestone.key)}
                                isRequired={milestone.required}
                                periodsData={quarterlyPeriods}
                                disabled={isLoading}
                                readOnly={isReadOnly}
                            />
                        </Grid>
                    )
                })}
            </CompactGrid>

            <RangeChangeConfirmModal
                isOpen={showConfirmModal}
                pendingStartYear={pendingStartYear}
                pendingEndYear={pendingEndYear}
                milestonesToRemove={removeMilestones}
                milestonesToMove={moveMilestones}
                onConfirm={handleConfirmRangeChange}
                onCancel={handleCancelRangeChange}
            />
        </GantContainer>
    )
}

export default GantChart
