import { Icon, Typography } from "@equinor/eds-core-react"
import {
    add, tune, close,
} from "@equinor/eds-icons"
import {
    Box, Button, Tooltip,
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

import MilestoneEntry from "./MileStoneEntry"
import RangeChangeConfirmModal, { MilestoneToRemove, MilestoneToMove } from "./RangeChangeConfirmModal"
import TimelineRangeSlider from "./TimelineRangeSlider"

import RangeSlider from "@/Components/Input/RangeSlider"
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
    formatDateToUtcIsoString
} from "@/Utils/DateUtils"

const GantContainer = styled(Box)<{ $isReadOnly?: boolean }>`
  width: 100%;
  padding-bottom: ${({ $isReadOnly }): string => ($isReadOnly ? "10px" : "20px")};
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

const ActionButtonGroup = styled(Box)`
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 12px;
`

const EntryChips = styled(Box)`
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

const ScheduleHeader = styled(Box)`
    margin-bottom: 24px;
    padding: 0 20px;
`

const ScheduleHeaderTexts = styled(Box)`
    p {
        line-height: 1.5;
    }
`

const calculateConstrainedRange = (
    newStart: number,
    newEnd: number,
    currentEnd: number,
    maxRange: number,
): [number, number] => {
    const range = newEnd - newStart

    if (range > maxRange) {
        if (newEnd !== currentEnd) {
            return [newEnd - maxRange, newEnd]
        }

        return [newStart, newStart + maxRange]
    }

    return [newStart, newEnd]
}

