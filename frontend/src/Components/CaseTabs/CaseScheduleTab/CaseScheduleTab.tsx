import { Divider, Typography } from "@mui/material"
import Grid from "@mui/material/Grid2"
import { useCallback, useMemo } from "react"
import styled from "styled-components"

import GantChart from "./Gant/GantChart"

import SwitchableDateInput from "@/Components/Input/SwitchableDateInput"
import CaseScheduleTabSkeleton from "@/Components/LoadingSkeletons/CaseScheduleTabSkeleton"
import { useCaseApiData } from "@/Hooks"
import { useCaseMutation } from "@/Hooks/Mutations"
import useCanUserEdit from "@/Hooks/useCanUserEdit"
import { useAppStore } from "@/Store/AppStore"
import { useProjectContext } from "@/Store/ProjectContext"
import {
    dateStringToDateUtc,
} from "@/Utils/DateUtils"

const TabContainer = styled(Grid)`
    width: 100%;
`

const MilestonesContainer = styled(Grid)`
    max-width: 800px;
    margin: 0 auto;
`

const DividerSection = styled(Divider)`
    margin: 40px 0 20px;
`

const SectionTitle = styled(Typography)`
    margin-bottom: 16px;
    font-weight: 500;
`

export interface CaseMilestoneDate {
    label: string
    key: string
    description: string
    visible?: boolean
    required?: boolean
}

export const CASE_MILESTONE_DATES: CaseMilestoneDate[] = [
    { label: "DGA", key: "dgaDate", description: "Decision Gate A" },
    { label: "DGB", key: "dgbDate", description: "Decision Gate B" },
    { label: "DGC", key: "dgcDate", description: "Decision Gate C" },
    { label: "APbo", key: "apboDate", description: "Approval of Plan for Building and Operating" },
    { label: "BOR", key: "borDate", description: "Basis of Risk Calculation" },
    { label: "VPbo", key: "vpboDate", description: "Verification Plan for Building and Operating" },
    { label: "DG0", key: "dg0Date", description: "Decision Gate 0" },
    { label: "DG1", key: "dg1Date", description: "Decision Gate 1" },
    { label: "DG2", key: "dg2Date", description: "Decision Gate 2" },
    { label: "DG3", key: "dg3Date", description: "Decision Gate 3" },
    {
        label: "DG4", key: "dg4Date", description: "Decision Gate 4", required: true,
    },
]

const CaseScheduleTab = () => {
    const { projectId } = useProjectContext()
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

    if (!apiData || !projectId) {
        return (<CaseScheduleTabSkeleton />)
    }

    return (
        <TabContainer container spacing={2}>
            <Grid size={{ xs: 12 }}>
                <SectionTitle variant="h5">Milestone Dates</SectionTitle>
                <MilestonesContainer container spacing={2}>
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
            </Grid>

            <Grid size={{ xs: 12 }}>
                <DividerSection />
                <SectionTitle variant="h5">Visual Timeline</SectionTitle>
            </Grid>

            <Grid size={{ xs: 12 }}>
                <GantChart />
            </Grid>
        </TabContainer>
    )
}

export default CaseScheduleTab
