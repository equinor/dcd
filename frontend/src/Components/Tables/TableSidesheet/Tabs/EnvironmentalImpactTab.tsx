import { Stack, Typography, LinearProgress } from "@mui/material"
import styled from "styled-components"
import {
    grey, green, orange, red,
} from "@mui/material/colors"

const Container = styled.div`
    padding:20px;
`

const Section = styled.div`
    margin-bottom: 32px;

    &:last-child {
        margin-bottom: 0;
    }
`

const SectionTitle = styled(Typography)`
    font-size: 14px;
    font-weight: 500;
    margin-bottom: 16px;
`

const MetricCard = styled.div`
    background: ${grey[50]};
    padding: 16px;
    border-radius: 4px;
    margin-bottom: 16px;
`

const MetricTitle = styled(Typography)`
    font-size: 14px;
    font-weight: 500;
    margin-bottom: 8px;
`

const MetricValue = styled(Typography)`
    font-family: monospace;
    font-size: 24px;
    font-weight: 500;
    margin-bottom: 8px;
`

const MetricDescription = styled(Typography)`
    color: ${grey[600]};
    font-size: 12px;
    margin-bottom: 12px;
`

const ProgressContainer = styled.div`
    margin-top: 8px;
`

const ProgressLabel = styled(Typography)`
    font-size: 12px;
    color: ${grey[600]};
    margin-bottom: 4px;
    display: flex;
    justify-content: space-between;
`

interface Props {
    rowData: any
}

const EnvironmentalImpactTab = ({ rowData }: Props) => {
    const profile = rowData.overrideProfile?.override ? rowData.overrideProfile : rowData.profile
    if (!profile) { return null }

    const calculateCarbonIntensity = () => {
        const totalEmissions = profile.values.reduce((sum: number, val: number) => sum + val, 0)
        // Assuming emissions are in tons and production in barrels
        return (totalEmissions / profile.values.length).toFixed(2)
    }

    const getEmissionsTrend = () => {
        const firstHalf = profile.values.slice(0, Math.floor(profile.values.length / 2))
        const secondHalf = profile.values.slice(Math.floor(profile.values.length / 2))
        const firstHalfAvg = firstHalf.reduce((sum: number, val: number) => sum + val, 0) / firstHalf.length
        const secondHalfAvg = secondHalf.reduce((sum: number, val: number) => sum + val, 0) / secondHalf.length
        const change = ((secondHalfAvg - firstHalfAvg) / firstHalfAvg) * 100
        return change.toFixed(1)
    }

    const getEnvironmentalScore = () => {
        const intensity = parseFloat(calculateCarbonIntensity())
        const trend = parseFloat(getEmissionsTrend())

        // Score based on intensity and trend
        let score = 100

        // Penalize for high intensity
        if (intensity > 50) { score -= 30 } else if (intensity > 30) { score -= 20 } else if (intensity > 10) { score -= 10 }

        // Penalize for increasing trend
        if (trend > 10) { score -= 30 } else if (trend > 5) { score -= 20 } else if (trend > 0) { score -= 10 }

        // Bonus for decreasing trend
        if (trend < -10) { score += 20 } else if (trend < -5) { score += 10 }

        return Math.max(0, Math.min(100, score))
    }

    const getScoreColor = (score: number) => {
        if (score >= 70) { return green[500] }
        if (score >= 40) { return orange[500] }
        return red[500]
    }

    const environmentalScore = getEnvironmentalScore()

    return (
        <Container>
            <Section>
                <SectionTitle>
                    Environmental Score
                </SectionTitle>
                <MetricCard>
                    <MetricTitle>
                        Overall Environmental Performance
                    </MetricTitle>
                    <MetricValue style={{ color: getScoreColor(environmentalScore) }}>
                        {environmentalScore}
                        /100
                    </MetricValue>
                    <ProgressContainer>
                        <LinearProgress
                            variant="determinate"
                            value={environmentalScore}
                            sx={{
                                height: 8,
                                borderRadius: 4,
                                backgroundColor: grey[200],
                                "& .MuiLinearProgress-bar": {
                                    backgroundColor: getScoreColor(environmentalScore),
                                    borderRadius: 4,
                                },
                            }}
                        />
                    </ProgressContainer>
                </MetricCard>
            </Section>

            <Section>
                <SectionTitle>
                    Key Metrics
                </SectionTitle>
                <MetricCard>
                    <MetricTitle>
                        Carbon Intensity
                    </MetricTitle>
                    <MetricValue>
                        {calculateCarbonIntensity()}
                        {" "}
                        tCO2e/unit
                    </MetricValue>
                    <MetricDescription>
                        Average carbon emissions per unit of production
                    </MetricDescription>
                </MetricCard>

                <MetricCard>
                    <MetricTitle>
                        Emissions Trend
                    </MetricTitle>
                    <MetricValue style={{
                        color: parseFloat(getEmissionsTrend()) > 0 ? red[500] : green[500],
                    }}
                    >
                        {getEmissionsTrend()}
                        %
                    </MetricValue>
                    <MetricDescription>
                        Change in emissions from first half to second half of the time series
                    </MetricDescription>
                </MetricCard>
            </Section>

            <Section>
                <SectionTitle>
                    Sustainability Indicators
                </SectionTitle>
                <Stack spacing={2}>
                    {[
                        {
                            label: "Emissions Efficiency",
                            value: Math.min(100, 100 - (parseFloat(calculateCarbonIntensity()) / 0.5)),
                        },
                        {
                            label: "Trend Performance",
                            value: Math.max(0, 100 - (parseFloat(getEmissionsTrend()) * 2)),
                        },
                        {
                            label: "Overall Sustainability",
                            value: environmentalScore,
                        },
                    ].map(({ label, value }) => (
                        <div key={label}>
                            <ProgressLabel>
                                <span>{label}</span>
                                <span>
                                    {Math.round(value)}
                                    %
                                </span>
                            </ProgressLabel>
                            <LinearProgress
                                variant="determinate"
                                value={value}
                                sx={{
                                    height: 6,
                                    borderRadius: 3,
                                    backgroundColor: grey[200],
                                    "& .MuiLinearProgress-bar": {
                                        backgroundColor: getScoreColor(value),
                                        borderRadius: 3,
                                    },
                                }}
                            />
                        </div>
                    ))}
                </Stack>
            </Section>
        </Container>
    )
}

export default EnvironmentalImpactTab