const GantChart = (): JSX.Element => {
    const { apiData } = useCaseApiData()
    const { updateMilestoneDate, updateWholeCase, isLoading } = useCaseMutation()
    const { canEdit } = useCanUserEdit()

    const currentYear = new Date().getFullYear()

    const [rangeStartYear, setRangeStartYear] = useState(currentYear - 5)
    const [rangeEndYear, setRangeEndYear] = useState(currentYear + 15)

    const [visualSliderValues, setVisualSliderValues] = useState<[number, number]>([rangeStartYear, rangeEndYear])

    const [showConfirmModal, setShowConfirmModal] = useState(false)
    const [pendingStartYear, setPendingStartYear] = useState<number | null>(null)
    const [pendingEndYear, setPendingEndYear] = useState<number | null>(null)
    const [removeMilestones, setRemoveMilestones] = useState<MilestoneToRemove[]>([])
    const [moveMilestones, setMoveMilestones] = useState<MilestoneToMove[]>([])

    const [showRangeSlider, setShowRangeSlider] = useState(false)

    const initialCenteringDoneRef = useRef(false)

    const MAX_RANGE_YEARS = GANTT_CHART_CONFIG.MAX_VISIBLE_RANGE_YEARS

    useEffect(() => {
        const range = rangeEndYear - rangeStartYear

        if (range > MAX_RANGE_YEARS) {
            setRangeStartYear(rangeEndYear - MAX_RANGE_YEARS)
        }
    }, [rangeEndYear, rangeStartYear])

    useEffect(() => {
        if (initialCenteringDoneRef.current || !apiData?.case?.dg4Date) {
            return
        }

        const dg4Date = dateStringToDateUtc(String(apiData.case.dg4Date))
        const dg4Year = dg4Date.getFullYear()

        const reasonableMinYear = currentYear - 100
        const reasonableMaxYear = currentYear + 100
        const safeYear = Math.max(reasonableMinYear, Math.min(dg4Year, reasonableMaxYear))

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
                            const newDateStr = formatDateToUtcIsoString(middleDate)

                            if (newDateStr !== dateValue) {
                                milestonesToUpdate.push({
                                    key: milestone.key,
                                    label: milestone.label,
                                    currentDate: formatDateToUtcIsoString(date),
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
                            currentDate: formatDateToUtcIsoString(date),
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
    }, [apiData, updateWholeCase])

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

    const handleSliderDrag = useCallback((_event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]): void => {
        if (isLoading) { return }
        const [newStart, newEnd] = newValue as number[]
        const currentEndValue = visualSliderValues[1]

        const [constrainedStart, constrainedEnd] = calculateConstrainedRange(
            newStart,
            newEnd,
            currentEndValue,
            MAX_RANGE_YEARS,
        )

        setVisualSliderValues([constrainedStart, constrainedEnd])
    }, [isLoading, MAX_RANGE_YEARS, visualSliderValues])

    const handleSliderCommit = useCallback((_event: Event | React.SyntheticEvent<Element, Event>, newValue: number | number[]): void => {
        if (isLoading) { return }
        const [newStart, newEnd] = newValue as number[]
        const currentEndValue = rangeEndYear // Use actual rangeEndYear for commit logic

        const [finalStartYear, finalEndYear] = calculateConstrainedRange(
            newStart,
            newEnd,
            currentEndValue,
            MAX_RANGE_YEARS,
        )

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
    }, [
        isLoading,
        MAX_RANGE_YEARS,
        rangeEndYear, // Add rangeEndYear dependency
        checkForOutOfRangeMilestones,
        applyRangeChange,
    ])

    const quarterlyPeriods = useMemo((): ReturnType<typeof generateDynamicQuarterlyPeriods> => {
        const fixedReferenceDate = new Date()

        fixedReferenceDate.setFullYear(rangeStartYear)

        const totalYears = rangeEndYear - rangeStartYear + 1

        return generateDynamicQuarterlyPeriods(fixedReferenceDate, totalYears)
    }, [rangeStartYear, rangeEndYear])

    const handleMilestoneSliderChange = useCallback((dateKey: string, newQuarterIndex: number): void => {
        if (!apiData || !apiData.case) { return }

        const newDate = quarterIndexToStartDate(newQuarterIndex, rangeStartYear)

        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate, rangeStartYear])

    const handleMilestoneDateChange = useCallback((dateKey: string, newDate: Date | null): void => {
        if (!apiData || !apiData.case) { return }

        updateMilestoneDate(dateKey, newDate)
    }, [apiData, updateMilestoneDate])

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

    const { existingMilestonesMap, missingMilestones } = useMemo(() => {
        if (!apiData?.case) {
            return { existingMilestonesMap: new Map(), missingMilestones: [] }
        }

        const existingMap = new Map<string, {
            key: string;
            label: string;
            required?: boolean;
            quarterIndex: number;
            dateObject: Date;
        }>()

        const foundKeys = new Set<string>()

        CASE_MILESTONE_DATES.forEach((milestone: CaseMilestoneDate) => {
            const dateValue = apiData.case[milestone.key as keyof typeof apiData.case]

            if (dateValue) {
                try {
                    const date = dateStringToDateUtc(String(dateValue))
                    const quarterIndex = dateToQuarterIndex(date, rangeStartYear)

                    if (quarterIndex !== undefined) {
                        existingMap.set(milestone.key, {
                            ...milestone,
                            quarterIndex,
                            dateObject: date,
                        })
                        foundKeys.add(milestone.key)
                    }
                } catch (error) {
                    console.error(`Error processing date for milestone ${milestone.key}:`, dateValue, error)
                }
            }
        })

        const missing = CASE_MILESTONE_DATES.filter((milestone: CaseMilestoneDate) => !foundKeys.has(milestone.key))

        return { existingMilestonesMap: existingMap, missingMilestones: missing }
    }, [apiData, rangeStartYear])

    const isReadOnly = !canEdit()

    if (!apiData?.case) {
        return (<CaseScheduleTabSkeleton />)
    }

    return (
        <GantContainer $isReadOnly={isReadOnly}>
            {showRangeSlider && canEdit() && (
                <TimelineRangeSlider
                    visualSliderValues={visualSliderValues}
                    minAllowedYear={GANTT_CHART_CONFIG.MIN_ALLOWED_YEAR}
                    maxAllowedYear={GANTT_CHART_CONFIG.MAX_ALLOWED_YEAR}
                    currentYear={currentYear}
                    isLoading={isLoading}
                    maxRangeYears={MAX_RANGE_YEARS}
                    onSliderDrag={handleSliderDrag}
                    onSliderCommit={handleSliderCommit}
                />
            )}

            <ScheduleHeader>
                <ScheduleHeaderTexts>
                    <Typography variant="h2">Milestone Schedule</Typography>
                    <Typography variant="ingress">
                        Visualize and adjust key project milestones. Use the date pickers for specific dates or the sliders for quarterly adjustments.
                    </Typography>
                </ScheduleHeaderTexts>
            </ScheduleHeader>

            <HeaderContainer $isReadOnly={isReadOnly}>
                {canEdit() && missingMilestones.length > 0 && (
                    <EntryChips>
                        {missingMilestones.map((milestone: CaseMilestoneDate): JSX.Element => (
                            <Tooltip key={milestone.key} title={`Add ${milestone.label} milestone to the timeline`}>
                                <Button
                                    variant="outlined"
                                    color="primary"
                                    size="small"
                                    onClick={(): void => handleAddMilestone(milestone.key)}
                                    disabled={isLoading}
                                    startIcon={<Icon data={add} size={16} />}
                                >
                                    {milestone.label}
                                </Button>
                            </Tooltip>
                        ))}
                    </EntryChips>
                )}

                {canEdit() && (
                    <ActionButtonGroup>
                        <Tooltip title={showRangeSlider ? "Close timeline adjuster" : "Adjust the visible timeline range"}>
                            <Button
                                variant="contained"
                                color="primary"
                                size="small"
                                onClick={() => setShowRangeSlider(!showRangeSlider)}
                                startIcon={<Icon data={showRangeSlider ? close : tune} size={16} />}
                            >
                                Adjust Timeline
                            </Button>
                        </Tooltip>
                    </ActionButtonGroup>
                )}
            </HeaderContainer>

            <CompactGrid container spacing={0} $isReadOnly={isReadOnly}>
                {Array.from(existingMilestonesMap.values()).map((milestoneData) => (
                    <Grid size={{ xs: 12 }} key={milestoneData.key}>
                        <MilestoneEntry
                            title={milestoneData.label}
                            value={milestoneData.quarterIndex}
                            dateValue={milestoneData.dateObject}
                            rangeStartYear={rangeStartYear}
                            rangeEndYear={rangeEndYear}
                            onSliderChange={(newValue: number): void => handleMilestoneSliderChange(milestoneData.key, newValue)}
                            onDateChange={(newDate: Date | null): void => handleMilestoneDateChange(milestoneData.key, newDate)}
                            onClear={(): void => handleClearMilestone(milestoneData.key)}
                            isRequired={milestoneData.required}
                            periodsData={quarterlyPeriods}
                            disabled={isLoading}
                            readOnly={isReadOnly}
                        />
                    </Grid>
                ))}
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
