import Grid from "@mui/material/Grid2"
import styled from "styled-components"

import GantChart from "./Gant/GantChart"

import CaseScheduleTabSkeleton from "@/Components/LoadingSkeletons/CaseScheduleTabSkeleton"
import { useCaseApiData } from "@/Hooks"
import { useProjectContext } from "@/Store/ProjectContext"

const TabContainer = styled(Grid)`
    width: 100%;
`

export interface CaseMilestoneDate {
    label: string
    key: string
    visible?: boolean
    required?: boolean
}

export const CASE_MILESTONE_DATES: CaseMilestoneDate[] = [
    { label: "DGA", key: "dgaDate" },
    { label: "DGB", key: "dgbDate" },
    { label: "DGC", key: "dgcDate" },
    { label: "APbo", key: "apboDate" },
    { label: "BOR", key: "borDate" },
    { label: "VPbo", key: "vpboDate" },
    { label: "DG0", key: "dg0Date" },
    { label: "DG1", key: "dg1Date" },
    { label: "DG2", key: "dg2Date" },
    { label: "DG3", key: "dg3Date" },
    {
        label: "DG4", key: "dg4Date", required: true,
    },
]

const CaseScheduleTab = (): JSX.Element => {
    const { projectId } = useProjectContext()
    const { apiData } = useCaseApiData()

    if (!apiData || !projectId) {
        return (<CaseScheduleTabSkeleton />)
    }

    return (
        <TabContainer container spacing={2}>
            <Grid size={{ xs: 12 }}>
                {/* Old date picker UI - Replaced with Gantt chart
                <OldSchedulePickers /> */}
            </Grid>
            <Grid size={{ xs: 12 }}>
                <GantChart />
            </Grid>
        </TabContainer>
    )
}

export default CaseScheduleTab
