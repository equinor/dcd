import { Typography } from "@mui/material"
import styled from "styled-components"
import { grey } from "@mui/material/colors"
import { AgChartsTimeseries } from "@/Components/AgGrid/AgChartsTimeseries"

const Container = styled.div`
    padding: 20px;
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

const ValueGrid = styled.div`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 16px;
`

const ValueCard = styled.div`
    background: ${grey[50]};
    padding: 12px;
    border-radius: 4px;
`

const ValueLabel = styled(Typography)`
    color: ${grey[600]};
    font-size: 12px;
    margin-bottom: 4px;
`

const Value = styled(Typography)`
    font-family: monospace;
    font-size: 14px;
    font-weight: 500;
`

type TrendType = "up" | "down" | "neutral"

/* eslint-disable react/no-unused-prop-types */
interface TimeSeriesDataItem {
    label: string
    value: string
    trend?: TrendType
}
/* eslint-enable react/no-unused-prop-types */

const TrendIndicator = styled.span<{ trend: TrendType }>`
    color: ${(props) => {
        switch (props.trend) {
        case "up": return "#4caf50"
        case "down": return "#f44336"
        default: return grey[600]
        }
    }};
    margin-left: 8px;
`

interface Props {
    rowData: any
    dg4Year: number
}

const TimeSeriesTab = ({ rowData, dg4Year }: Props) => {
    const profile = rowData.overrideProfile?.override ? rowData.overrideProfile : rowData.profile
    if (!profile) { return null }

    const chartData = profile.values.map((value: number, index: number) => ({
        year: dg4Year + profile.startYear + index,
        [rowData.profileName]: value,
    }))

    const calculatePercentageChange = (newValue: number, oldValue: number): string => {
        // Handle cases where old value is 0 or missing
        if (oldValue === 0 || !oldValue) {
            if (newValue === 0) { return "0%" }
            return "New entry"
        }
        return `${(((newValue - oldValue) / oldValue) * 100).toFixed(1)}%`
    }

    const determineTrend = (newValue: number, oldValue: number): TrendType => {
        if (oldValue === 0 || !oldValue) { return "neutral" }
        const change = ((newValue - oldValue) / oldValue) * 100
        if (change > 1) { return "up" }
        if (change < -1) { return "down" }
        return "neutral"
    }

    const getStatistics = (): TimeSeriesDataItem[] | null => {
        const { values } = profile
        if (!values?.length) { return null }

        const sum = values.reduce((a: number, b: number) => a + b, 0)
        const avg = sum / values.length
        const max = Math.max(...values)
        const min = Math.min(...values)

        // Calculate year-over-year changes for non-zero/non-missing values only
        const yoyChanges = values.slice(1).map((value: number, index: number) => {
            const prevValue = values[index]
            if (prevValue === 0 || !prevValue) { return null }
            return ((value - prevValue) / prevValue) * 100
        }).filter((change: number | null): change is number => change !== null)

        const avgYoyChange = yoyChanges.length > 0
            ? yoyChanges.reduce((a: number, b: number) => a + b, 0) / yoyChanges.length
            : 0
        const trend: TrendType = (() => {
            if (avgYoyChange > 1) { return "up" }
            if (avgYoyChange < -1) { return "down" }
            return "neutral"
        })()

        // Calculate rate of change using first and last non-zero values
        const nonZeroValues = values.filter((v: number) => v !== 0)
        const firstValue = nonZeroValues[0] || 0
        const lastValue = nonZeroValues[nonZeroValues.length - 1] || 0
        const totalChange = firstValue === 0 ? 0 : ((lastValue - firstValue) / firstValue) * 100
        const annualizedChange = values.length > 1 ? totalChange / (values.length - 1) : 0

        return [
            { label: "Total", value: sum.toFixed(2) },
            { label: "Average", value: avg.toFixed(2) },
            { label: "Maximum", value: max.toFixed(2) },
            { label: "Minimum", value: min.toFixed(2) },
            { label: "Data Points", value: values.length.toString() },
            {
                label: "Avg Annual Change",
                value: yoyChanges.length > 0 ? `${annualizedChange.toFixed(1)}%` : "N/A",
                trend: yoyChanges.length > 0 ? trend : "neutral",
            },
            {
                label: "Total Change",
                value: firstValue === 0 ? "N/A" : `${totalChange.toFixed(1)}%`,
                trend: (() => {
                    if (firstValue === 0) { return "neutral" }
                    if (totalChange > 0) { return "up" }
                    if (totalChange < 0) { return "down" }
                    return "neutral"
                })(),
            },
        ]
    }

    const getYearOverYearChanges = (): TimeSeriesDataItem[] | null => {
        if (!profile.values || profile.values.length < 2) { return null }

        return profile.values.slice(1).map((value: number, index: number) => {
            const year = dg4Year + profile.startYear + index + 1
            const prevValue = profile.values[index]
            const changeValue = calculatePercentageChange(value, prevValue)
            const trend = determineTrend(value, prevValue)

            return {
                label: `${year} vs ${year - 1}`,
                value: changeValue,
                trend,
            }
        })
    }

    const renderTrendIndicator = (trend: TrendType | undefined) => {
        if (!trend) { return null }
        const arrows = {
            up: "↑",
            down: "↓",
            neutral: "→",
        }
        return (
            <TrendIndicator trend={trend}>
                {arrows[trend]}
            </TrendIndicator>
        )
    }

    return (
        <Container>
            <Section>
                <AgChartsTimeseries
                    data={chartData}
                    chartTitle={rowData.profileName}
                    barColors={["#007079"]}
                    barProfiles={[rowData.profileName]}
                    barNames={[rowData.profileName]}
                    unit={rowData.unit}
                    height="300px"
                />
            </Section>

            <Section>
                <SectionTitle>
                    Statistics
                </SectionTitle>
                <ValueGrid>
                    {getStatistics()?.map(({ label, value, trend }: TimeSeriesDataItem) => (
                        <ValueCard key={label}>
                            <ValueLabel variant="caption">
                                {label}
                            </ValueLabel>
                            <Value>
                                {value}
                                {renderTrendIndicator(trend)}
                                {label !== "Data Points" && !value.includes("%") && rowData.unit}
                            </Value>
                        </ValueCard>
                    ))}
                </ValueGrid>
            </Section>

            <Section>
                <SectionTitle>
                    Year-over-Year Changes
                </SectionTitle>
                <ValueGrid>
                    {getYearOverYearChanges()?.map(({ label, value, trend }: TimeSeriesDataItem) => (
                        <ValueCard key={label}>
                            <ValueLabel variant="caption">
                                {label}
                            </ValueLabel>
                            <Value>
                                {value}
                                {renderTrendIndicator(trend)}
                            </Value>
                        </ValueCard>
                    ))}
                </ValueGrid>
            </Section>
        </Container>
    )
}

export default TimeSeriesTab
