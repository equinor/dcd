import { Box, Typography } from "@mui/material"
import Grid from "@mui/material/Grid2"
import { useState, useMemo } from "react"
import styled from "styled-components"

import MileStoneEntry from "./MileStoneEntry"
import RangeEntry from "./RangeEntry"

const GantContainer = styled(Box)`
  width: 100%;
  padding-bottom: 20px;
`

const SectionTitle = styled(Typography)`
  margin-bottom: 24px;
  font-weight: 500;
  padding-left: 4px;
`

// Generate quarterly periods from 2021 to 2025
export const generateQuarterlyPeriods = () => {
    const quarters = []
    let index = 0

    for (let year = 2021; year <= 2035; year += 1) {
        for (let quarter = 1; quarter <= 4; quarter += 1) {
            quarters.push({
                value: index,
                // Only show label for Q1 of each year to avoid overcrowding
                label: quarter === 1 ? `${year}` : "",
                quarter,
                year,
            })
            index += 1
        }
    }

    return quarters
}

const GantChart = () => {
    // Shared data for all entry components
    const quarterlyPeriods = useMemo(() => generateQuarterlyPeriods(), [])

    // State for range entries
    const [projectRange, setProjectRange] = useState<number[]>([0, 3]) // Project timeline
    const [implementationRange, setImplementationRange] = useState<number[]>([4, 7]) // Implementation phase

    // State for event entries
    const [milestoneDate, setMilestoneDate] = useState<number>(8) // Key milestone
    const [launchDate, setLaunchDate] = useState<number>(10) // Product launch

    return (
        <GantContainer>
            <SectionTitle variant="h5">Project Schedule</SectionTitle>

            <Grid container spacing={0}>
                <Grid size={{ xs: 12 }}>
                    <RangeEntry
                        title="Project Timeline"
                        description="Overall project duration"
                        value={projectRange}
                        onChange={(newValue: number[]) => setProjectRange(newValue)}
                        periodsData={quarterlyPeriods}
                    />
                </Grid>

                <Grid size={{ xs: 12 }}>
                    <RangeEntry
                        title="Implementation Phase"
                        description="Core implementation timeline"
                        value={implementationRange}
                        onChange={(newValue: number[]) => setImplementationRange(newValue)}
                        periodsData={quarterlyPeriods}
                    />
                </Grid>

                <Grid size={{ xs: 12 }}>
                    <MileStoneEntry
                        title="Major Milestone"
                        description="Key project milestone date"
                        value={milestoneDate}
                        onChange={(newValue: number) => setMilestoneDate(newValue)}
                        periodsData={quarterlyPeriods}
                    />
                </Grid>

                <Grid size={{ xs: 12 }}>
                    <MileStoneEntry
                        title="Product Launch"
                        description="Official product launch date"
                        value={launchDate}
                        onChange={(newValue: number) => setLaunchDate(newValue)}
                        periodsData={quarterlyPeriods}
                    />
                </Grid>
            </Grid>
        </GantContainer>
    )
}

export default GantChart
